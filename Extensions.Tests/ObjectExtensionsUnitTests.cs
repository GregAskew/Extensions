namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    #endregion

    [TestClass]
    public class ObjectExtensionsUnitTests {

        [Serializable]
        public class TestObject {
            public string Name { get; set; }
        }
        [Serializable]
        public class TestChildObject : TestObject {
            public string Location { get; set; }
            public List<string> TelephoneNumbers { get; set; }
        }

        private DataTable CreateTemplateTableAllColumnTypes() {
            // bool, byte, char, DateTime, decimal, double, Guid, short, int, long, single, string, TimeSpan,
            // ushort, uint, ulong, byte[]
            var dataTable = new DataTable();
            var c1 = new DataColumn("BoolTest", Type.GetType("System.Boolean"));
            var c2 = new DataColumn("ByteTest", Type.GetType("System.Byte"));
            var c3 = new DataColumn("CharTest", Type.GetType("System.Char"));
            var c4 = new DataColumn("DateTimeTest", Type.GetType("System.DateTime"));
            var c5 = new DataColumn("DecimalTest", Type.GetType("System.Decimal"));
            var c6 = new DataColumn("DoubleTest", Type.GetType("System.Double"));
            var c7 = new DataColumn("GuidTest", Type.GetType("System.Guid"));
            var c8 = new DataColumn("ShortTest", Type.GetType("System.Int16"));
            var c9 = new DataColumn("IntTest", Type.GetType("System.Int32"));
            var c10 = new DataColumn("LongTest", Type.GetType("System.Int64"));
            var c11 = new DataColumn("SingleTest", Type.GetType("System.Single"));
            var c12 = new DataColumn("StringTest", Type.GetType("System.String"));
            var c13 = new DataColumn("TimeSpanTest", Type.GetType("System.TimeSpan"));
            var c14 = new DataColumn("UShortTest", Type.GetType("System.UInt16"));
            var c15 = new DataColumn("UIntTest", Type.GetType("System.UInt32"));
            var c16 = new DataColumn("ULongTest", Type.GetType("System.UInt64"));
            var c17 = new DataColumn("ByteArrayTest", typeof(byte[]));
            var c18 = new DataColumn("NullTest", Type.GetType("System.String"));
            var c19 = new DataColumn("X", Type.GetType("System.String"));
            var c20 = new DataColumn("Y", Type.GetType("System.String"));
            var c21 = new DataColumn("Z", Type.GetType("System.String"));

            #region Add columns to table
            dataTable.Columns.Add(c1);
            dataTable.Columns.Add(c2);
            dataTable.Columns.Add(c3);
            dataTable.Columns.Add(c4);
            dataTable.Columns.Add(c5);
            dataTable.Columns.Add(c6);
            dataTable.Columns.Add(c7);
            dataTable.Columns.Add(c8);
            dataTable.Columns.Add(c9);
            dataTable.Columns.Add(c10);
            dataTable.Columns.Add(c11);
            dataTable.Columns.Add(c12);
            dataTable.Columns.Add(c13);
            dataTable.Columns.Add(c14);
            dataTable.Columns.Add(c15);
            dataTable.Columns.Add(c16);
            dataTable.Columns.Add(c17);
            dataTable.Columns.Add(c18);
            dataTable.Columns.Add(c19);
            dataTable.Columns.Add(c20);
            dataTable.Columns.Add(c21);
            #endregion

            var testDateTime = new DateTime(2103, 7, 1, 1, 15, 59);
            DataRow row1 = dataTable.NewRow();
            row1["BoolTest"] = true;
            row1["ByteTest"] = 1;
            row1["CharTest"] = 'A';
            row1["DateTimeTest"] = testDateTime;
            row1["DecimalTest"] = 123.456m;
            row1["DoubleTest"] = 123.456;
            row1["GuidTest"] = new Guid("956BD8E1-72F8-4756-AB1F-13B3DC6881AD");
            row1["ShortTest"] = 56;
            row1["IntTest"] = -1234567;
            row1["LongTest"] = 1234567;
            row1["StringTest"] = "Test 123";
            row1["SingleTest"] = .1f;
            row1["TimeSpanTest"] = new TimeSpan(testDateTime.Hour, testDateTime.Minute, testDateTime.Second);
            row1["UShortTest"] = 32767;
            row1["UIntTest"] = 7654321;
            row1["ULongTest"] = 7654321;
            row1["ByteArrayTest"] = new byte[] { };
            row1["NullTest"] = null;
            row1["X"] = "ABCDE12345";
            row1["Y"] = "ABCDE12345";
            row1["Z"] = "ABCDE12345";
            dataTable.Rows.Add(row1);

            DataRow row2 = dataTable.NewRow();
            row2["BoolTest"] = false;
            row2["ByteTest"] = 255;
            row2["CharTest"] = 'B';
            row2["DateTimeTest"] = testDateTime.AddMonths(1);
            row2["DecimalTest"] = 456.789m;
            row2["DoubleTest"] = 789;
            row2["GuidTest"] = new Guid("31120DE1-450D-4A42-9993-5B4B541EFB8E");
            row2["ShortTest"] = 56;
            row2["IntTest"] = -1234567;
            row2["LongTest"] = 1234567;
            row2["StringTest"] = "Test 123";
            row2["SingleTest"] = .1f;
            row2["TimeSpanTest"] = new TimeSpan(testDateTime.Hour, testDateTime.Minute, testDateTime.Second);
            row2["UShortTest"] = 32767;
            row2["UIntTest"] = 7654321;
            row2["ULongTest"] = 7654321;
            row2["ByteArrayTest"] = new byte[] { };
            row2["NullTest"] = null;
            row2["X"] = "FGHIJ67890";
            row2["Y"] = "FGHIJ67890";
            row2["Z"] = "FGHIJ67890";
            dataTable.Rows.Add(row2);

            DataRow row3 = dataTable.NewRow();
            row3["BoolTest"] = DBNull.Value;
            row3["ByteTest"] = DBNull.Value;
            row3["CharTest"] = DBNull.Value;
            row3["DateTimeTest"] = DBNull.Value;
            row3["DecimalTest"] = DBNull.Value;
            row3["DoubleTest"] = DBNull.Value;
            row3["GuidTest"] = DBNull.Value;
            row3["ShortTest"] = DBNull.Value;
            row3["IntTest"] = DBNull.Value;
            row3["LongTest"] = DBNull.Value;
            row3["StringTest"] = DBNull.Value;
            row3["SingleTest"] = DBNull.Value;
            row3["TimeSpanTest"] = DBNull.Value;
            row3["UShortTest"] = DBNull.Value;
            row3["UIntTest"] = DBNull.Value;
            row3["ULongTest"] = DBNull.Value;
            row3["ByteArrayTest"] = DBNull.Value;
            row3["NullTest"] = DBNull.Value;
            row3["X"] = DBNull.Value;
            row3["Y"] = DBNull.Value;
            row3["Z"] = DBNull.Value;
            dataTable.Rows.Add(row3);

            return dataTable;
        }

        [TestMethod]
        public void Test_Clone() {
            var testObject = new TestObject() { Name = "Somename " };
            var testObject2 = testObject.Clone<TestObject>();

            Assert.AreEqual(testObject.Name, testObject2.Name);
        }

        [TestMethod]
        public void Test_HandleDbNull() {
            var datatable = CreateTemplateTableAllColumnTypes();

            var allRowContents = new List<List<string>>();

            // get row data
            for (int rowIndex = 0; rowIndex < datatable.Rows.Count; rowIndex++) {

                DataRow dataRow = datatable.Rows[rowIndex];
                var rowContents = new List<string>();

                for (int columnIndex = 0; columnIndex < datatable.Columns.Count; columnIndex++) {
                    // get the data contents from the current column position, unless the value is null, return an empty string
                    var columnData = dataRow[columnIndex];
                    string rowCellContents = string.Empty;
                    string columnDataType = datatable.Columns[columnIndex].DataType.Name;

                    if (datatable.Columns[columnIndex].DataType == typeof(byte[])) {
                        rowCellContents = "<Binary Data>";
                    }
                    else {
                        rowCellContents = columnData.HandleDbNull<string>(string.Empty).Trim();
                    }
                    rowContents.Add(rowCellContents);

                } // for (int columnIndex = 0; columnIndex < datatable.Columns.Count; columnIndex++) {

                allRowContents.Add(rowContents);
            } // for (int rowIndex = 0; rowIndex < datatable.Rows.Count; rowIndex++) {
        }

        [TestMethod]
        public void Test_SafeString() {
            object testobject = null;
            Assert.IsNotNull(testobject.SafeString(true));

            testobject = "This is a string ";
            Assert.IsNotNull(testobject);
            Assert.AreEqual(testobject.SafeString(true), "This is a string");
        }

        [TestMethod]
        public void Test_ShallowCopy() {
            var testObject = new TestObject() { Name = "Somename " };
            var testChildObject = new TestChildObject();
            testObject.ShallowCopy<TestObject, TestChildObject>(testChildObject);

            Assert.AreEqual(testObject.Name, testChildObject.Name);

            var testObject2 = new TestObject();
            testObject.ShallowCopy<TestObject, TestObject>(testObject2);
            Assert.AreEqual(testObject.Name, testObject2.Name);
        }

        [TestMethod]
        public void Test_ToXml() {
            var testChildObject = new TestChildObject();
            testChildObject.Name = "Somename";
            testChildObject.Location = "Somelocation";
            testChildObject.TelephoneNumbers = new List<string>();
            testChildObject.TelephoneNumbers.Add("202-555-1212");
            testChildObject.TelephoneNumbers.Add("301-555-1212");

            var serializedXml = testChildObject.ToXml();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(serializedXml));
            Assert.IsTrue(serializedXml.Contains("<Name>Somename</Name>"));
            Assert.IsTrue(serializedXml.Contains("<Location>Somelocation</Location>"));
            Assert.IsTrue(serializedXml.Contains("<TelephoneNumbers>"));
            Assert.IsTrue(serializedXml.Contains("<string>202-555-1212</string>"));
            Assert.IsTrue(serializedXml.Contains("<string>301-555-1212</string>"));
            Assert.IsTrue(serializedXml.Contains("</TelephoneNumbers>"));


            //<?xml version="1.0"?>
            //<TestChildObject xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            //  <Name>Somename</Name>
            //  <Location>Somelocation</Location>
            //  <TelephoneNumbers>
            //    <string>202-555-1212</string>
            //    <string>301-555-1212</string>
            //  </TelephoneNumbers>
            //</TestChildObject>
        }

        [TestMethod]
        public void Test_TrimstringValues() {
            var testObject = new TestChildObject() { Location = "Somewhere ", Name = "Somename " };
            testObject.TrimStringValues();

            Assert.AreEqual(testObject.Location, "Somewhere");
            Assert.AreEqual(testObject.Name, "Somename");
        }

    }
}
