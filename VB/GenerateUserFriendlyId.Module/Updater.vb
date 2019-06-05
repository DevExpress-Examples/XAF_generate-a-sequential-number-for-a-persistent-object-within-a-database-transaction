Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating
Imports GenerateUserFriendlyId.Module

Namespace GenerateUserFriendlyId.Module
	Public Class Updater
		Inherits ModuleUpdater

		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			'Dennis: It is necessary to register sequences for persistent types used in your application.
			SequenceGenerator.RegisterSequences(XafTypesInfo.Instance.PersistentTypes)
		End Sub
	End Class
End Namespace