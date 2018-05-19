using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using GenerateUserFriendlyId.Module.BusinessObjects;

namespace Demo.Module.BusinessObjects {
    [DefaultClassOptions]
    [DomainComponent]
    [XafDefaultProperty("Title")]
    [ImageName("BO_Note")]
    public interface IDocument : IUserFriendlyIdDomainComponent {
        [Calculated("concat('D', ToStr(SequentialNumber))")]
        string DocumentId { get; }
        [RuleRequiredField("IDocument.Title.RuleRequiredField", DefaultContexts.Save)]
        [FieldSize(255)]
        string Title { get; set; }
        [FieldSize(8192)]
        string Text { get; set; }
    }
}
