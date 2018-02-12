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
    public class DateTimeExtensionsUnitTests {

        [TestMethod]
        public void Test_GetFirstDayOfMonth() {
            var datetime = new DateTime(2015, 1, 14);
            var lastDayOfMonthDateTime = new DateTime(2015, 1, 1);

            Assert.AreEqual(datetime.GetFirstDayOfMonth(), lastDayOfMonthDateTime);
        }

        [TestMethod]
        public void Test_GetLastDayOfMonth() {
            var datetime = new DateTime(2015, 1, 14);
            var lastDayOfMonthDateTime = new DateTime(2015, 1, 31);

            Assert.AreEqual(datetime.GetLastDayOfMonth(), lastDayOfMonthDateTime);
        }

        [TestMethod]
        public void Test_LongDateSqlFriendly() {
            var datetime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
            var datetimeString = datetime.LongDateSqlFriendly();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "2015-01-01 01:01");

            datetime = SqlDateTime.MinValue.Value;
            datetimeString = datetime.LongDateSqlFriendly();
            Assert.AreEqual(datetimeString, "N/A");
        }

        [TestMethod]
        public void Test_ShortDateSqlFriendly() {
            var datetime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
            var datetimeString = datetime.ShortDateSqlFriendly();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "2015-01-01");

            datetime = SqlDateTime.MinValue.Value;
            datetimeString = datetime.ShortDateSqlFriendly();
            Assert.AreEqual(datetimeString, "N/A");
        }

        [TestMethod]
        public void Test_ToRfc2068DateString() {
            var datetime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
            var datetimeString = datetime.ToRfc2068DateString();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "Thu, 01 Jan 2015 01:01:01 GMT");

        }

        [TestMethod]
        public void Test_VerboseString() {
            var datetime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
            var datetimeString = datetime.VerboseString();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "2015-01-01 01:01:01.001");

            long ticks = 635676411777163328;
            datetime = new DateTime(ticks);
            datetimeString = datetime.VerboseString();
            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "2015-05-19 14:06:17.71633");
        }

        [TestMethod]
        public void Test_YMDFriendly() {
            var datetime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
            var datetimeString = datetime.YMDFriendly();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "2015-01-01");

        }

        [TestMethod]
        public void Test_YMDHMFriendly() {
            var datetime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
            var datetimeString = datetime.YMDHMFriendly();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "2015-01-01 01:01");

        }

        [TestMethod]
        public void Test_YMDHMSFriendly() {
            var datetime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
            var datetimeString = datetime.YMDHMSFriendly();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "2015-01-01 01:01:01");

        }

        [TestMethod]
        public void Test_YMDHMSFFFFriendly() {
            var datetime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
            var datetimeString = datetime.YMDHMSFFFFriendly();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(datetimeString, "2015-01-01 01:01:01.001");

        }

        [TestMethod]
        public void Test_YMDHMSFFFFFFFFriendly() {
            var datetime = new DateTime(635945051100786037, DateTimeKind.Utc);
            var datetimeString = datetime.YMDHMSFFFFFFFFriendly();

            Assert.IsNotNull(datetimeString);
            Assert.AreEqual(27, datetimeString.Length);

        }
    }
}
