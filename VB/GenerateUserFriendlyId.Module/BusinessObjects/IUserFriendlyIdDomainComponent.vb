Imports DevExpress.ExpressApp.DC
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.Validation
Imports GenerateUserFriendlyId.Module

Namespace GenerateUserFriendlyId.Module.BusinessObjects
	 <DomainComponent>
	 Public Interface IUserFriendlyIdDomainComponent
		 Inherits ISupportSequentialNumber

	 End Interface
End Namespace