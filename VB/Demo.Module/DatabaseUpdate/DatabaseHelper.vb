Imports System
Imports DevExpress.ExpressApp
Imports System.Security.Cryptography
Imports Demo.Module.BusinessObjects
Imports System.Runtime.InteropServices

Namespace Demo.Module.DatabaseUpdate

    Public Module DatabaseHelper

        Private maleNames As String() = New String() {"Denzel", "Stratford", "Leanian", "Patwin", "Renaldo", "Welford", "Maher", "Gregorio", "Roth", "Gawain", "Fiacre", "Coillcumhann", "Honi", "Westcot", "Walden", "Onfroi", "Merlow", "Atol", "Gimm", "Dumont", "Weorth", "Corcoran", "Sinley", "Perekin", "Galt"}

        Private femaleNames As String() = New String() {"Tequiefah", "Zina", "Hemi Skye", "Chiziana", "Adelie", "Afric", "Laquinta", "Molli", "Cimberleigh", "Morissa", "Alastriona", "Ailisa", "Leontina", "Aruba", "Marilda", "Ascencion", "Lidoine", "Winema", "Eraman", "Karline", "Edwinna", "Yseult", "Florencia", "Bethsaida", "Aminah"}

        Private lastNames As String() = New String() {"SMITH", "JOHNSON", "WILLIAMS", "JONES", "BROWN", "DAVIS", "MILLER", "WILSON", "MOORE", "TAYLOR", "ANDERSON", "THOMAS", "JACKSON", "WHITE", "HARRIS", "MARTIN", "THOMPSON", "GARCIA", "MARTINEZ", "ROBINSON", "CLARK", "RODRIGUEZ", "LEWIS", "LEE", "WALKER", "HALL", "ALLEN", "YOUNG", "HERNANDEZ", "KING", "WRIGHT", "LOPEZ", "HILL", "SCOTT", "GREEN", "ADAMS", "BAKER", "GONZALEZ", "NELSON", "CARTER", "MITCHELL", "PEREZ", "ROBERTS", "TURNER", "PHILLIPS", "CAMPBELL", "PARKER", "EVANS", "EDWARDS", "COLLINS", "STEWART", "SANCHEZ", "MORRIS", "ROGERS", "REED", "COOK", "MORGAN", "BELL", "MURPHY", "BAILEY", "RIVERA", "COOPER", "RICHARDSON", "COX", "HOWARD", "WARD", "TORRES", "PETERSON", "GRAY", "RAMIREZ", "JAMES", "WATSON", "BROOKS", "KELLY", "SANDERS", "PRICE", "BENNETT", "WOOD", "BARNES", "ROSS", "HENDERSON", "COLEMAN", "JENKINS", "PERRY", "POWELL", "LONG", "PATTERSON", "HUGHES", "FLORES", "WASHINGTON", "BUTLER", "SIMMONS", "FOSTER", "GONZALES", "BRYANT", "ALEXANDER", "RUSSELL", "GRIFFIN", "DIAZ", "HAYES", "MYERS", "FORD", "HAMILTON", "GRAHAM", "SULLIVAN", "WALLACE", "WOODS", "COLE", "WEST", "JORDAN", "OWENS", "REYNOLDS", "FISHER", "ELLIS", "HARRISON"}

        Private cityList As String() = New String() {"New York,New York", "Los Angeles,California", "Chicago,Illinois", "Houston,Texas", "Phoenix,Arizona", "Philadelphia,Pennsylvania", "San Antonio,Texas", "San Diego,California", "Dallas,Texas", "San Jose,California", "Detroit,Michigan", "San Francisco,California", "Jacksonville,Florida", "Indianapolis,Indiana", "Austin,Texas", "Columbus,Ohio", "Fort Worth,Texas", "Charlotte,North Carolina", "Memphis,Tennessee", "Boston,Massachusetts", "Baltimore,Maryland", "El Paso,Texas", "Seattle,Washington", "Denver,Colorado", "Nashville,Tennessee", "Milwaukee,Wisconsin", "Washington,District of Columbia", "Las Vegas,Nevada", "Louisville,Kentucky", "Portland,Oregon", "Oklahoma City,Oklahoma", "Tucson,Arizona", "Atlanta,Georgia", "Albuquerque,New Mexico", "Kansas City,Missouri", "Fresno,California", "Sacramento,California", "Long Beach,California", "Mesa,Arizona", "Omaha,Nebraska", "Virginia Beach,Virginia", "Miami,Florida", "Cleveland,Ohio", "Oakland,California", "Raleigh,North Carolina", "Colorado Springs,Colorado", "Tulsa,Oklahoma", "Odessa,Texas", "Boulder,Colorado"}

        <ThreadStatic>
        Private currentContactMaleName As Integer = 0

        <ThreadStatic>
        Private currentAddressCity As Integer = 0

        <ThreadStatic>
        Private currentContactLastName As Integer = 0

        <ThreadStatic>
        Private currentContactFemaleName As Integer = 0

        Private currentContactSex As Sex = Sex.Male

        Private Function GetNextName(<Out> ByRef sex As Sex) As String
            If currentContactSex = Sex.Male Then
                sex = Sex.Male
                currentContactSex = Sex.Female
                If currentContactMaleName >= maleNames.Length Then currentContactMaleName = 0
                Return maleNames(Math.Min(Threading.Interlocked.Increment(currentContactMaleName), currentContactMaleName - 1))
            End If

            sex = Sex.Female
            currentContactSex = Sex.Male
            If currentContactFemaleName >= femaleNames.Length Then currentContactFemaleName = 0
            Return femaleNames(Math.Min(Threading.Interlocked.Increment(currentContactFemaleName), currentContactFemaleName - 1))
        End Function

        Public Function CreateContact(ByVal objectSpace As IObjectSpace) As Contact
            Dim Contact As Contact = objectSpace.CreateObject(Of Contact)()
            Dim sex As Sex
            Contact.FirstName = GetNextName(sex)
            Contact.Sex = sex
            Contact.LastName = GetNextLastName()
            Contact.Age = Randomize.Next(70)
            Contact.Address = CreateAddress(objectSpace)
            Return Contact
        End Function

        Public Function CreateAddress(ByVal objectSpace As IObjectSpace) As Address
            Dim address As Address = objectSpace.CreateObject(Of Address)()
            address.Country = "USA"
            Dim province As String
            address.City = GetNextCity(province)
            address.Province = province
            Return address
        End Function

        Friend Function CreateDocument(ByVal objectSpace As IObjectSpace) As IDocument
            Dim document As IDocument = objectSpace.CreateObject(Of IDocument)()
            Dim data As Byte() = New Byte(7) {}
            Dim generator As RandomNumberGenerator = RandomNumberGenerator.Create()
            generator.GetBytes(data)
            document.Title = BitConverter.ToString(data)
            Return document
        End Function

        Private Function GetNextLastName() As String
            If currentContactLastName >= lastNames.Length Then currentContactLastName = 0
            Return lastNames(Math.Min(Threading.Interlocked.Increment(currentContactLastName), currentContactLastName - 1))
        End Function

        Private Function GetNextCity(<Out> ByRef province As String) As String
            If currentAddressCity >= cityList.Length Then currentAddressCity = 0
            Dim clItem As String() = cityList(Math.Min(Threading.Interlocked.Increment(currentAddressCity), currentAddressCity - 1)).Split(","c)
            province = clItem(1)
            Return clItem(0)
        End Function

        Private randomizeField As Random

        Private ReadOnly Property Randomize As Random
            Get
                If randomizeField Is Nothing Then randomizeField = New Random()
                Return randomizeField
            End Get
        End Property
    End Module
End Namespace
