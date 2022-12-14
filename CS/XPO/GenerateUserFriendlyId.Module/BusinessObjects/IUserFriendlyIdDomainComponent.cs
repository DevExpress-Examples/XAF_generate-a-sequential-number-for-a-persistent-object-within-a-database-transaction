using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using GenerateUserFriendlyId.Module;

namespace GenerateUserFriendlyId.Module.BusinessObjects {
     [DomainComponent]
    public interface IUserFriendlyIdDomainComponent : ISupportSequentialNumber { }
}