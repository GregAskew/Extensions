namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    public static class CertificateExtensions {

        /// <summary>
        /// Creates a friendly collection of extension names/values for an X509V2 certificate
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns>
        /// Dictionary of extension names/values. Key: extension.Oid.FriendlyName Value: AsnEncodedData.Format output, with newlines
        /// </returns>
        [SecurityCritical]
        [DebuggerStepThroughAttribute]
        public static Dictionary<string, string> ExtensionsFriendly(this X509Certificate2 certificate) {
            var extensionsFriendly = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if ((certificate != null) && (certificate.Extensions != null)) {
                foreach (var extension in certificate.Extensions) {
                    if ((extension.Oid == null) || string.IsNullOrWhiteSpace(extension.Oid.FriendlyName) || string.IsNullOrWhiteSpace(extension.Oid.Value) || (extension.RawData == null)) continue;
                    // Create an AsnEncodedData object using the extensions information.
                    AsnEncodedData asndata = new AsnEncodedData(extension.Oid, extension.RawData);
                    if (asndata == null) continue;
                    var key = extension.Oid.FriendlyName;
                    var value = asndata.Format(multiLine: true);

                    if (!extensionsFriendly.ContainsKey(key) && !string.IsNullOrWhiteSpace(value)) {
                        extensionsFriendly.Add(key, value);
                    }
                }
            }

            return extensionsFriendly;
        }
    }
}
