Imports System
Imports Demo.Module.BusinessObjects
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
            If ObjectSpace.GetObjectsCount(GetType(Contact), Nothing) = 0 Then
                For i As Integer = 0 To 9
                    DatabaseHelper.CreateContact(ObjectSpace)
                    DatabaseHelper.CreateAddress(ObjectSpace)
                    DatabaseHelper.CreateDocument(ObjectSpace)
                    Me.UpdateStatus("Creating test data", "", String.Format("Batch #{0} was created", i))
                Next i
            End If
            ObjectSpace.CommitChanges()
        End Sub
    End Class
End Namespace
