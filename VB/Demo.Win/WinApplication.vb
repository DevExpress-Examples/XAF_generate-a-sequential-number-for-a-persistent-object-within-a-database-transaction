Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Win
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Xpo

Namespace GenerateUserFriendlyId.Win
    Partial Public Class GenerateUserFriendlyIdWindowsFormsApplication
        Inherits WinApplication

        Public Sub New()
            InitializeComponent()
            DelayedViewItemsInitialization = True
        End Sub
        Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
            GenerateUserFriendlyId.Module.SequenceGenerator.Initialize(Me, args.ConnectionString)
            Dim provider As New XPObjectSpaceProvider(args.ConnectionString, args.Connection, True)
            args.ObjectSpaceProvider = provider
        End Sub
        Private Sub GenerateUserFriendlyIdWindowsFormsApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
#If EASYTEST Then
            e.Updater.Update()
            e.Handled = True
#Else
            If System.Diagnostics.Debugger.IsAttached Then
                e.Updater.Update()
                e.Handled = True
            Else
                Throw New InvalidOperationException("The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application." & ControlChars.CrLf & "This error occurred  because the automatic database update was disabled when the application was started without debugging." & ControlChars.CrLf & "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " & "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " & "or manually create a database using the 'DBUpdater' tool." & ControlChars.CrLf & "Anyway, refer to the 'Update Application and Database Versions' help topic at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm " & "for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/")
            End If
#End If
        End Sub
        Private Sub GenerateUserFriendlyIdWindowsFormsApplication_CustomizeLanguagesList(ByVal sender As Object, ByVal e As CustomizeLanguagesListEventArgs) Handles MyBase.CustomizeLanguagesList
            Dim userLanguageName As String = System.Threading.Thread.CurrentThread.CurrentUICulture.Name
            If userLanguageName <> "en-US" AndAlso e.Languages.IndexOf(userLanguageName) = -1 Then
                e.Languages.Add(userLanguageName)
            End If
        End Sub
    End Class
End Namespace
