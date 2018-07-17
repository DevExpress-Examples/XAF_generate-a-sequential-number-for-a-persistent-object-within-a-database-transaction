Imports System
Imports DevExpress.Xpo
Imports System.ComponentModel
Imports System.Collections.Generic
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports GenerateUserFriendlyId.Module.BusinessObjects

Namespace Demo.Module.BusinessObjects
    <DefaultClassOptions, DefaultProperty("FullName"), ImageName("BO_Person")> _
    Public Class Contact
        Inherits UserFriendlyIdPersistentObject

        <PersistentAlias("Concat('C',PadLeft(ToStr(SequentialNumber),6,'0'))")> _
        Public ReadOnly Property ContactId() As String
            Get
                Return Convert.ToString(EvaluateAlias("ContactId"))
            End Get
        End Property


        Private firstName_Renamed As String

        Private lastName_Renamed As String

        Private sex_Renamed As Sex

        Private age_Renamed As Integer

        Private address_Renamed As Address
        Public Property FirstName() As String
            Get
                Return firstName_Renamed
            End Get
            Set(ByVal value As String)
                SetPropertyValue("FirstName", firstName_Renamed, value)
            End Set
        End Property
        Public Property LastName() As String
            Get
                Return lastName_Renamed
            End Get
            Set(ByVal value As String)
                SetPropertyValue("LastName", lastName_Renamed, value)
            End Set
        End Property
        Public Property Age() As Integer
            Get
                Return age_Renamed
            End Get
            Set(ByVal value As Integer)
                SetPropertyValue("Age", age_Renamed, value)
            End Set
        End Property
        Public Property Sex() As Sex
            Get
                Return sex_Renamed
            End Get
            Set(ByVal value As Sex)
                SetPropertyValue("Sex", sex_Renamed, value)
            End Set
        End Property
        <Association> _
        Public Property Address() As Address
            Get
                Return address_Renamed
            End Get
            Set(ByVal value As Address)
                SetPropertyValue("Address", address_Renamed, value)
            End Set
        End Property
        <PersistentAlias("concat(FirstName, LastName)")> _
        Public ReadOnly Property FullName() As String
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
