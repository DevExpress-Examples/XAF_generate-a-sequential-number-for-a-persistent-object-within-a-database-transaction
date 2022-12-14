using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using GenerateUserFriendlyId.Module;

namespace GenerateUserFriendlyId.Module {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            //Dennis: It is necessary to register sequences for persistent types used in your application.
            SequenceGenerator.RegisterSequences(XafTypesInfo.Instance.PersistentTypes);
        }
    }
}