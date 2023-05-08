Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating

Namespace Demo.Module.DatabaseUpdate

    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub

        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            For i As Integer = 0 To 5 - 1
                CreateContact(ObjectSpace)
                CreateAddress(ObjectSpace)
                CreateDocument(ObjectSpace)
                UpdateStatus("Creating test data", "", String.Format("Batch #{0} was created", i))
            Next

            ObjectSpace.CommitChanges()
        End Sub
    End Class
End Namespace
