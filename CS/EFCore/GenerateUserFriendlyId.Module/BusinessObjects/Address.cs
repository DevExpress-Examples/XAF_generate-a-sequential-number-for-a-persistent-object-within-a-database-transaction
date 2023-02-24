using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Module.BusinessObjects {
    [DefaultClassOptions]
    [DefaultProperty("FullAddress")]
    [ImageName("BO_Address")]
    public class Address : BaseObject {
        public string AddressId {
            get {
                return $"A{SequentialNumber.ToString("D6")}";
            }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long SequentialNumber { get; set; }

        public virtual string Province { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string City { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual ICollection<Contact> Persons { get; set; } = new ObservableCollection<Contact>();
       
        public string FullAddress {
            get { return $"{Country}, {Province}, {City}, {ZipCode}"; }
        }
    }
}
