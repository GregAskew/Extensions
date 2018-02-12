namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    #endregion

    [TestClass]
    public class GuidExtensionsUnitTests {

        [TestMethod]
        public void Test_ToOctectString() {

            var testGuid = new Guid("51F78B5D-9705-4D86-B611-B2B26CA3EB4A");
            var guidOctetString = testGuid.ToOctectString();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(guidOctetString));
            Assert.AreEqual(guidOctetString, "\\5d\\8b\\f7\\51\\05\\97\\86\\4d\\b6\\11\\b2\\b2\\6c\\a3\\eb\\4a");

            testGuid = Guid.Empty;
            guidOctetString = testGuid.ToOctectString();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(guidOctetString));
            Assert.AreEqual(guidOctetString, "\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00");

        }
    }
}
