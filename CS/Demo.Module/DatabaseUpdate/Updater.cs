using System;
using Demo.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace Demo.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if(ObjectSpace.GetObjectsCount(typeof(Contact), null) == 0) {
                for(int i = 0; i < 10; i++) {
                    DatabaseHelper.CreateContact(ObjectSpace);
                    DatabaseHelper.CreateAddress(ObjectSpace);
                    DatabaseHelper.CreateDocument(ObjectSpace);
                    this.UpdateStatus("Creating test data", "", string.Format("Batch #{0} was created", i));
                }
            }
            ObjectSpace.CommitChanges();
        }
    }
}
