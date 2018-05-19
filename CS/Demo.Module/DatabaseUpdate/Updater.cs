using System;
using System.Security.Principal;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace Demo.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            for(int i = 0; i < 5; i++) {
                DatabaseHelper.CreateContact(ObjectSpace);
                DatabaseHelper.CreateAddress(ObjectSpace);
                DatabaseHelper.CreateDocument(ObjectSpace);
                this.UpdateStatus("Creating test data", "", string.Format("Batch #{0} was created", i));
            }
            ObjectSpace.CommitChanges();
        }
    }
}
