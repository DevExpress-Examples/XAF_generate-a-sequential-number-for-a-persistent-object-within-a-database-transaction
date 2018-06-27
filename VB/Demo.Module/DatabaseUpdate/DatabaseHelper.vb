Imports System
Imports DevExpress.ExpressApp
Imports System.Security.Cryptography
Imports Demo.Module.BusinessObjects

Namespace Demo.Module.DatabaseUpdate
    Public NotInheritable Class DatabaseHelper

        Private Sub New()
        End Sub

        Private Shared maleNames() As String = { "Denzel", "Stratford", "Leanian", "Patwin", "Renaldo", "Welford", "Maher", "Gregorio", "Roth", "Gawain", "Fiacre", "Coillcumhann", "Honi", "Westcot", "Walden", "Onfroi", "Merlow", "Atol", "Gimm", "Dumont", "Weorth", "Corcoran", "Sinley", "Perekin", "Galt" }
        Private Shared femaleNames() As String = { "Tequiefah", "Zina", "Hemi Skye", "Chiziana", "Adelie", "Afric", "Laquinta", "Molli", "Cimberleigh", "Morissa", "Alastriona", "Ailisa", "Leontina", "Aruba", "Marilda", "Ascencion", "Lidoine", "Winema", "Eraman", "Karline", "Edwinna", "Yseult", "Florencia", "Bethsaida", "Aminah" }

        Private Shared lastNames() As String = { "SMITH", "JOHNSON", "WILLIAMS", "JONES", "BROWN", "DAVIS", "MILLER", "WILSON", "MOORE", "TAYLOR", "ANDERSON", "THOMAS", "JACKSON", "WHITE", "HARRIS", "MARTIN", "THOMPSON", "GARCIA", "MARTINEZ", "ROBINSON", "CLARK", "RODRIGUEZ", "LEWIS", "LEE", "WALKER", "HALL", "ALLEN", "YOUNG", "HERNANDEZ", "KING", "WRIGHT", "LOPEZ", "HILL", "SCOTT", "GREEN", "ADAMS", "BAKER", "GONZALEZ", "NELSON", "CARTER", "MITCHELL", "PEREZ", "ROBERTS", "TURNER", "PHILLIPS", "CAMPBELL", "PARKER", "EVANS", "EDWARDS", "COLLINS", "STEWART", "SANCHEZ", "MORRIS", "ROGERS", "REED", "COOK", "MORGAN", "BELL", "MURPHY", "BAILEY", "RIVERA", "COOPER", "RICHARDSON", "COX", "HOWARD", "WARD", "TORRES", "PETERSON", "GRAY", "RAMIREZ", "JAMES", "WATSON", "BROOKS", "KELLY", "SANDERS", "PRICE", "BENNETT", "WOOD", "BARNES", "ROSS", "HENDERSON", "COLEMAN", "JENKINS", "PERRY", "POWELL", "LONG", "PATTERSON", "HUGHES", "FLORES", "WASHINGTON", "BUTLER", "SIMMONS", "FOSTER", "GONZALES", "BRYANT", "ALEXANDER", "RUSSELL", "GRIFFIN", "DIAZ", "HAYES", "MYERS", "FORD", "HAMILTON", "GRAHAM", "SULLIVAN", "WALLACE", "WOODS", "COLE", "WEST", "JORDAN", "OWENS", "REYNOLDS", "FISHER", "ELLIS", "HARRISON" }

        Private Shared cityList() As String = { "New York,New York", "Los Angeles,California", "Chicago,Illinois", "Houston,Texas", "Phoenix,Arizona", "Philadelphia,Pennsylvania", "San Antonio,Texas", "San Diego,California", "Dallas,Texas", "San Jose,California", "Detroit,Michigan", "San Francisco,California", "Jacksonville,Florida", "Indianapolis,Indiana", "Austin,Texas", "Columbus,Ohio", "Fort Worth,Texas", "Charlotte,North Carolina", "Memphis,Tennessee", "Boston,Massachusetts", "Baltimore,Maryland", "El Paso,Texas", "Seattle,Washington", "Denver,Colorado", "Nashville,Tennessee", "Milwaukee,Wisconsin", "Washington,District of Columbia", "Las Vegas,Nevada", "Louisville,Kentucky", "Portland,Oregon", "Oklahoma City,Oklahoma", "Tucson,Arizona", "Atlanta,Georgia", "Albuquerque,New Mexico", "Kansas City,Missouri", "Fresno,California", "Sacramento,California", "Long Beach,California", "Mesa,Arizona", "Omaha,Nebraska", "Virginia Beach,Virginia", "Miami,Florida", "Cleveland,Ohio", "Oakland,California", "Raleigh,North Carolina", "Colorado Springs,Colorado", "Tulsa,Oklahoma", "Odessa,Texas", "Boulder,Colorado" }

        <ThreadStatic> _
        Private Shared currentContactMaleName As Integer = 0
        <ThreadStatic> _
        Private Shared currentAddressCity As Integer = 0
        <ThreadStatic> _
        Private Shared currentContactLastName As Integer = 0
        <ThreadStatic> _
        Private Shared currentContactFemaleName As Integer = 0
        Private Shared currentContactSex As Sex = Sex.Male
        Private Shared Function GetNextName(ByRef sex As Sex) As String
            If currentContactSex = Demo.Module.BusinessObjects.Sex.Male Then
                sex = Demo.Module.BusinessObjects.Sex.Male
                currentContactSex = Demo.Module.BusinessObjects.Sex.Female
                If currentContactMaleName >= maleNames.Length Then
                    currentContactMaleName = 0
                End If
                Dim tempVar = maleNames(currentContactMaleName)
                currentContactMaleName += 1
                Return tempVar
            End If
            sex = Demo.Module.BusinessObjects.Sex.Female
            currentContactSex = Demo.Module.BusinessObjects.Sex.Male
            If currentContactFemaleName >= femaleNames.Length Then
                currentContactFemaleName = 0
            End If
            Dim tempVar2 = femaleNames(currentContactFemaleName)
            currentContactFemaleName += 1
            Return tempVar2
        End Function
        Public Shared Function CreateContact(ByVal objectSpace As IObjectSpace) As Contact
            Dim Contact As Contact = objectSpace.CreateObject(Of Contact)()
            Dim sex As Sex = Nothing
            Contact.FirstName = GetNextName(sex)
            Contact.Sex = sex
            Contact.LastName = GetNextLastName()
            Contact.Age = Randomize.Next(70)
            Contact.Address = CreateAddress(objectSpace)
            Return Contact
        End Function
        Public Shared Function CreateAddress(ByVal objectSpace As IObjectSpace) As Address
            Dim address As Address = objectSpace.CreateObject(Of Address)()
            address.Country = "USA"
            Dim province As String = Nothing
            address.City = GetNextCity(province)
            address.Province = province
            Return address
        End Function
        Friend Shared Function CreateDocument(ByVal objectSpace As IObjectSpace) As IDocument
            Dim document As IDocument = objectSpace.CreateObject(Of IDocument)()
            Dim data(7) As Byte
            Dim generator As RandomNumberGenerator = RandomNumberGenerator.Create()
            generator.GetBytes(data)
            document.Title = BitConverter.ToString(data)
            Return document
        End Function
        Private Shared Function GetNextLastName() As String
            If currentContactLastName >= lastNames.Length Then
                currentContactLastName = 0
            End If
            Dim tempVar = lastNames(currentContactLastName)
            currentContactLastName += 1
            Return tempVar
        End Function
        Private Shared Function GetNextCity(ByRef province As String) As String
            If currentAddressCity >= cityList.Length Then
                currentAddressCity = 0
            End If
            Dim clItem() As String = cityList(currentAddressCity).Split(","c)
            currentAddressCity += 1
            province = clItem(1)
            Return clItem(0)
        End Function


        Private Shared randomize_Renamed As Random
        Private Shared ReadOnly Property Randomize() As Random
            Get
                If randomize_Renamed Is Nothing Then
                    randomize_Renamed = New Random()
                End If
                Return randomize_Renamed
            End Get
        End Property
    End Class
End Namespace
