//,new MyUpdater(objectSpace,versionFromDB)
//            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/Contact_ListView", SecurityPermissionState.Allow);
            //defaultRole.AddTypePermissionsRecursively<Contact>(SecurityOperations.CRUDAccess, SecurityPermissionState.Allow);
using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

using Demo.Module.BusinessObjects;

namespace dxTestSolution.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class MyUpdater : ModuleUpdater {
        public MyUpdater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
		
            var cnt = ObjectSpace.GetObjects<Contact>().Count;
            if(cnt > 0) {
                return;
            }
            for (int i = 0; i < 5; i++) {
				var contact = ObjectSpace.CreateObject<Contact>();
				contact.FirstName = "FirstName" + i;
				contact.LastName = "LastName" + i;
				contact.Age = i * 10;
            }
            //secur#0  
			ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
        }

  

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
