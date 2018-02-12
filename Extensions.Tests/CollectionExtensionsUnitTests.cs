namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    [TestClass]
    public class CollectionExtensionsUnitTests {

        [TestMethod]
        public void Test_Sort() {
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dictionary.Add("XYZ", "XYZVALUE");
            dictionary.Add("ABC", "ABCVALUE");

            var sortedDictionary = dictionary.Sort<string, string>(StringComparer.OrdinalIgnoreCase);
            var sortedDictionaryKeys = sortedDictionary.Keys.ToList();

            Assert.AreEqual(sortedDictionaryKeys[0], "ABC");
            Assert.AreEqual(sortedDictionaryKeys[1], "XYZ");
        }

        [TestMethod]
        public void Test_AddListValueItemForGuidListString() {
            var dictionary = new Dictionary<Guid, List<string>>();
            var g1 = new Guid("A7386C03-F15A-457F-A2D0-72B5B31D2A21");
            dictionary.AddListItem(g1, "Test123");

            Assert.AreEqual(dictionary.Count, 1);
            Assert.AreEqual(dictionary[g1][0], "Test123");
        }

        [TestMethod]
        public void Test_ToQuotedCSVString() {
            var list = new List<string>();
            list.Add("ABC");
            list.Add("123");

            var csv = list.ToQuotedCSVString();

            Assert.AreEqual(csv, "\"ABC\",\"123\"");
        }

        [TestMethod]
        public void Test_ToDelimitedString() {
            var lines = new List<string>();
            var delimitedString = lines.ToDelimitedString();

            Assert.AreEqual("", delimitedString);

            lines.Add("1");
            lines.Add("2");
            lines.Add("");
            lines.Add("3");
            delimitedString = lines.ToDelimitedString();

            Assert.AreEqual("1;2;;3", delimitedString);

            var linesArray = new string[] { "1", "2", "", "3" };
            var delimitedStringArray = linesArray.ToDelimitedString();

            Assert.AreEqual("1;2;;3", delimitedStringArray);

        }
    }
}
