using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using Demo.Module.BusinessObjects;
using Demo.Module.DatabaseUpdate;

namespace GenerateUserFriendlyId.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        if (ObjectSpace.GetObjectsCount(typeof(Contact), null) == 0) {
            for (int i = 0; i < 10; i++) {
                DatabaseHelper.CreateContact(ObjectSpace);
                DatabaseHelper.CreateAddress(ObjectSpace);
                DatabaseHelper.CreateDocument(ObjectSpace);
                this.UpdateStatus("Creating test data", "", string.Format("Batch #{0} was created", i));
            }
        }
        ObjectSpace.CommitChanges();
    }
    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
    }
}
