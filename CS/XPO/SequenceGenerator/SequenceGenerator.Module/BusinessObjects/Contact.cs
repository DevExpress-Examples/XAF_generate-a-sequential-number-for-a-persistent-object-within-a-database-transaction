using System;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using GenerateUserFriendlyId.Module.BusinessObjects;

namespace Demo.Module.BusinessObjects {
    [DefaultClassOptions]
    [DefaultProperty("FullName")]
    [ImageName("BO_Person")]
    public class Contact : UserFriendlyIdPersistentObject {
        [PersistentAlias("Concat('C',PadLeft(ToStr(SequentialNumber),6,'0'))")]
        public string ContactId {
            get { return Convert.ToString(EvaluateAlias("ContactId")); }
        }

        string firstName;
        string lastName;
  
        public string FirstName {
            get { return firstName; }
            set { SetPropertyValue("FirstName", ref firstName, value); }
        }
        public string LastName {
            get { return lastName; }
            set { SetPropertyValue("LastName", ref lastName, value); }
        }
        public Contact(Session session) : base(session) { }
    }
 
}
