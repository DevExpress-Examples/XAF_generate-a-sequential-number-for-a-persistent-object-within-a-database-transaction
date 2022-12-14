using System;
using DevExpress.ExpressApp;
using System.Security.Cryptography;
using Demo.Module.BusinessObjects;

namespace Demo.Module.DatabaseUpdate {
    public static class DatabaseHelper {
        static string[] maleNames = new string[] { "Denzel", "Stratford", "Leanian", "Patwin", "Renaldo", "Welford", "Maher", "Gregorio", "Roth",
            "Gawain", "Fiacre", "Coillcumhann", "Honi", "Westcot", "Walden", "Onfroi", "Merlow", "Atol", "Gimm", "Dumont", "Weorth", 
            "Corcoran", "Sinley", "Perekin", "Galt" };
        static string[] femaleNames = new string[] { "Tequiefah", "Zina", "Hemi Skye", "Chiziana", "Adelie", "Afric", "Laquinta", "Molli", "Cimberleigh",
            "Morissa", "Alastriona", "Ailisa", "Leontina", "Aruba", "Marilda", "Ascencion", "Lidoine", "Winema", "Eraman", "Karline", "Edwinna",
            "Yseult", "Florencia", "Bethsaida", "Aminah" };

        static string[] lastNames = new string[] { "SMITH", "JOHNSON", "WILLIAMS", "JONES", "BROWN", "DAVIS", "MILLER", "WILSON", "MOORE", "TAYLOR",
            "ANDERSON", "THOMAS", "JACKSON", "WHITE", "HARRIS", "MARTIN", "THOMPSON", "GARCIA", "MARTINEZ", "ROBINSON", "CLARK", "RODRIGUEZ",
            "LEWIS", "LEE", "WALKER", "HALL", "ALLEN", "YOUNG", "HERNANDEZ", "KING", "WRIGHT", "LOPEZ", "HILL", "SCOTT", "GREEN", "ADAMS", "BAKER",
            "GONZALEZ", "NELSON", "CARTER", "MITCHELL", "PEREZ", "ROBERTS", "TURNER", "PHILLIPS", "CAMPBELL", "PARKER", "EVANS", "EDWARDS", "COLLINS",
            "STEWART", "SANCHEZ", "MORRIS", "ROGERS", "REED", "COOK", "MORGAN", "BELL", "MURPHY", "BAILEY", "RIVERA", "COOPER", "RICHARDSON", "COX", "HOWARD",
            "WARD", "TORRES", "PETERSON", "GRAY", "RAMIREZ", "JAMES", "WATSON", "BROOKS", "KELLY", "SANDERS", "PRICE", "BENNETT", "WOOD", "BARNES", "ROSS", "HENDERSON",
            "COLEMAN", "JENKINS", "PERRY", "POWELL", "LONG", "PATTERSON", "HUGHES", "FLORES", "WASHINGTON", "BUTLER", "SIMMONS", "FOSTER", "GONZALES", "BRYANT", "ALEXANDER",
            "RUSSELL", "GRIFFIN", "DIAZ", "HAYES", "MYERS", "FORD", "HAMILTON", "GRAHAM", "SULLIVAN", "WALLACE", "WOODS", "COLE", "WEST", "JORDAN", "OWENS", "REYNOLDS", "FISHER", "ELLIS", "HARRISON" };

        static string[] cityList = new string[] { "New York,New York", "Los Angeles,California", "Chicago,Illinois", "Houston,Texas", "Phoenix,Arizona", "Philadelphia,Pennsylvania", 
            "San Antonio,Texas", "San Diego,California", "Dallas,Texas", "San Jose,California", "Detroit,Michigan", "San Francisco,California", "Jacksonville,Florida",
            "Indianapolis,Indiana", "Austin,Texas", "Columbus,Ohio", "Fort Worth,Texas", "Charlotte,North Carolina", "Memphis,Tennessee", "Boston,Massachusetts", 
            "Baltimore,Maryland", "El Paso,Texas", "Seattle,Washington", "Denver,Colorado", "Nashville,Tennessee", "Milwaukee,Wisconsin", "Washington,District of Columbia",
            "Las Vegas,Nevada", "Louisville,Kentucky", "Portland,Oregon", "Oklahoma City,Oklahoma", "Tucson,Arizona", "Atlanta,Georgia", "Albuquerque,New Mexico", "Kansas City,Missouri",
            "Fresno,California", "Sacramento,California", "Long Beach,California", "Mesa,Arizona", "Omaha,Nebraska", "Virginia Beach,Virginia", "Miami,Florida", "Cleveland,Ohio", "Oakland,California",
            "Raleigh,North Carolina", "Colorado Springs,Colorado", "Tulsa,Oklahoma", "Odessa,Texas", "Boulder,Colorado" };

        [ThreadStatic]
        static int currentContactMaleName = 0;
        [ThreadStatic]
        static int currentAddressCity = 0;
        [ThreadStatic]
        static int currentContactLastName = 0;
        [ThreadStatic]
        static int currentContactFemaleName = 0;
        static Sex currentContactSex = Sex.Male;
        static string GetNextName(out Sex sex) {
            if(currentContactSex == Sex.Male) {
                sex = Sex.Male;
                currentContactSex = Sex.Female;
                if(currentContactMaleName >= maleNames.Length) currentContactMaleName = 0;
                return maleNames[currentContactMaleName++];
            }
            sex = Sex.Female;
            currentContactSex = Sex.Male;
            if(currentContactFemaleName >= femaleNames.Length) 
                currentContactFemaleName = 0;
            return femaleNames[currentContactFemaleName++];
        }
        public static Contact CreateContact(IObjectSpace objectSpace) {
            Contact Contact = objectSpace.CreateObject<Contact>();
            Sex sex;
            Contact.FirstName = GetNextName(out sex);
            Contact.Sex = sex;
            Contact.LastName = GetNextLastName();
            Contact.Age = Randomize.Next(70);
            Contact.Address = CreateAddress(objectSpace);
            return Contact;
        }
        public static Address CreateAddress(IObjectSpace objectSpace) {
            Address address = objectSpace.CreateObject<Address>();
            address.Country = "USA";
            string province;
            address.City = GetNextCity(out province);
            address.Province = province;
            return address;
        }
        internal static IDocument CreateDocument(IObjectSpace objectSpace) {
            IDocument document = objectSpace.CreateObject<IDocument>();
            byte[] data = new byte[8];
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            generator.GetBytes(data);
            document.Title = BitConverter.ToString(data);
            return document;
        }
        static string GetNextLastName() {
            if(currentContactLastName >= lastNames.Length) currentContactLastName = 0;
            return lastNames[currentContactLastName++];
        }
        static string GetNextCity(out string province) {
            if(currentAddressCity >= cityList.Length) currentAddressCity = 0;
            string[] clItem = cityList[currentAddressCity++].Split(',');
            province = clItem[1];
            return clItem[0];
        }

        static Random randomize;
        private static Random Randomize {
            get {
                if(randomize == null) randomize = new Random();
                return randomize;
            }
        }
    }
}
