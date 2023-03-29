using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.SystemModule;
using GenerateUserFriendlyId.Module.BusinessObjects;

namespace dxTestSolution.Module.BusinessObjects {
     [DefaultClassOptions]
     [DefaultProperty("Subject")]
    public class MyTask : UserFriendlyIdPersistentObject {
        public MyTask(Session session)
            : base(session) {
        }
        [PersistentAlias("Concat('T',PadLeft(ToStr(SequentialNumber),6,'0'))")]
        public string TaskId {
            get { return Convert.ToString(EvaluateAlias(nameof(TaskId))); }
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        string _subject;
        public string Subject {
            get {
                return _subject;
            }
            set {
                SetPropertyValue(nameof(Subject), ref _subject, value);
            }
        }
        Contact _assignedTo;
        [Association("Contact-Tasks")]
        public Contact AssignedTo {
            get {
                return _assignedTo;
            }
            set {
                SetPropertyValue(nameof(AssignedTo), ref _assignedTo, value);
            }
        }
        bool _isActive;
        public bool IsActive {
            get {
                return _isActive;
            }
            set {
                SetPropertyValue(nameof(IsActive), ref _isActive, value);
            }
        }
        Priority _priority;
        public Priority Priority {
            get {
                return _priority;
            }
            set {
                SetPropertyValue(nameof(Priority), ref _priority, value);
            }
        }
    }
    public enum Priority {
        [ImageName("State_Priority_Low")]
        Low = 0,
        [ImageName("State_Priority_Normal")]
        Normal = 1,
        [ImageName("State_Priority_High")]
        High = 2
    }
}