namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    public static class GuidExtensions {

        /// <summary>
        /// Returns a Guid formatted as an octet string
        /// </summary>
        /// <param name="guid">The Guid</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string ToOctectString(this Guid guid) {
            byte[] byteGuid = guid.ToByteArray();
            var info = new StringBuilder();
            foreach (byte b in byteGuid) {
                info.AppendFormat(@"\{0}", b.ToString("x2"));
            }

            return info.ToString();
        }
    }
}
