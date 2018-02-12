namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    [TestClass]
    public class CharExtensionsUnitTests {

        [TestMethod]
        public void Test_GetAsciiDecimalValue() {
            var testString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?";
            foreach (char c in testString) {
                Debug.WriteLine($"Char: {c} Ascii DECIMAL value: {c.GetAsciiDecimalValue()}");
                if (c == 'A') {
                    Assert.AreEqual(c.GetAsciiDecimalValue(), "65");
                }
            }
        }

        [TestMethod]
        public void Test_GetAsciiHexValue() {
            var testString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?";
            foreach (char c in testString) {
                Debug.WriteLine($"Char: {c} Ascii HEX value: {c.GetAsciiHexValue()}");
                if (c == 'A') {
                    Assert.AreEqual(c.GetAsciiHexValue(), "41");
                }
            }
        }

        [TestMethod]
        public void Test_GetNumber() {
            char seven = '7';
            Assert.AreEqual(7, seven.GetNumber());
        }
    }
}
