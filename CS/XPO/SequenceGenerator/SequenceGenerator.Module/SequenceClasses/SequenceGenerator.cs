using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB.Exceptions;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.Extensions.Options;

namespace GenerateUserFriendlyId.Module {

    public class SequenceGeneratorOptions {
        public Func<IServiceProvider, string> GetConnectionString;
    }

    public class SequenceGeneratorProvider {
        readonly IOptions<SequenceGeneratorOptions> options;
        readonly IServiceProvider serviceProvider;
        static readonly Dictionary<string, SequenceGenerator> sequenceGenerators = new Dictionary<string, SequenceGenerator>();
        static readonly object syncRoot = new object();
        public SequenceGeneratorProvider(IServiceProvider serviceProvider, IOptions<SequenceGeneratorOptions> options) {
            this.options = options;
            this.serviceProvider = serviceProvider;
        }
        public SequenceGenerator GetSequenceGenerator() {
            string connectionString = options.Value.GetConnectionString(serviceProvider);
            lock (syncRoot) {
                SequenceGenerator generator;
                if (!sequenceGenerators.TryGetValue(connectionString, out generator)) {
                    var dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
                    var dataLayer = new SimpleDataLayer(XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary, dataStoreProvider.CreateUpdatingStore(false, out _));
                    generator = new SequenceGenerator(dataLayer);
                    sequenceGenerators[connectionString] = generator;
                }
                return generator;
            }
        }
    }

    //Dennis: This class is used to generate sequential numbers for persistent objects.
    //Use the GetNextSequence method to get the next number and the Accept method, to save these changes to the database.
    public class SequenceGenerator : IDisposable {
        public const int MaxGenerationAttemptsCount = 10;
        public const int MinGenerationAttemptsDelay = 100;
        private static object syncRoot = new Object();
        private ExplicitUnitOfWork explicitUnitOfWork;
        private IDataLayer dataLayer;
        public SequenceGenerator(IDataLayer dataLayer) {
            this.dataLayer = dataLayer;
        }

        public void EnsureSequencesUnlocked(IEnumerable<string> lockedSequenceTypes) {
            if (explicitUnitOfWork != null) {
                return;
            }
            int count = MaxGenerationAttemptsCount;
            while (true) {
                try {
                    lock (syncRoot) {
                        //Dennis: It is necessary to update all sequences because objects graphs may be complex enough, and so their sequences should be locked to avoid a deadlock.
                        XPCollection<Sequence> sequences = new XPCollection<Sequence>(ExplicitUnitOfWork, new InOperator("TypeName", lockedSequenceTypes), new SortProperty("TypeName", SortingDirection.Ascending));
                        foreach (Sequence seq in sequences) {
                            seq.Save();
                        }
                        ExplicitUnitOfWork.FlushChanges();
                    }
                    break;
                }
                catch (LockingException) {
                    Close();
                    count--;
                    if (count <= 0) {
                        throw;
                    }
                    Thread.Sleep(MinGenerationAttemptsDelay * count);
                }
            }
        }

        private ExplicitUnitOfWork ExplicitUnitOfWork {
            get {
                lock (syncRoot) {
                    if (explicitUnitOfWork == null) {
                        explicitUnitOfWork = new ExplicitUnitOfWork(dataLayer);
                    }
                    return explicitUnitOfWork;
                }
            }
        }

