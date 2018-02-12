namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    #endregion

    [TestClass]
    public class TimeSpanExtensionsUnitTests {

        [TestMethod]
        public void Test_DHMSFriendly() {
            var timespan = new TimeSpan(1, 1, 1, 1, 1);
            var timespanString = timespan.DHMSFriendly();
            Assert.IsNotNull(timespanString);
            Assert.AreEqual(timespanString, "01.01:01:01");
        }

        [TestMethod]
        public void Test_HMSFriendly() {
            var timespan = new TimeSpan(1, 1, 1, 1, 1);
            var timespanString = timespan.HMSFriendly();
            Assert.IsNotNull(timespanString);
            Assert.AreEqual(timespanString, "01:01:01");
        }

        [TestMethod]
        public void Test_HMFriendly() {
            var timespan = new TimeSpan(1, 1, 1, 1, 1);
            var timespanString = timespan.HMFriendly();
            Assert.IsNotNull(timespanString);
            Assert.AreEqual(timespanString, "01:01");
        }
    }
}
