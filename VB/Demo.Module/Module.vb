Imports System
Imports System.Collections.Generic

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating
Imports Demo.Module.BusinessObjects

Namespace Demo.Module
    Public NotInheritable Partial Class DemoModule
        Inherits ModuleBase

        Public Sub New()
            InitializeComponent()
        End Sub
        Public Overrides Function GetModuleUpdaters(ByVal objectSpace As IObjectSpace, ByVal versionFromDB As Version) As IEnumerable(Of ModuleUpdater)
            Dim updater As ModuleUpdater = New DatabaseUpdate.Updater(objectSpace, versionFromDB)
            Return New ModuleUpdater() { updater }
        End Function
        Public Overrides Sub Setup(ByVal application As XafApplication)
            MyBase.Setup(application)
            XafTypesInfo.Instance.RegisterEntity("Document", GetType(IDocument), GetType(GenerateUserFriendlyId.Module.BusinessObjects.UserFriendlyIdPersistentObject))
        End Sub
    End Class
End Namespace
