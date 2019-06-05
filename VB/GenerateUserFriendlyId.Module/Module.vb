Imports DevExpress.ExpressApp
Imports GenerateUserFriendlyId.Module.BusinessObjects
Imports GenerateUserFriendlyId.Module

Namespace GenerateUserFriendlyId.Module
	Public NotInheritable Partial Class GenerateUserFriendlyIdModule
		Inherits ModuleBase

		Public Sub New()
			InitializeComponent()
		End Sub
		Protected Overrides Function GetDeclaredExportedTypes() As System.Collections.Generic.IEnumerable(Of System.Type)
			Return New System.Type() { GetType(Sequence) }
		End Function
	End Class
End Namespace