        public void Accept() {
            lock (syncRoot) {
                if (explicitUnitOfWork != null) {
                    ExplicitUnitOfWork.CommitChanges();
                }
            }
        }
        public void Close() {
            lock (syncRoot) {
                if (explicitUnitOfWork != null) {
                    if (explicitUnitOfWork.InTransaction) {
                        explicitUnitOfWork.RollbackTransaction();
                    }
                    explicitUnitOfWork.Dispose();
                    explicitUnitOfWork = null;
                }
            }
        }
        public void Dispose() {
            Close();
        }
        public long GetNextSequence(object theObject) {
            Guard.ArgumentNotNull(theObject, "theObject");
            return GetNextSequence(XafTypesInfo.Instance.FindTypeInfo(theObject.GetType()));
        }
        public long GetNextSequence(ITypeInfo typeInfo) {
            Guard.ArgumentNotNull(typeInfo, "typeInfo");
            return GetNextSequence(XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary.GetClassInfo(typeInfo.Type));
        }
        public long GetNextSequence(XPClassInfo classInfo) {
            return GetNextSequence(GetBaseSequenceName(classInfo));
        }
        public long GetNextSequence(string name) {
            lock (syncRoot) {
                var seq = ExplicitUnitOfWork.GetObjectByKey<Sequence>(name, true);
                if (seq == null) {
                    //throw new InvalidOperationException(string.Format("Sequence for the {0} type was not found.", name));
                    seq = CreateSequence(ExplicitUnitOfWork, name);
                }
                long nextSequence = seq.NextSequence;
                seq.NextSequence++;
                ExplicitUnitOfWork.FlushChanges();
                return nextSequence;
            }
        }
        public static string GetBaseSequenceName(XPClassInfo classInfo) {
            Guard.ArgumentNotNull(classInfo, "classInfo");
            XPClassInfo ci = classInfo;
            //Comment this code if you want to have the SequentialNumber column created in each derived class table.
            while(ci.BaseClass != null && ci.BaseClass.IsPersistent) {
                ci = ci.BaseClass;
            }
            return ci.FullName;
        }
        private static Sequence CreateSequence(UnitOfWork uow, string typeName) {
            Sequence seq = new Sequence(uow);
            seq.TypeName = typeName;
            seq.NextSequence = 1;
            return seq;
        }
        //Dennis: It is necessary to generate (only once) sequences for all the persistent types before using the GetNextSequence method.
        public static void RegisterSequences(IDataLayer dataLayer, IEnumerable<ITypeInfo> persistentTypes) {
            if(persistentTypes != null)
                using(UnitOfWork uow = new UnitOfWork(dataLayer)) {
                    XPCollection<Sequence> sequenceList = new XPCollection<Sequence>(uow);
                    Dictionary<string, bool> typeToExistsMap = new Dictionary<string, bool>();
                    foreach(Sequence seq in sequenceList) {
                        typeToExistsMap[seq.TypeName] = true;
                    }
                    foreach(ITypeInfo typeInfo in persistentTypes) {
                        ITypeInfo ti = typeInfo;
                        if(typeToExistsMap.ContainsKey(ti.FullName)) {
                            continue;
                        }
                        //Comment this code if you want to have the SequentialNumber column created in each derived class table.
                        while(ti.Base != null && ti.Base.IsPersistent) {
                            ti = ti.Base;
                        }
                        string typeName = ti.FullName;
                        //Dennis: This code is required for the Domain Components only.
                        if(ti.IsInterface && ti.IsPersistent) {
                            Type generatedEntityType = XpoTypesInfoHelper.GetXpoTypeInfoSource().GetGeneratedEntityType(ti.Type);
                            if(generatedEntityType != null) {
                                typeName = generatedEntityType.FullName;
                            }
                        }
                        if(typeToExistsMap.ContainsKey(typeName)) {
                            continue;
                        }
                        if(ti.IsPersistent) {
                            typeToExistsMap[typeName] = true;
                            CreateSequence(uow, typeName);
                        }
                    }
                    uow.CommitChanges();
                }
        }
    }
    //This persistent class is used to store last sequential number for persistent objects.
    public class Sequence : XPBaseObject {
        private string typeName;
        private long nextSequence;
        public Sequence(Session session)
            : base(session) {
        }
        [Key]
        //Dennis: The size should be enough to store a full type name. However, you cannot use unlimited size for key columns.
        [Size(1024)]
        public string TypeName {
            get { return typeName; }
            set { SetPropertyValue("TypeName", ref typeName, value); }
        }
        public long NextSequence {
            get { return nextSequence; }
            set { SetPropertyValue("NextSequence", ref nextSequence, value); }
        }
    }
    public interface ISupportSequentialNumber {
        long SequentialNumber { get; set; }
    }
}