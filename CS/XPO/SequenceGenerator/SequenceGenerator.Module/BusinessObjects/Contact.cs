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
using System.Diagnostics;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.SystemModule;
using GenerateUserFriendlyId.Module.BusinessObjects;

namespace dxTestSolution.Module.BusinessObjects {
     [DefaultClassOptions]
	  
    public class Contact : UserFriendlyIdPersistentObject { 
        public Contact(Session session)
            : base(session) {
        }
        [PersistentAlias("Concat('C',PadLeft(ToStr(SequentialNumber),6,'0'))")]
        public string ContactId {
            get { return Convert.ToString(EvaluateAlias(nameof(ContactId))); }
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        string _firstName;
        public string FirstName {
            get {
                return _firstName;
            }
            set {
                SetPropertyValue(nameof(FirstName), ref _firstName, value);
            }
        }
        string _lastName;
        public string LastName {
            get {
                return _lastName;
            }
            set {
                SetPropertyValue(nameof(LastName), ref _lastName, value);
            }
        }
		int _age;
        public int Age {
            get {
                return _age;
            }
            set {
                SetPropertyValue(nameof(Age), ref _age, value);
            }
        }
	
        [Association("Contact-Tasks")]
        public XPCollection<MyTask> Tasks {
            get {
                return GetCollection<MyTask>(nameof(Tasks));
            }
        }
		

    }
}
