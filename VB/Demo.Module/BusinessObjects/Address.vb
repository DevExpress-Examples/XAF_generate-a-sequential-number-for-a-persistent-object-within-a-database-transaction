Imports System
Imports DevExpress.Xpo
Imports System.ComponentModel
Imports DevExpress.Persistent.Base
Imports GenerateUserFriendlyId.Module.BusinessObjects

Namespace Demo.Module.BusinessObjects

    <DefaultClassOptions>
    <DefaultProperty("FullAddress")>
    <ImageName("BO_Address")>
    Public Class Address
        Inherits UserFriendlyIdPersistentObject

        Private zipCodeField As String

        Private countryField As String

        Private provinceField As String

        Private cityField As String

        Private address1Field As String

        Private address2Field As String

        <PersistentAlias("Concat('A',PadLeft(ToStr(SequentialNumber),6,'0'))")>
        Public ReadOnly Property AddressId As String
            Get
                Return Convert.ToString(EvaluateAlias("AddressId"))
            End Get
        End Property

        Public Property Province As String
            Get
                Return provinceField
            End Get

            Set(ByVal value As String)
                SetPropertyValue("Province", provinceField, value)
            End Set
        End Property

        Public Property ZipCode As String
            Get
                Return zipCodeField
            End Get

            Set(ByVal value As String)
                SetPropertyValue("ZipCode", zipCodeField, value)
            End Set
        End Property

        Public Property Country As String
            Get
                Return countryField
            End Get

            Set(ByVal value As String)
                SetPropertyValue("Country", countryField, value)
            End Set
        End Property

        Public Property City As String
            Get
                Return cityField
            End Get

            Set(ByVal value As String)
                SetPropertyValue("City", cityField, value)
            End Set
        End Property

        Public Property Address1 As String
            Get
                Return address1Field
            End Get

            Set(ByVal value As String)
                SetPropertyValue("Address1", address1Field, value)
            End Set
        End Property

        Public Property Address2 As String
            Get
                Return address2Field
            End Get

            Set(ByVal value As String)
                SetPropertyValue("Address2", address2Field, value)
            End Set
        End Property

        <Association>
        Public ReadOnly Property Persons As XPCollection(Of Contact)
            Get
                Return GetCollection(Of Contact)("Persons")
            End Get
        End Property

        <PersistentAlias("concat(Country, Province, City, ZipCode)")>
        Public ReadOnly Property FullAddress As String
            Get
                Return ObjectFormatter.Format("{Country}; {Province}; {City}; {ZipCode}", Me, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty)
            End Get
        End Property

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        Protected Overrides Function GetSequenceName() As String
            Return String.Concat(ClassInfo.FullName, "-", Province.Replace(" ", "_"))
        End Function
    End Class
End Namespace
