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
    public class ExceptionExtensionsUnitTests {

        [TestMethod]
        public void Test_VerboseExceptionString() {

            try {
                ThrowException();
            }
            catch (ApplicationException e) {
                var verboseExceptionString = e.VerboseExceptionString();
                Assert.IsNotNull(verboseExceptionString);
                Assert.AreNotEqual(notExpected: -1, actual: verboseExceptionString.IndexOf("Testing 123"));
                Assert.AreEqual(expected: -1, actual: verboseExceptionString.IndexOf("NULL"));
                Assert.AreNotEqual(notExpected: -1, actual: verboseExceptionString.IndexOf("StackTrace:    at Extensions.Tests.ExceptionExtensionsUnitTests.ThrowException()"));
                Assert.AreNotEqual(notExpected: -1, actual: verboseExceptionString.IndexOf("TargetSite: Void ThrowException()"));

                Assert.AreNotEqual(notExpected: -1, actual: verboseExceptionString.IndexOf("Inner Exception:"));
                Assert.AreNotEqual(notExpected: -1, actual: verboseExceptionString.IndexOf("Testing 456"));

            }
        }

        private void ThrowException() {
            try {
                ThrowException2();
            }
            catch (Exception e) {
                throw new ApplicationException(message: "Testing 456", innerException: e);
            }
        }

        private void ThrowException2() {
            throw new ApplicationException("Testing 123");
        }
    }
}
