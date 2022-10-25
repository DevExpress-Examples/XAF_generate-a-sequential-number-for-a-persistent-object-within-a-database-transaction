#If DEBUG
Imports DevExpress.ExpressApp
Imports System.Threading.Tasks
Imports DevExpress.ExpressApp.Actions
Imports Demo.Module.DatabaseUpdate

Namespace Demo.Module.Controllers

    Public Partial Class TestListViewController
        Inherits ViewController

        Private Const MaxTestersCount As Integer = 10

        Private testAction As SimpleAction

        Public Sub New()
            testAction = New SimpleAction(Me, "Test", "View")
            TargetViewType = ViewType.ListView
            AddHandler testAction.Execute, AddressOf testAction_Execute
        End Sub

        Private Sub testAction_Execute(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
            testAction.Caption = "Test is running..."
            Dim tasks As Task() = New Task(9) {}
            For i As Integer = 0 To MaxTestersCount - 1
                tasks(i) = Task.Factory.StartNew(Sub()
                    For j As Integer = 0 To 50 - 1
                        Using os As IObjectSpace = Application.CreateObjectSpace(GetType(BusinessObjects.Contact))
                            Try
                                CreateContact(os)
                                CreateAddress(os)
                                CreateDocument(os)
                                os.CommitChanges()
                            Catch
                                os.Rollback()
                                Throw
                            End Try
                        End Using
                    Next
                End Sub)
            Next

            Try
                Task.WaitAll(tasks)
            Catch
                testAction.Caption = "Failed"
            End Try

            View.ObjectSpace.Refresh()
            testAction.Caption = "Succeeded"
            testAction.Enabled("Run only once") = False
        End Sub
    End Class
End Namespace
#End If
