using System;
using DevExpress.Xpo;
using System.Threading;
using DevExpress.ExpressApp;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using DevExpress.Xpo.DB.Exceptions;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;

namespace GenerateUserFriendlyId.Module {
    //Dennis: This class is used to generate sequential numbers for persistent objects.
    //Use the GetNextSequence method to get the next number and the Accept method, to save these changes to the database.
    public class SequenceGenerator : IDisposable {
        public const int MaxGenerationAttemptsCount = 10;
        public const int MinGenerationAttemptsDelay = 100;
        private static volatile IDataLayer defaultDataLayer;
        private static object syncRoot = new Object();
        private ExplicitUnitOfWork euow;
        private Sequence seq;
        public SequenceGenerator(Dictionary<string, bool> lockedSequenceTypes) {
            int count = MaxGenerationAttemptsCount;
            while(true) {
                try {
                    euow = new ExplicitUnitOfWork(DefaultDataLayer);
                    //Dennis: It is necessary to update all sequences because objects graphs may be complex enough, and so their sequences should be locked to avoid a deadlock.
                    XPCollection<Sequence> sequences = new XPCollection<Sequence>(euow, new InOperator("TypeName", lockedSequenceTypes.Keys), new SortProperty("TypeName", SortingDirection.Ascending));
                    foreach(Sequence seq in sequences) {
                        seq.Save();
                    }
                    euow.FlushChanges();
                    break;
                } catch(LockingException) {
                    Close();
                    count--;
                    if(count <= 0) {
                        throw;
                    }
                    Thread.Sleep(MinGenerationAttemptsDelay * count);
                }
            }
        }
        public void Accept() {
            euow.CommitChanges();
        }
        public void Close() {
            if(euow != null) {
                if(euow.InTransaction) {
                    euow.RollbackTransaction();
                }
                euow.Dispose();
                euow = null;
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
            seq = euow.GetObjectByKey<Sequence>(name, true);
            if(seq == null) {
                //throw new InvalidOperationException(string.Format("Sequence for the {0} type was not found.", name));
                seq = CreateSequece(euow, name);
            }
            long nextSequence = seq.NextSequence;
            seq.NextSequence++;
            euow.FlushChanges();
            return nextSequence;
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
        private static Sequence CreateSequece(UnitOfWork uow, string typeName) {
            Sequence seq = new Sequence(uow);
            seq.TypeName = typeName;
            seq.NextSequence = 1;
            return seq;
        }
        //Dennis: It is necessary to generate (only once) sequences for all the persistent types before using the GetNextSequence method.
        public static void RegisterSequences(IEnumerable<ITypeInfo> persistentTypes) {
            if(persistentTypes != null)
                using(UnitOfWork uow = new UnitOfWork(DefaultDataLayer)) {
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
                            CreateSequece(uow, typeName);
                        }
                    }
                    uow.CommitChanges();
                }
        }
        //It may be necessary to use ValueManager if your application uses different databases for different users
        public static IDataLayer DefaultDataLayer {
            get {
                lock (syncRoot) {
                    if (defaultDataLayer == null) {
                        IDisposable[] disposableObjects;
                        defaultDataLayer = new SimpleDataLayer(XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary, DataStoreProvider.CreateUpdatingStore(false, out disposableObjects));
                    }
                    return defaultDataLayer;
                }
            }
        }
        public static void Initialize(IXpoDataStoreProvider dataStoreProvider) {
            Guard.ArgumentNotNull(dataStoreProvider, "dataStoreProvider");
            DataStoreProvider = dataStoreProvider;
        }
        protected static IXpoDataStoreProvider DataStoreProvider { get; private set; }
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