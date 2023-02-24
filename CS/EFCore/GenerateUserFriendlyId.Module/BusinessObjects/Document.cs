using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Module.BusinessObjects {
    [DefaultClassOptions]
    [XafDefaultProperty("Title")]
    [ImageName("BO_Note")]
    public class Document : BaseObject {
        public virtual string DocumentId {
            get { return $"D{SequentialNumber.ToString("D6")}"; }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long SequentialNumber { get; set; }

        [RuleRequiredField("IDocument.Title.RuleRequiredField", DefaultContexts.Save)]
        [FieldSize(255)]
        public virtual string Title { get; set; }
        [FieldSize(8192)]
        public virtual string Text { get; set; }
    }
}
