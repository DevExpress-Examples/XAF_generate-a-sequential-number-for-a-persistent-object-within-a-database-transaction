Imports DevExpress.ExpressApp.DC
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.Validation
Imports GenerateUserFriendlyId.Module.BusinessObjects

Namespace Demo.Module.BusinessObjects
    <DefaultClassOptions, DomainComponent, XafDefaultProperty("Title"), ImageName("BO_Note")> _
    Public Interface IDocument
        Inherits IUserFriendlyIdDomainComponent

        <Calculated("Concat('D',PadLeft(ToStr(SequentialNumber),6,'0'))")> _
        ReadOnly Property DocumentId() As String
        <RuleRequiredField("IDocument.Title.RuleRequiredField", DefaultContexts.Save), FieldSize(255)> _
        Property Title() As String
        <FieldSize(8192)> _
        Property Text() As String
    End Interface
End Namespace
