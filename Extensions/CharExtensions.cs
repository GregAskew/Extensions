namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    public static class CharExtensions {

        /// <summary>
        /// Gets the Ascii value of a number
        /// </summary>
        /// <param name="c">The char</param>
        /// <returns>The formatted string of the char Ascii decimal value</returns>
        /// <example>The char 'A' will return "65"</example>
        [DebuggerStepThroughAttribute]
        public static string GetAsciiDecimalValue(this char c) {
            return string.Format("{0:D}", (int)c);
        }

        /// <summary>
        /// Gets the Ascii value of a number
        /// </summary>
        /// <param name="c">The char</param>
        /// <returns>The formatted string of the char Ascii hex value</returns>
        /// <example>The char 'A' will return "41"</example>
        [DebuggerStepThroughAttribute]
        public static string GetAsciiHexValue(this char c) {
            return string.Format("{0:X}", (int)c);
        }

        /// <summary>
        /// Gets the number value of a char
        /// </summary>
        /// <param name="c">The char</param>
        /// <returns>The int</returns>
        /// <example>A char '7' will return an int of 7</example>
        [DebuggerStepThroughAttribute]
        public static int GetNumber(this char c) {
            return c - '0';
        }
    }
}
