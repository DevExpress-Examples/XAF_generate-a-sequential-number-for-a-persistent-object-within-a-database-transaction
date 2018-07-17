using System;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using GenerateUserFriendlyId.Module.BusinessObjects;

namespace Demo.Module.BusinessObjects {
    [DefaultClassOptions]
    [DefaultProperty("FullAddress")]
    [ImageName("BO_Address")]
    public class Address : UserFriendlyIdPersistentObject {
        string zipCode;
        string country;
        string province;
        string city;
        string address1;
        string address2;
        [PersistentAlias("Concat('A',PadLeft(ToStr(SequentialNumber),6,'0'))")]
        public string AddressId {
            get {
                return Convert.ToString(EvaluateAlias("AddressId"));
            }
        }
        public string Province {
            get {
                return province;
            }
            set {
                SetPropertyValue("Province", ref province, value);
            }
        }
        public string ZipCode {
            get {
                return zipCode;
            }
            set {
                SetPropertyValue("ZipCode", ref zipCode, value);
            }
        }
        public string Country {
            get {
                return country;
            }
            set {
                SetPropertyValue("Country", ref country, value);
            }
        }
        public string City {
            get {
                return city;
            }
            set {
                SetPropertyValue("City", ref city, value);
            }
        }
        public string Address1 {
            get {
                return address1;
            }
            set {
                SetPropertyValue("Address1", ref address1, value);
            }
        }
        public string Address2 {
            get {
                return address2;
            }
            set {
                SetPropertyValue("Address2", ref address2, value);
            }
        }
        [Association]
        public XPCollection<Contact> Persons {
            get {
                return GetCollection<Contact>("Persons");
            }
        }
        [PersistentAlias("concat(Country, Province, City, ZipCode)")]
        public string FullAddress {
            get { return ObjectFormatter.Format("{Country}; {Province}; {City}; {ZipCode}", this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
        }
        public Address(Session session) : base(session) { }
    }
}
