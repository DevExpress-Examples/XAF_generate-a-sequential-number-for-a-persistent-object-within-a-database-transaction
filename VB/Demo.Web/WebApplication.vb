Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Web
Imports DevExpress.ExpressApp.Xpo

Namespace GenerateUserFriendlyId.Web
    Partial Public Class GenerateUserFriendlyIdAspNetApplication
        Inherits WebApplication

        Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
        Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule
        Private module3 As GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule
        Private businessClassLibraryCustomizationModule1 As DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule
        Private demoModule1 As Demo.Module.DemoModule

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
            Dim provider As New XPObjectSpaceProvider(args.ConnectionString, args.Connection, True)
            GenerateUserFriendlyId.Module.SequenceGenerator.Initialize(Me, args.ConnectionString)
            args.ObjectSpaceProvider = provider
        End Sub
        Private Sub GenerateUserFriendlyIdAspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles Me.DatabaseVersionMismatch
#If EASYTEST Then
            e.Updater.Update()
            e.Handled = True
#Else
            If System.Diagnostics.Debugger.IsAttached Then
                e.Updater.Update()
                e.Handled = True
            Else
                Dim message As String = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application." & ControlChars.CrLf & "This error occurred  because the automatic database update was disabled when the application was started without debugging." & ControlChars.CrLf & "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " & "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " & "or manually create a database using the 'DBUpdater' tool." & ControlChars.CrLf & "Anyway, refer to the following help topics for more detailed information:" & ControlChars.CrLf & "'Update Application and Database Versions' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm" & ControlChars.CrLf & "'Database Security References' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument3237.htm" & ControlChars.CrLf & "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/"

                If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
                    message &= ControlChars.CrLf & ControlChars.CrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
                End If
                Throw New InvalidOperationException(message)
            End If
#End If
        End Sub

        Private Sub InitializeComponent()
            Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
            Me.module2 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
            Me.module3 = New GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule()
            Me.businessClassLibraryCustomizationModule1 = New DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule()
            Me.demoModule1 = New Demo.Module.DemoModule()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            ' 
            ' GenerateUserFriendlyIdAspNetApplication
            ' 
            Me.ApplicationName = "GenerateUserFriendlyId"
            Me.Modules.Add(Me.module1)
            Me.Modules.Add(Me.module2)
            Me.Modules.Add(Me.businessClassLibraryCustomizationModule1)
            Me.Modules.Add(Me.module3)
            Me.Modules.Add(Me.demoModule1)
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
    End Class
End Namespace
