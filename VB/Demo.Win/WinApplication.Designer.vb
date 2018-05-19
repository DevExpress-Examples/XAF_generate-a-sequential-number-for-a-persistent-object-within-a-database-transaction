Namespace GenerateUserFriendlyId.Win
    Partial Public Class GenerateUserFriendlyIdWindowsFormsApplication
        ''' <summary> 
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary> 
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Component Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
            Me.module2 = New DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule()
            Me.module3 = New GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule()
            Me.businessClassLibraryCustomizationModule1 = New DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule()
            Me.demoModule1 = New Demo.Module.DemoModule()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            ' 
            ' GenerateUserFriendlyIdWindowsFormsApplication
            ' 
            Me.ApplicationName = "GenerateUserFriendlyId"
            Me.Modules.Add(Me.module1)
            Me.Modules.Add(Me.module2)
            Me.Modules.Add(Me.businessClassLibraryCustomizationModule1)
            Me.Modules.Add(Me.module3)
            Me.Modules.Add(Me.demoModule1)
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub

        #End Region

        Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
        Private module2 As DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule
        Private module3 As GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule
        Private businessClassLibraryCustomizationModule1 As DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule
        Private demoModule1 As Demo.Module.DemoModule
    End Class
End Namespace
