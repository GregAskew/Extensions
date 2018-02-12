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
    public class SecurityExtensionsUnitTests {

        [TestMethod]
        public void Test_GenerateHash() {
            var stringToHash = "ABC123";

            // if not specified, will create a SHA512Managed hash
            var hash = stringToHash.GenerateHash();
            var hashString = string.Empty;

            Assert.IsNotNull(hash);
            Assert.IsTrue(hash.Length > 0);
            hashString = hash.ToHashString();
            Assert.IsNotNull(hashString);
            Assert.AreEqual(expected: 128, actual: hashString.Length);

            hash = stringToHash.GenerateHash<SHA512Managed>();
            Assert.IsNotNull(hash);
            Assert.IsTrue(hash.Length > 0);
            hashString = hash.ToHashString();
            Assert.IsNotNull(hashString);
            Assert.AreEqual(expected: 128, actual: hashString.Length);

            hash = stringToHash.GenerateHash<SHA256Managed>();
            Assert.IsNotNull(hash);
            Assert.IsTrue(hash.Length > 0);
            hashString = hash.ToHashString();
            Assert.IsNotNull(hashString);
            Assert.AreEqual(expected: 64, actual: hashString.Length);

            hash = stringToHash.GenerateHash<SHA1Managed>();
            Assert.IsNotNull(hash);
            Assert.IsTrue(hash.Length > 0);
            hashString = hash.ToHashString();
            Assert.IsNotNull(hashString);
            Assert.AreEqual(expected: 40, actual: hashString.Length);
        }

        [TestMethod]
        public void Test_GetPublicKey() {
            var cspParms = new CspParameters();
            //cspParms.Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseDefaultKeyContainer | CspProviderFlags.UseMachineKeyStore;
            cspParms.Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseArchivableKey;
            cspParms.KeyContainerName = "FAKEKeyContainerName";

            var rsaCryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize: 4096, parameters: cspParms);
            var publicKey = rsaCryptoServiceProvider.GetPublicKey();

            Assert.IsNotNull(publicKey);
            var rootElement = XElement.Parse(publicKey);
            Assert.IsNotNull(rootElement);

            var modulusElement = rootElement.Elements()
                .Where(x => x.Name.LocalName == "Modulus")
                .FirstOrDefault();
            Assert.IsNotNull(modulusElement);
            Assert.IsNotNull(modulusElement.Value);
            var modulusValue = modulusElement.Value.ToString();
            Assert.IsNotNull(modulusValue);

            var exponentElement = rootElement.Elements()
                .Where(x => x.Name.LocalName == "Exponent")
                .FirstOrDefault();
            Assert.IsNotNull(exponentElement);
            Assert.IsNotNull(exponentElement.Value);
            var exponentValue = exponentElement.Value.ToString();
            Assert.IsNotNull(exponentValue);

        }

        [TestMethod]
        public void Test_ReadSecureString() {
            var testString = "ABC123";
            var secureString = testString.ToSecureString();

            Assert.IsNotNull(secureString);
            Assert.IsTrue(secureString.Length > 0);

            var decryptedString = secureString.ReadString();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(decryptedString));
            Assert.AreEqual(decryptedString, testString);
        }

        [TestMethod]
        public void Test_ToSecureString() {
            var testString = "ABC123";
            var secureString = testString.ToSecureString();

            Assert.IsNotNull(secureString);
            Assert.IsTrue(secureString.Length > 0);
        }

        #region Random Tests
        [TestMethod]
        public void Test_NextLong() {
            long minimumValue = long.MinValue;
            long maximumValue = long.MaxValue;
            var random = new Random(Guid.NewGuid().GetHashCode());
            long randomLong = random.NextLong();

            Assert.IsTrue(randomLong >= minimumValue);
            Assert.IsTrue(randomLong <= maximumValue);
        }

        [TestMethod]
        public void Test_NextLong_With_Minimum_And_Maximum_Values() {
            long minimumValue = long.MaxValue - 1000;
            long maximumValue = long.MaxValue;
            var random = new Random(Guid.NewGuid().GetHashCode());
            long randomLong = random.NextLong(minimumValue, maximumValue);

            Assert.IsTrue(randomLong >= minimumValue);
            Assert.IsTrue(randomLong < maximumValue);
        }

        [TestMethod]
        public void Test_NextULong() {
            ulong minimumValue = ulong.MinValue;
            ulong maximumValue = ulong.MaxValue;
            var random = new Random(Guid.NewGuid().GetHashCode());
            ulong randomULong = random.NextULong();

            Assert.IsTrue(randomULong >= minimumValue);
            Assert.IsTrue(randomULong <= maximumValue);
        }

        [TestMethod]
        public void Test_NextULong_With_Minimum_And_Maximum_Values() {
            ulong minimumValue = ulong.MaxValue - 1000;
            ulong maximumValue = ulong.MaxValue;
            var random = new Random(Guid.NewGuid().GetHashCode());
            ulong randomULong = random.NextULong(minimumValue, maximumValue);

            Assert.IsTrue(randomULong >= minimumValue);
            Assert.IsTrue(randomULong < maximumValue);
        }
        #endregion
    }
}
