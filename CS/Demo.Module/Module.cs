using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using Demo.Module.BusinessObjects;

namespace Demo.Module {
    public sealed partial class DemoModule : ModuleBase {
        public DemoModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            XafTypesInfo.Instance.RegisterEntity("Document", typeof(IDocument), typeof(GenerateUserFriendlyId.Module.BusinessObjects.UserFriendlyIdPersistentObject));
        }
    }
}
