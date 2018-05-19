Imports System
Imports System.Security.Principal

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Namespace Demo.Module.DatabaseUpdate
    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub
        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            For i As Integer = 0 To 4
                DatabaseHelper.CreateContact(ObjectSpace)
                DatabaseHelper.CreateAddress(ObjectSpace)
                DatabaseHelper.CreateDocument(ObjectSpace)
                Me.UpdateStatus("Creating test data", "", String.Format("Batch #{0} was created", i))
            Next i
            ObjectSpace.CommitChanges()
        End Sub
    End Class
End Namespace
