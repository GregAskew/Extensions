namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    #endregion

    [TestClass]
    public class StringExtensionsUnitTests {

        [TestMethod]
        public void Test_CountStringOccurrences() {
            var testString = "OU=Computers,OU=TSTOU,DC=contoso,DC=COM";
            var ouOccurrences = testString.CountStringOccurrences("OU=");
            Assert.AreEqual(ouOccurrences, 2);
            var commaOccurrences = testString.CountStringOccurrences(",", StringComparison.Ordinal);
            Assert.AreEqual(commaOccurrences, 3);
        }

        [TestMethod]
        public void Test_HexEncode() {
            var testString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?";
            var encoededString = testString.HexEncode();
            Assert.AreEqual(encoededString, "4100420043004400450046004700480049004A004B004C004D004E004F0050005100520053005400550056005700580059005A00310032003300340035003600370038003900300060007E00210040002300240025005E0026002A00280029002D005F003D002B005B005D007B007D005C007C003B003A00270022002C002E003C003E002F003F00");
        }

        [TestMethod]
        public void Test_UrlEncode() {
            var testString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?";
            var encodedString = testString.UrlEncode();
            Assert.AreEqual(encodedString, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%60%7E!%40%23%24%25%5E%26*()-_%3D%2B%5B%5D%7B%7D%5C%7C%3B%3A%27%22%2C.%3C%3E%2F%3F");
        }

        [TestMethod]
        public void Test_UrlDecode() {
            var testString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%60%7E!%40%23%24%25%5E%26*()-_%3D%2B%5B%5D%7B%7D%5C%7C%3B%3A%27%22%2C.%3C%3E%2F%3F";
            var encodedString = testString.UrlDecode();
            Assert.AreEqual(encodedString, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?");
        }

        [TestMethod]
        public void Test_IsGuid() {

            var guid = Guid.NewGuid();
            Assert.IsTrue(guid.ToString().IsGuid());

            var notGuid = "test123";
            Assert.IsFalse(notGuid.IsGuid());

        }

        [TestMethod]
        public void Test_IsValidEmail() {
            var email = "joe@contoso.com";
            Assert.IsTrue(email.IsValidEmail());

            email = string.Empty;
            Assert.IsFalse(email.IsValidEmail());

            email = "XXXXXXXXXXX";
            Assert.IsFalse(email.IsValidEmail());

            email = "joecontoso.com";
            Assert.IsFalse(email.IsValidEmail());

            email = "joe@contosocom";
            Assert.IsFalse(email.IsValidEmail());
        }

        [TestMethod]
        public void Test_IsValidIPV4Address() {
            var testIpAddress = "10.0.0.1";
            Assert.IsTrue(testIpAddress.IsValidIPV4Address());

            testIpAddress = "0.0.0.0";
            Assert.IsTrue(testIpAddress.IsValidIPV4Address());

            testIpAddress = "255.255.255.255";
            Assert.IsTrue(testIpAddress.IsValidIPV4Address());

            testIpAddress = string.Empty;
            Assert.IsFalse(testIpAddress.IsValidIPV4Address());

            testIpAddress = "123.456.789.0";
            Assert.IsFalse(testIpAddress.IsValidIPV4Address());

        }

        [TestMethod]
        public void Test_RemoveControlCharacters() {

            var controlCharacters = new List<char>();
            for (int index = 0; index < 32; index++) {
                controlCharacters.Add((char)index);
            }

            var referenceCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890";
            var info = new StringBuilder();
            for (int index = 0; index < controlCharacters.Count; index++) {
                info.Append(referenceCharacters[index]);
                info.Append(controlCharacters[index]);
            }

            var testString = info.ToString();
            Assert.IsFalse(testString.All(x => !char.IsControl(x)));
            testString = testString.RemoveControlCharacters();

            Assert.IsTrue(testString.All(x => !char.IsControl(x)));
            Assert.AreEqual(testString, "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z 0 1 2 3 4 5 ");

        }

        [TestMethod]
        public void Test_RemoveFormatCharacters() {

            #region Format characters
            //Char value decimal: 1536 hex: 600
            //Char value decimal: 1537 hex: 601
            //Char value decimal: 1538 hex: 602
            //Char value decimal: 1539 hex: 603
            //Char value decimal: 1757 hex: 6DD
            //Char value decimal: 1807 hex: 70F
            //Char value decimal: 6068 hex: 17B4
            //Char value decimal: 6069 hex: 17B5
            //Char value decimal: 8203 hex: 200B
            //Char value decimal: 8204 hex: 200C
            //Char value decimal: 8205 hex: 200D
            //Char value decimal: 8206 hex: 200E
            //Char value decimal: 8207 hex: 200F
            //Char value decimal: 8234 hex: 202A
            //Char value decimal: 8235 hex: 202B
            //Char value decimal: 8236 hex: 202C
            //Char value decimal: 8237 hex: 202D
            //Char value decimal: 8238 hex: 202E
            //Char value decimal: 8288 hex: 2060
            //Char value decimal: 8289 hex: 2061
            //Char value decimal: 8290 hex: 2062
            //Char value decimal: 8291 hex: 2063
            //Char value decimal: 8292 hex: 2064
            //Char value decimal: 8298 hex: 206A
            //Char value decimal: 8299 hex: 206B
            //Char value decimal: 8300 hex: 206C
            //Char value decimal: 8301 hex: 206D
            //Char value decimal: 8302 hex: 206E
            //Char value decimal: 8303 hex: 206F
            //Char value decimal: 65279 hex: FEFF
            //Char value decimal: 65529 hex: FFF9
            //Char value decimal: 65530 hex: FFFA
            //Char value decimal: 65531 hex: FFFB
            //Char value decimal: 67072 hex: 10600
            //Char value decimal: 67073 hex: 10601
            //Char value decimal: 67074 hex: 10602
            //Char value decimal: 67075 hex: 10603
            //Char value decimal: 67293 hex: 106DD
            //Char value decimal: 67343 hex: 1070F
            //Char value decimal: 71604 hex: 117B4
            //Char value decimal: 71605 hex: 117B5
            //Char value decimal: 73739 hex: 1200B
            //Char value decimal: 73740 hex: 1200C
            //Char value decimal: 73741 hex: 1200D
            //Char value decimal: 73742 hex: 1200E
            //Char value decimal: 73743 hex: 1200F
            //Char value decimal: 73770 hex: 1202A
            //Char value decimal: 73771 hex: 1202B
            //Char value decimal: 73772 hex: 1202C
            //Char value decimal: 73773 hex: 1202D
            //Char value decimal: 73774 hex: 1202E
            //Char value decimal: 73824 hex: 12060
            //Char value decimal: 73825 hex: 12061
            //Char value decimal: 73826 hex: 12062
            //Char value decimal: 73827 hex: 12063
            //Char value decimal: 73828 hex: 12064
            //Char value decimal: 73834 hex: 1206A
            //Char value decimal: 73835 hex: 1206B
            //Char value decimal: 73836 hex: 1206C
            //Char value decimal: 73837 hex: 1206D
            //Char value decimal: 73838 hex: 1206E
            //Char value decimal: 73839 hex: 1206F
            #endregion

            var formatCharacter = (char)int.Parse("202A", NumberStyles.AllowHexSpecifier);

            var testCharacters = new char[] { 'A', 'B', 'C', formatCharacter };

            var testString = new string(testCharacters);

            foreach (var c in testString) {
                Debug.WriteLine(char.GetUnicodeCategory(c));
            }

            Assert.IsFalse(testString.All(x => char.GetUnicodeCategory(x) != UnicodeCategory.Format));
            testString = testString.RemoveFormatCharacters();

            Assert.IsTrue(testString.All(x => char.GetUnicodeCategory(x) != UnicodeCategory.Format));
            Assert.AreEqual(testString, "ABC ");

        }

        [TestMethod]
        public void Test_RemoveInvalidFileNameCharacters() {
            var bogusFileName = @"test\/:*?<>|123.txt";
            var fileNameFixed = bogusFileName.RemoveInvalidFileNameCharacters();
            Assert.AreEqual("test--------123.txt", fileNameFixed);

            bogusFileName = @"test\/:*?<>|123.txt";
            fileNameFixed = bogusFileName.RemoveInvalidFileNameCharacters("");
            Assert.AreEqual("test123.txt", fileNameFixed);
        }

        [TestMethod]
        public void Test_RemoveNonAlphaNumericChars() {

            var controlCharacters = new List<char>();
            for (int index = 0; index < 32; index++) {
                controlCharacters.Add((char)index);
            }

            var referenceCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890";
            var info = new StringBuilder();
            for (int index = 0; index < controlCharacters.Count; index++) {
                info.Append(referenceCharacters[index]);
                info.Append(controlCharacters[index]);
            }


            info.Append((char)int.Parse("202A", NumberStyles.AllowHexSpecifier));

            var testString = info.ToString();
            Assert.IsFalse(testString.All(x => !char.IsLetterOrDigit(x)));
            testString = testString.RemoveNonAlphaNumericChars();

            Assert.IsTrue(testString.All(x => char.IsLetterOrDigit(x)));
            Assert.AreEqual(testString, "ABCDEFGHIJKLMNOPQRSTUVWXYZ012345");

        }

        [TestMethod]
        public void Test_Replace() {
            var testString = "ABCXYZ123SplatABCXYZ123SplatABCXYZ123SplatABCXYZ123";
            var oldValue = "splat";
            var newValue = "Weasel";

            var updatedString = testString.Replace(oldValue, newValue, StringComparison.OrdinalIgnoreCase);
            Assert.AreEqual(updatedString, "ABCXYZ123WeaselABCXYZ123WeaselABCXYZ123WeaselABCXYZ123");
        }

        [TestMethod]
        public void Test_ReverseString() {
            string testString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?";
            string testStringReversed = testString.ReverseString();
            Assert.AreEqual(testStringReversed, "?/><.,\"':;|\\}{][+=_-)(*&^%$#@!~`0987654321ZYXWVUTSRQPONMLKJIHGFEDCBA");
        }

        [TestMethod]
        public void Test_SafeString() {
            string testString = null;
            testString = testString.SafeString(true);
            Assert.IsNotNull(testString);

            testString = "ABC123 ";
            testString = testString.SafeString(true);
            Assert.IsNotNull(testString);
            Assert.AreEqual(testString, "ABC123");
        }

        [TestMethod]
        public void Test_SingleLabelName() {
            var testString = "server1.contoso.com";
            var singleLabelName = testString.SingleLabelName();

            Assert.IsNotNull(singleLabelName);
            Assert.AreEqual(singleLabelName, "server1");
        }

        [TestMethod]
        public void Test_ToTitleCase() {
            var text = "SMITH, JOHN";
            var titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith, John");

            text = "SMITH, john";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith, John");

            text = "smith, JOHN";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith, John");

            text = "smith, john";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith, John");

            text = "Smith, john";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith, John");

            text = "smith, John";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith, John");

            text = "o'smith, john";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "O'Smith, John");

            text = "smith Ii, John";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith II, John");

            text = "smith Iii, John";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith III, John");

            text = "smith Iv, John";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith IV, John");

            text = "John smith Ii";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "John Smith II");

            text = "John smith Iii";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "John Smith III");

            text = "John smith Iv";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "John Smith IV");

            text = "John allen smith-Jones";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "John Allen Smith-Jones");

            text = "John billy-bob smith";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "John Billy-Bob Smith");

            text = "smith jr, JOHN";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith Jr, John");

            text = "smith sr, JOHN";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "Smith Sr, John");

            text = "john smith jr";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "John Smith Jr");

            text = "john smith sr";
            titleCase = text.ToTitleCase();
            Assert.AreEqual(titleCase, "John Smith Sr");
        }

        [TestMethod]
        public void Test_FromBase64() {

            var testStringBase64 = "QQBCAEMAMQAyADMA";
            var testStringBytes = testStringBase64.FromBase64String();
            Assert.IsNotNull(testStringBytes);

            var testString = new string(Encoding.Unicode.GetChars(testStringBytes));
            Assert.IsNotNull(testString);
            Assert.AreEqual(testString, "ABC123");
        }

        [TestMethod]
        public void Test_FromBase64ToDecodedString() {
            var testString = "ABC123";
            var testStringBase64 = "QQBCAEMAMQAyADMA";

            var testStringBytes = Encoding.Unicode.GetBytes(testString);
            Assert.IsNotNull(testStringBytes);
            var testStringBase64Result = testStringBytes.ToBase64String();
            Assert.IsNotNull(testStringBase64Result);
            Assert.AreEqual(testStringBase64, testStringBase64Result);

            var testStringResult = testStringBase64.FromBase64ToDecodedString();

            Assert.IsNotNull(testStringResult);
            Assert.AreEqual(testString, testStringResult);
        }

        [TestMethod]
        public void Test_ToBase64() {
            var testString = "ABC123";
            var testStringBytes = Encoding.Unicode.GetBytes(testString);
            var testStringBase64 = testStringBytes.ToBase64String();
            Assert.IsNotNull(testStringBase64);
            Assert.AreEqual(testStringBase64, "QQBCAEMAMQAyADMA");

        }
    }
}
