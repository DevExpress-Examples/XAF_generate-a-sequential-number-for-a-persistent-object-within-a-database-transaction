Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports DevExpress.ExpressApp.Web
Imports DevExpress.ExpressApp.Web.TestScripts
Imports DevExpress.ExpressApp.Web.SystemModule

Public Partial Class ErrorPage
    Inherits Page

    Protected Overrides Sub InitializeCulture()
        If WebApplication.Instance IsNot Nothing Then Call WebApplication.Instance.InitializeCulture()
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim testScriptsManager As TestScriptsManager = New TestScriptsManager(Page)
        testScriptsManager.RegisterControl(JSLabelTestControl.ClassName, "FormCaption", TestControlType.Field, "FormCaption")
        testScriptsManager.RegisterControl(JSLabelTestControl.ClassName, "RequestUrl", TestControlType.Field, "RequestUrl")
        testScriptsManager.RegisterControl(JSLabelTestControl.ClassName, "DescriptionTextBox", TestControlType.Field, "Description")
        testScriptsManager.RegisterControl(JSDefaultTestControl.ClassName, "ReportButton", TestControlType.Action, "Report")
        testScriptsManager.AllControlRegistered("")
        If WebApplication.Instance IsNot Nothing Then
            ApplicationTitle.Text = WebApplication.Instance.Title
        Else
            ApplicationTitle.Text = "No application"
        End If

        Header.Title = "Application Error - " & ApplicationTitle.Text
        Dim errorInfo As ErrorInfo = ErrorHandling.GetApplicationError()
        If errorInfo IsNot Nothing Then
            RequestUrl.NavigateUrl = errorInfo.Url
            RequestUrl.Text = errorInfo.Url
            RequestUrl2.NavigateUrl = errorInfo.Url
            RequestUrl2.Text = errorInfo.Url
            If Not String.IsNullOrEmpty(errorInfo.UrlReferrer) Then
                HyperLinkReturn.NavigateUrl = errorInfo.UrlReferrer
            Else
                LiteralReturn.Visible = False
                HyperLinkReturn.Visible = False
            End If

            If ErrorHandling.CanShowDetailedInformation Then
                DetailsText.Text = errorInfo.GetTextualPresentation(True)
            Else
                Details.Visible = False
            End If

            ReportResult.Visible = False
            ReportForm.Visible = ErrorHandling.CanSendAlertToAdmin
        Else
            ErrorPanel.Visible = False
        End If
    End Sub

'#Region "Web Form Designer generated code"
    Overrides Protected Sub OnInit(ByVal e As EventArgs)
        InitializeComponent()
        MyBase.OnInit(e)
    End Sub

    Private Sub InitializeComponent()
        AddHandler Load, New EventHandler(AddressOf Page_Load)
        AddHandler PreRender, New EventHandler(AddressOf ErrorPage_PreRender)
    End Sub

    Private Sub ErrorPage_PreRender(ByVal sender As Object, ByVal e As EventArgs)
        Call RegisterThemeAssemblyController.RegisterThemeResources(CType(sender, Page))
    End Sub

'#End Region
    Protected Sub ReportButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim errorInfo As ErrorInfo = ErrorHandling.GetApplicationError()
        If errorInfo IsNot Nothing Then
            ErrorHandling.SendAlertToAdmin(errorInfo.Id, DescriptionTextBox.Text, errorInfo.Exception.Message)
            Call ErrorHandling.ClearApplicationError()
            ApologizeMessage.Visible = False
            ReportForm.Visible = False
            Details.Visible = False
            ReportResult.Visible = True
        End If
    End Sub
End Class
