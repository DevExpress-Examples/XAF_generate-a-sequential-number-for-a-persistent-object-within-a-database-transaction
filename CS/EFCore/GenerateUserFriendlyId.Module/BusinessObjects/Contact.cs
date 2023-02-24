using System;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Module.BusinessObjects {
    [DefaultClassOptions]
    [DefaultProperty("FullName")]
    [ImageName("BO_Person")]
    public class Contact : BaseObject {
        public string ContactId {
            get { return $"A{SequentialNumber.ToString("D6")}"; }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long SequentialNumber { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int Age { get; set; }
        public virtual Sex Sex { get; set; }
        public virtual Address Address { get; set; }

        public string FullName {
            get { return $"{FirstName} {LastName}"; }
        }
    }
    public enum Sex {
        Male,
        Female
    }
}
