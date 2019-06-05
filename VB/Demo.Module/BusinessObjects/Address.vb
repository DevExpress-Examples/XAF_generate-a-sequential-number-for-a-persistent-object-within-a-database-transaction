Imports System
Imports DevExpress.Xpo
Imports System.ComponentModel
Imports System.Collections.Generic
Imports DevExpress.Persistent.Base
Imports GenerateUserFriendlyId.Module.BusinessObjects

Namespace Demo.Module.BusinessObjects
	<DefaultClassOptions>
	<DefaultProperty("FullAddress")>
	<ImageName("BO_Address")>
	Public Class Address
		Inherits UserFriendlyIdPersistentObject

'INSTANT VB NOTE: The field zipCode was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private zipCode_Renamed As String
'INSTANT VB NOTE: The field country was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private country_Renamed As String
'INSTANT VB NOTE: The field province was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private province_Renamed As String
'INSTANT VB NOTE: The field city was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private city_Renamed As String
'INSTANT VB NOTE: The field address1 was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private address1_Renamed As String
'INSTANT VB NOTE: The field address2 was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private address2_Renamed As String
		<PersistentAlias("Concat('A',PadLeft(ToStr(SequentialNumber),6,'0'))")>
		Public ReadOnly Property AddressId() As String
			Get
				Return Convert.ToString(EvaluateAlias("AddressId"))
			End Get
		End Property
		Public Property Province() As String
			Get
				Return province_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Province", province_Renamed, value)
			End Set
		End Property
		Public Property ZipCode() As String
			Get
				Return zipCode_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("ZipCode", zipCode_Renamed, value)
			End Set
		End Property
		Public Property Country() As String
			Get
				Return country_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Country", country_Renamed, value)
			End Set
		End Property
		Public Property City() As String
			Get
				Return city_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("City", city_Renamed, value)
			End Set
		End Property
		Public Property Address1() As String
			Get
				Return address1_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Address1", address1_Renamed, value)
			End Set
		End Property
		Public Property Address2() As String
			Get
				Return address2_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Address2", address2_Renamed, value)
			End Set
		End Property
		<Association>
		Public ReadOnly Property Persons() As XPCollection(Of Contact)
			Get
				Return GetCollection(Of Contact)("Persons")
			End Get
		End Property
		<PersistentAlias("concat(Country, Province, City, ZipCode)")>
		Public ReadOnly Property FullAddress() As String
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
