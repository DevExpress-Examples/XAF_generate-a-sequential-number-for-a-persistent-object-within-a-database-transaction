Imports System
Imports DevExpress.Xpo
Imports System.ComponentModel
Imports DevExpress.Persistent.Base
Imports GenerateUserFriendlyId.Module.BusinessObjects

Namespace Demo.Module.BusinessObjects

    <DefaultClassOptions>
    <DefaultProperty("FullName")>
    <ImageName("BO_Person")>
    Public Class Contact
        Inherits UserFriendlyIdPersistentObject

        <PersistentAlias("concat('C', ToStr(SequentialNumber))")>
        Public ReadOnly Property ContactId As String
            Get
                Return Convert.ToString(EvaluateAlias("ContactId"))
            End Get
        End Property

        Private firstNameField As String

        Private lastNameField As String

        Private sexField As Sex

        Private ageField As Integer

        Private addressField As Address

        Public Property FirstName As String
            Get
                Return firstNameField
            End Get

            Set(ByVal value As String)
                SetPropertyValue("FirstName", firstNameField, value)
            End Set
        End Property

        Public Property LastName As String
            Get
                Return lastNameField
            End Get

            Set(ByVal value As String)
                SetPropertyValue("LastName", lastNameField, value)
            End Set
        End Property

        Public Property Age As Integer
            Get
                Return ageField
            End Get

            Set(ByVal value As Integer)
                SetPropertyValue("Age", ageField, value)
            End Set
        End Property

        Public Property Sex As Sex
            Get
                Return sexField
            End Get

            Set(ByVal value As Sex)
                SetPropertyValue("Sex", sexField, value)
            End Set
        End Property

        <Association>
        Public Property Address As Address
            Get
                Return addressField
            End Get

            Set(ByVal value As Address)
                SetPropertyValue("Address", addressField, value)
            End Set
        End Property

        <PersistentAlias("concat(FirstName, LastName)")>
        Public ReadOnly Property FullName As String
            Get
                Return ObjectFormatter.Format("{FirstName} {LastName}", Me, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty)
            End Get
        End Property

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class

    Public Enum Sex
        Male
        Female
    End Enum
End Namespace
