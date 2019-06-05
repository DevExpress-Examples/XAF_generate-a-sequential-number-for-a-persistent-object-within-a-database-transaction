#if DEBUG
using System;
using System.Linq;
using DevExpress.ExpressApp;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevExpress.ExpressApp.Actions;
using Demo.Module.DatabaseUpdate;

namespace Demo.Module.Controllers {
    public partial class TestListViewController : ViewController {
        private const int MaxTestersCount = 10;
        private SimpleAction testAction;
        public TestListViewController() {
            testAction = new SimpleAction(this, "Test", "View");
            TargetViewType = ViewType.ListView;
            testAction.Execute += testAction_Execute;
        }
        void testAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            testAction.Caption = "Test is running...";
            Task[] tasks = new Task[MaxTestersCount];
            for (int i = 0; i < MaxTestersCount; i++) {
                tasks[i] = Task.Factory.StartNew(() => {
                    for (int j = 0; j < 50; j++) {
                        using (IObjectSpace os = Application.CreateObjectSpace(typeof(Demo.Module.BusinessObjects.Contact))) {
                            try {
                                DatabaseHelper.CreateContact(os);
                                DatabaseHelper.CreateAddress(os);
                                DatabaseHelper.CreateDocument(os);
                                os.CommitChanges();
                            } catch {
                                os.Rollback();
                                throw;
                            }
                        }
                    }
                }
                );
            }
            try {
                Task.WaitAll(tasks);
            } catch {
                testAction.Caption = "Failed";
            }
            View.ObjectSpace.Refresh();
            testAction.Caption = "Succeeded";
            testAction.Enabled["Run only once"] = false;
        }
    }
}
#endif