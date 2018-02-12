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
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    #endregion

    [TestClass]
    public class CertificateExtensionsUnitTests {

        [TestMethod]
        [StorePermission(SecurityAction.Demand, OpenStore = true, EnumerateCertificates = true)]
        public void Test_ExtensionsFriendly() {

            var storeName = StoreName.Root;
            var keyStore = new X509Store(storeName, StoreLocation.LocalMachine);

            // Use a well-known certificate
            X509Certificate2 x509TestCert = null;
            string x509TestCertName = "Microsoft Root Certificate Authority 2011";
            keyStore.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

            foreach (X509Certificate2 x509cert in keyStore.Certificates) {
                if (x509cert == null) continue;
                if (x509cert.Archived) continue;

                if (string.Equals(x509cert.FriendlyName, x509TestCertName, StringComparison.OrdinalIgnoreCase)) {
                    x509TestCert = x509cert;
                    break;
                }
            }

            Assert.IsNotNull(x509TestCert);
            Assert.IsNotNull(x509TestCert.ExtensionsFriendly());
            Assert.AreNotEqual(notExpected: 0, actual: x509TestCert.ExtensionsFriendly().Count);

            Assert.IsTrue(x509TestCert.ExtensionsFriendly().ContainsKey("Key Usage"));

            var keyUsageExpected = "Digital Signature, Certificate Signing, Off-line CRL Signing, CRL Signing (86)\r\n";
            var keyUsageActual = x509TestCert.ExtensionsFriendly()["Key Usage"];

            Assert.AreEqual(expected: keyUsageExpected, actual: keyUsageActual);
        }
    }
}
