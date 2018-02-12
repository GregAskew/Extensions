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
    public class XElementExtensionsUnitTests {

        private string xElementTestString1 = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Docroot><Entry><cn Guid=\"{54849625-5478-4994-A5BA-3E3B0328C30D}\">jsmith001</cn><telephonenumber>301-555-1212</telephonenumber><telephonenumber>202-555-1212</telephonenumber></Entry><Entry><cn Guid=\"{54849625-5478-4994-A5BA-3E3B0328C30F}\">jsmith002</cn><telephonenumber>301-555-1212</telephonenumber><telephonenumber>202-555-1212</telephonenumber></Entry></Docroot>";
        private string xElementTestString2 = "<Event xmlns=\"http://schemas.microsoft.com/win/2004/08/events/event\"><EventData><Data Name=\"UtcTime\">2015-10-24 15:22:05.411</Data><Data Name=\"SourceProcessGuid\">{B3DFD171-D20D-5627-0000-00103710AC04}</Data><Data Name=\"SourceProcessId\">3772</Data><Data Name=\"SourceImage\">C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe</Data><Data Name=\"TargetProcessGuid\">{B3DFD171-A21C-562B-0000-0010EF16190D}</Data><Data Name=\"TargetProcessId\">3200</Data><Data Name=\"TargetImage\">C:\\Windows\\SysWOW64\\Macromed\\Flash\\FlashPlayerPlugin_19_0_0_226.exe</Data><Data Name=\"NewThreadId\">8088</Data><Data Name=\"StartAddress\">0x0000000004B7119C</Data><Data Name=\"StartModule\"></Data><Data Name=\"StartFunction\"></Data></EventData></Event>";

        [TestMethod]
        public void Test_GetAttributeValue() {
            var expectedGuid1 = Guid.Parse("{54849625-5478-4994-A5BA-3E3B0328C30D}");
            var expectedGuid2 = Guid.Parse("{54849625-5478-4994-A5BA-3E3B0328C30F}");

            var rootElement = XElement.Parse(xElementTestString1);
            var entryElements = rootElement.Elements("Entry");

            var guids = new List<Guid>();
            foreach (var element in entryElements) {
                var cnElement = element.Elements()
                    .Where(x => x.Name.LocalName == "cn")
                    .FirstOrDefault();
                Assert.IsNotNull(cnElement);
                var guid = cnElement.GetAttributeValue<Guid>("Guid");
                Assert.AreNotEqual(notExpected: Guid.Empty, actual: guid);
                guids.Add(guid);
            }

            Assert.AreEqual(expected: 2, actual: guids.Count);
            Assert.IsTrue(guids.Any(x => x == expectedGuid1));
            Assert.IsTrue(guids.Any(x => x == expectedGuid2));
        }

        [TestMethod]
        public void Test_GetElementValue() {
            var rootElement = XElement.Parse(xElementTestString1);
            var entryElements = rootElement.Elements("Entry");

            var cns = new List<string>();
            foreach (var element in entryElements) {
                var cnElementValue = element.GetElementValue<string>("cn");
                Assert.IsNotNull(cnElementValue);
                cns.Add(cnElementValue);
            }

            Assert.AreEqual(expected: 2, actual: cns.Count);
            Assert.IsTrue(cns.Any(x => string.Equals(x, "jsmith001")));
            Assert.IsTrue(cns.Any(x => string.Equals(x, "jsmith002")));
        }

        [TestMethod]
        public void Test_GetElementValueForChildELement() {
            var expectedComputer = "CONTOSOMDDC1.CONTOSO.com";
            var elementText = "<Event xmlns=\"http://schemas.microsoft.com/win/2004/08/events/event\" xml:lang=\"en-US\">	<System><Computer>CONTOSOMDDC1.CONTOSO.com</Computer></System></Event>";
            var rootElement = XElement.Parse(elementText);
            var eventSystemElement = rootElement.Elements()
                .Where(x => x.Name.LocalName == "System")
                .FirstOrDefault();

            var computer = eventSystemElement.GetElementValue<string>("Computer");
            Assert.IsNotNull(computer);
            Assert.AreEqual(expected: expectedComputer, actual: computer);
        }

        [TestMethod]
        public void Test_GetElementValueForAttributeName() {
            var rootElement = XElement.Parse(xElementTestString2);
            var eventDataElement = rootElement.Elements()
                .Where(x => x.Name.LocalName == "EventData")
                .FirstOrDefault();

            var newThreadId = eventDataElement.GetElementValueForAttributeName<int>("Data", "Name", "NewThreadId");
            Assert.AreEqual(newThreadId, 8088);
            var sourceImage = eventDataElement.GetElementValueForAttributeName<string>("Data", "Name", "SourceImage");
            Assert.AreEqual(sourceImage, @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe");
            var sourceProcessGuid = eventDataElement.GetElementValueForAttributeName<Guid>("Data", "Name", "SourceProcessGuid");
            Assert.AreEqual(sourceProcessGuid, new Guid("{B3DFD171-D20D-5627-0000-00103710AC04}"));
            var sourceProcessId = eventDataElement.GetElementValueForAttributeName<int>("Data", "Name", "SourceProcessId");
            Assert.AreEqual(sourceProcessId, 3772);
            var startAddress = eventDataElement.GetElementValueForAttributeName<string>("Data", "Name", "StartAddress");
            Assert.AreEqual(startAddress, "0x0000000004B7119C");
            var startFunction = eventDataElement.GetElementValueForAttributeName<string>("Data", "Name", "StartFunction");
            Assert.AreEqual(startFunction, string.Empty);
            var startModule = eventDataElement.GetElementValueForAttributeName<string>("Data", "Name", "StartModule");
            Assert.AreEqual(startModule, string.Empty);
            var targetImage = eventDataElement.GetElementValueForAttributeName<string>("Data", "Name", "TargetImage");
            Assert.AreEqual(targetImage, @"C:\Windows\SysWOW64\Macromed\Flash\FlashPlayerPlugin_19_0_0_226.exe");
            var targetProcessGuid = eventDataElement.GetElementValueForAttributeName<Guid>("Data", "Name", "TargetProcessGuid");
            Assert.AreEqual(targetProcessGuid, new Guid("{B3DFD171-A21C-562B-0000-0010EF16190D}"));
            var targetProcessId = eventDataElement.GetElementValueForAttributeName<int>("Data", "Name", "TargetProcessId");
            Assert.AreEqual(targetProcessId, 3200);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_GetElementValue_Multivalued_Element_Throws_InvalidOperationException() {
            var rootElement = XElement.Parse(xElementTestString1);
            var elements = rootElement.Elements("Entry");

            foreach (var element in elements) {
                var telephonenumberElementValue = element.GetElementValue<string>("telephonenumber");
            }
        }

        [TestMethod]
        public void Test_GetElementValues() {
            var rootElement = XElement.Parse(xElementTestString1);
            var elements = rootElement.Elements("Entry");

            var expectedTelephoneNumber1 = "202-555-1212";
            var expectedTelephoneNumber2 = "301-555-1212";

            var telephoneNumbers = new List<string>();

            foreach (var element in elements) {
                var telephonenumberElementValues = element.GetElementValues<string>("telephonenumber");
                Assert.AreEqual(expected: 2, actual: telephonenumberElementValues.Count);
                telephoneNumbers.AddRange(telephonenumberElementValues);
            }

            Assert.AreEqual(expected: 4, actual: telephoneNumbers.Count);
            Assert.AreEqual(expected: 2, actual: telephoneNumbers.Where(x => string.Equals(x, expectedTelephoneNumber1)).Count());
            Assert.AreEqual(expected: 2, actual: telephoneNumbers.Where(x => string.Equals(x, expectedTelephoneNumber2)).Count());
        }
    }
}
