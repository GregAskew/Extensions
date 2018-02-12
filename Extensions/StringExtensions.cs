namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    #endregion

    public static class StringExtensions {

        #region Members
        private static readonly string EidRegexFilter = @"^[a-z]{1}\w{0,4}\d{3}$";
        private static readonly string EmailAddressInvalidCharacterPattern = "^(?!\\.)(\"([^\"\\r\\\\]|\\\\[\"\\r\\\\])*\"|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\\.)\\.)*)(?<!\\.)@[a-z0-9][\\w\\.-]*[a-z0-9]\\.[a-z][a-z\\.]*[a-z]$";
        #endregion

        /// <summary>
        /// Count the number of occurrences of string in the specified string.
        /// </summary>
        /// <param name="text">The string to search for the specified pattern/substring</param>
        /// <param name="pattern">The pattern to search for in the string</param>
        [DebuggerStepThroughAttribute]
        public static int CountStringOccurrences(this string text, string pattern) {
            return CountStringOccurrences(text, pattern, StringComparison.OrdinalIgnoreCase);
        }
        [DebuggerStepThroughAttribute]
        public static int CountStringOccurrences(this string text, string pattern, StringComparison stringComparison) {
            int count = 0;
            int index = 0;
            while ((index = text.IndexOf(pattern, index, stringComparison)) != -1) {
                index += pattern.Length;
                count++;
            }
            return count;
        }

        /// <summary>
        /// Returns the integer indexes of the locations where a specified substring exists in the supplied string.
        /// </summary>
        /// <param name="text">The string to search</param>
        /// <param name="filter">The substring to search for</param>
        /// <returns>A list of integer index locations of the specified substring</returns>
        [DebuggerStepThroughAttribute]
        public static List<int> GetTextLocations(this string text, string filter) {
            var locations = new List<int>();
            int location = 0;
            do {
                location = text.IndexOf(filter, location, StringComparison.OrdinalIgnoreCase);
                if (location > -1) {
                    locations.Add(location);
                    location++;
                    for (int index = 1; index < filter.Length; index++, location++) {
                        locations.Add(location);
                    }
                }
            } while (location > -1);

            return locations;
        }

        /// <summary>
        /// Encodes a string's bytes to it's hexadecimal characters
        /// </summary>
        /// <param name="text">The string to encode</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>The encoded string</returns>
        [DebuggerStepThroughAttribute]
        public static string HexEncode(this string text) {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("text");
            return HexEncode(text, Encoding.Unicode);
        }
        [DebuggerStepThroughAttribute]
        public static string HexEncode(this string text, Encoding encoding) {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("text");
            byte[] stringBytes = encoding.GetBytes(text);
            var encodedString = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes) {
                encodedString.AppendFormat("{0:X2}", b);
            }
            return encodedString.ToString();
        }

        /// <summary>
        /// Determines if a string is a Guid
        /// Guid should contain 32 digits with 4 dashes xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>True if string is a Guid</returns>
        [DebuggerStepThroughAttribute]
        public static bool IsGuid(this string text) {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var tempGuid = Guid.Empty;
            return Guid.TryParse(text, out tempGuid);
        }

        /// <summary>
        /// Determines if the supplied string is a valid EID
        /// </summary>
        /// <param name="text">The EID</param>
        /// <returns>True if valid</returns>
        [DebuggerStepThroughAttribute]
        public static bool IsValidEID(this string text) {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, EidRegexFilter, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Determines if the supplied string is a valid email address
        /// </summary>
        /// <param name="text">The email address</param>
        /// <returns>True if valid</returns>
        [DebuggerStepThroughAttribute]
        public static bool IsValidEmail(this string text) {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, EmailAddressInvalidCharacterPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Determines if the supplied string is a valid IPV4 address
        /// </summary>
        /// <param name="text">The string to test</param>
        /// <returns>True if a valid IPV4 address</returns>
        [DebuggerStepThroughAttribute]
        public static bool IsValidIPV4Address(this string text) {
            if (string.IsNullOrWhiteSpace(text)) return false;
            bool isValidIPV4Address = false;

            //return Regex.IsMatch(text, IPV4RegexFilter);

            IPAddress ip;
            if (IPAddress.TryParse(text, out ip)) {
                isValidIPV4Address = ip.AddressFamily == AddressFamily.InterNetwork;
            }

            return isValidIPV4Address;
        }

        /// <summary>
        /// Determines if the supplied string is a valid IPV6 address
        /// </summary>
        /// <param name="text">The string to test</param>
        /// <returns>True if a valid IPV6 address</returns>
        [DebuggerStepThroughAttribute]
        public static bool IsValidIPV6Address(this string text) {
            if (string.IsNullOrWhiteSpace(text)) return false;
            bool isValidIPV6Address = false;

            //return Regex.IsMatch(text, IPV6RegexFilter);

            IPAddress ip;
            if (IPAddress.TryParse(text, out ip)) {
                isValidIPV6Address = ip.AddressFamily == AddressFamily.InterNetworkV6;
            }

            return isValidIPV6Address;
        }

        /// <summary>
        /// Removes control characters from text and replaces with a space
        /// </summary>
        /// <param name="text">The string</param>
        /// <returns>The modified string</returns>
        /// <remarks>
        /// Control characters are formatting and other non-printing characters, such as ACK, BEL, CR, FF, LF, and VT. 
        /// The Unicode standard assigns code points from \U0000 to \U001F, \U007F, and from \U0080 to \U009F to control characters. 
        /// According to the Unicode standard, these values are to be interpreted as control characters unless their use is otherwise 
        /// defined by an application. Valid control characters are members of the UnicodeCategory.Control category. 
        /// </remarks>
        [DebuggerStepThroughAttribute]
        public static string RemoveControlCharacters(this string text, char replacementCharacter = ' ') {
            if (text == null) return string.Empty;
            var textNormalized = new StringBuilder();
            for (int index = 0; index < text.Length; index++) {
                if (char.IsControl(text[index])) {
                    textNormalized.Append(replacementCharacter);
                }
                else {
                    textNormalized.Append(text[index]);
                }
            }

            return textNormalized.ToString();
        }

        /// <summary>
        /// Removes format characters from text and replaces with a space
        /// </summary>
        /// <param name="text">The string</param>
        /// <returns>The modified string</returns>
        /// <remarks>
        /// A member of the UnicodeCategory enumeration is returned by the Char.GetUnicodeCategory and CharUnicodeInfo.GetUnicodeCategory methods. 
        /// The UnicodeCategory enumeration is also used to support Char methods, such as IsUpper(Char). Such methods determine whether a specified 
        /// character is a member of a particular Unicode general category. A Unicode general category defines the broad classification of a character, 
        /// that is, designation as a type of letter, decimal digit, separator, mathematical symbol, punctuation, and so on.
        /// This enumeration is based on The Unicode Standard, version 5.0. For more information, see the "UCD File Format" and "General Category Values"
        /// subtopics at the Unicode Character Database.
        /// The Unicode Standard defines the following:
        /// 
        /// A surrogate pair is a coded character representation for a single abstract character that consists of a sequence of two code units, 
        /// where the first unit of the pair is a high surrogate and the second is a low surrogate. A high surrogate is a Unicode code point in 
        /// the range U+D800 through U+DBFF and a low surrogate is a Unicode code point in the range U+DC00 through U+DFFF.
        /// 
        /// A combining character sequence is a combination of a base character and one or more combining characters. A surrogate pair represents a 
        /// base character or a combining character. A combining character is either spacing or nonspacing. A spacing combining character takes up a 
        /// spacing position by itself when rendered, while a nonspacing combining character does not. Diacritics are an example of nonspacing combining
        /// characters.
        /// 
        /// A modifier letter is a free-standing spacing character that, like a combining character, indicates modifications of a preceding letter.
        /// 
        /// An enclosing mark is a nonspacing combining character that surrounds all previous characters up to and including a base character.
        /// 
        /// A format character is a character that is not normally rendered but that affects the layout of text or the operation of text processes.
        /// 
        /// The Unicode Standard defines several variations to some punctuation marks. For example, a hyphen can be one of several code values that 
        /// represent a hyphen, such as U+002D (hyphen-minus) or U+00AD (soft hyphen) or U+2010 (hyphen) or U+2011 (nonbreaking hyphen). 
        /// The same is true for dashes, space characters, and quotation marks.
        /// 
        /// The Unicode Standard also assigns codes to representations of decimal digits that are specific to a given script or language, for example,
        /// U+0030 (digit zero) and U+0660 (Arabic-Indic digit zero).
        /// 
        /// http://www.fileformat.info/info/unicode/category/Cf/list.htm
        /// </remarks>
        [DebuggerStepThroughAttribute]
        public static string RemoveFormatCharacters(this string text, char replacementCharacter = ' ') {
            if (text == null) return string.Empty;
            var textNormalized = new StringBuilder();
            for (int index = 0; index < text.Length; index++) {
                if (char.GetUnicodeCategory(text[index]) == UnicodeCategory.Format) {
                    textNormalized.Append(replacementCharacter);
                }
                else {
                    textNormalized.Append(text[index]);
                }
            }

            return textNormalized.ToString();
        }

        /// <summary>
        /// Removes characters that would be invalud in a file name.
        /// </summary>
        /// <param name="text">The file name</param>
        /// <returns>The adjusted file name, with replacement characters</returns>
        [DebuggerStepThroughAttribute]
        public static string RemoveInvalidFileNameCharacters(this string text, string replacementCharacter = "-") {
            if (text == null) return string.Empty;
            return text.Replace(@"\", replacementCharacter).Replace("/", replacementCharacter).Replace(":", replacementCharacter)
                .Replace("*", replacementCharacter).Replace("?", replacementCharacter)
                .Replace("<", replacementCharacter).Replace(">", replacementCharacter).Replace("|", replacementCharacter);
        }

        /// <summary>
        /// Removes the non alphanumeric characters from a string.
        /// </summary>
        /// <param name="text">The string.</param>
        /// <returns>The modified string</returns>
        [DebuggerStepThroughAttribute]
        public static string RemoveNonAlphaNumericChars(this string text) {
            if (text == null) return string.Empty;
            var textNormalized = new StringBuilder();
            for (int index = 0; index < text.Length; index++) {
                if (char.IsLetterOrDigit(text[index])) {
                    textNormalized.Append(text[index]);
                }
            }

            return textNormalized.ToString();
        }

        /// <summary>
        /// Performs a case-insensitive string replacment
        /// </summary>
        /// <param name="str">The string to replace</param>
        /// <param name="oldValue">The old value to replace</param>
        /// <param name="newValue">The new value</param>
        /// <param name="comparison">The comparison operator</param>
        /// <returns>The updated string</returns>
        [DebuggerStepThroughAttribute]
        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison) {
            var updatedString = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1) {
                updatedString.Append(str.Substring(previousIndex, index - previousIndex));
                updatedString.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }

            updatedString.Append(str.Substring(previousIndex));

            return updatedString.ToString();
        }

        /// <summary>
        /// Reverses the provided string
        /// </summary>
        /// <param name="input">The string</param>
        /// <returns>The reversed string</returns>
        [DebuggerStepThroughAttribute]
        public static string ReverseString(this string input) {
            if (input == null) throw new ArgumentNullException("input");

            // allocate a buffer to hold the output
            char[] output = new char[input.Length];
            for (int outputIndex = 0, inputIndex = input.Length - 1; (outputIndex < input.Length); outputIndex++, inputIndex--) {
                // check for surrogate pair
                if ((input[inputIndex] >= 0xDC00) && (input[inputIndex] <= 0xDFFF)
                    && (inputIndex > 0) && (input[inputIndex - 1] >= 0xD800)
                    && (input[inputIndex - 1] <= 0xDBFF)) {
                    // preserve the order of the surrogate pair code units
                    output[outputIndex + 1] = input[inputIndex];
                    output[outputIndex] = input[inputIndex - 1];
                    outputIndex++;
                    inputIndex--;
                }
                else {
                    output[outputIndex] = input[inputIndex];
                }
            }

            return new string(output);
        }

        /// <summary>
        /// Return a non-null string from a value, optionally trimming
        /// </summary>
        /// <param name="sourceValue">The value</param>
        /// <param name="trimSpaces">if set to <c>true</c> trim spaces from string</param>
        /// <returns>The string</returns>
        [DebuggerStepThroughAttribute]
        public static string SafeString(this string sourceValue, bool trimSpaces) {
            sourceValue = sourceValue ?? string.Empty;
            return trimSpaces ? sourceValue.Trim() : sourceValue;
        }

        /// <summary>
        /// If a name has dots/is a fully-qualified name, return the first name part
        /// </summary>
        /// <param name="name">The string</param>
        /// <returns>The single label name</returns>
        [DebuggerStepThroughAttribute]
        public static string SingleLabelName(this string name) {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;
            int firstDotIndex = name.IndexOf(".");
            if (firstDotIndex > -1) {
                return name.Substring(0, firstDotIndex);
            }
            else {
                return name;
            }
        }

        /// <summary>
        /// Converts a string to Title Case
        /// </summary>
        /// <param name="text">The string</param>
        /// <returns>The string formatted in Title Case</returns>
        [DebuggerStepThroughAttribute]
        public static string ToTitleCase(this string text) {
            string titleCase = string.Empty;

            if (!string.IsNullOrWhiteSpace(text)) {
                // make the first letter of each word uppercase
                titleCase = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text.ToLower());
                // match any letter after an apostrophe and make uppercase
                titleCase = Regex.Replace(titleCase, "[^A-Za-z0-9 ](?:.)", m => m.Value.ToUpper());
                // look for 'S at the end of a word and make lower
                titleCase = Regex.Replace(titleCase, @"('S)\b", m => m.Value.ToLower());

                if (titleCase.IndexOf(" Ii,", StringComparison.Ordinal) > -1) {
                    titleCase = titleCase.Replace(" Ii,", " II,");
                }
                if (titleCase.IndexOf(" Iii,", StringComparison.Ordinal) > -1) {
                    titleCase = titleCase.Replace(" Iii,", " III,");
                }
                if (titleCase.IndexOf(" Iv,", StringComparison.Ordinal) > -1) {
                    titleCase = titleCase.Replace(" Iv,", " IV,");
                }
                if (titleCase.EndsWith(" Ii", StringComparison.Ordinal)) {
                    titleCase = titleCase.Substring(0, titleCase.Length - " Ii".Length) + " II";
                }
                if (titleCase.EndsWith(" Iii", StringComparison.Ordinal)) {
                    titleCase = titleCase.Substring(0, titleCase.Length - " Iii".Length) + " III";
                }
                if (titleCase.EndsWith(" Iv", StringComparison.Ordinal)) {
                    titleCase = titleCase.Substring(0, titleCase.Length - " Iv".Length) + " IV";
                }
            }

            return titleCase;
        }

        /// <summary>
        /// Encodes the url string
        /// </summary>
        /// <param name="text">The string to encode</param>
        /// <returns>The encoded string</returns>
        [DebuggerStepThroughAttribute]
        public static string UrlEncode(this string text) {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("text");
            return WebUtility.UrlEncode(text);
        }

        /// <summary>
        /// Decodes the url string
        /// </summary>
        /// <param name="text">The string to decode</param>
        /// <returns>The decoded string</returns>
        [DebuggerStepThroughAttribute]
        public static string UrlDecode(this string text) {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("text");
            return WebUtility.UrlDecode(text);
        }

        #region Base64 Conversion
        /// <summary>
        /// Converts a Base64 string into a Byte Array
        /// </summary>
        /// <param name="base64String">Base64 Encoded String</param>
        /// <returns>Byte Array</returns>
        [DebuggerStepThroughAttribute]
        public static byte[] FromBase64String(this string base64String) {
            if (base64String == null) throw new ArgumentNullException("base64String");
            return Convert.FromBase64String(base64String);
        }

        [DebuggerStepThroughAttribute]
        public static string FromBase64ToDecodedString(this string base64String) {
            if (base64String == null) throw new ArgumentNullException("base64String");
            return new string(Encoding.Unicode.GetChars(base64String.FromBase64String()));
        }

        /// <summary>
        /// Converts a Byte Array into a Base64 Encoded String
        /// </summary>
        /// <param name="byteArray">Byte Array</param>
        /// <returns>Base64 Encoded String</returns>
        [DebuggerStepThroughAttribute]
        public static string ToBase64String(this byte[] byteArray) {
            if (byteArray == null) throw new ArgumentNullException("byteArray");
            return Convert.ToBase64String(byteArray);
        }

        /// <summary>
        /// Returns the formatted string of a SHA1/SHA256/SHA512 hash byte array
        /// </summary>
        /// <param name="byteArray">The byte array</param>
        /// <returns>The hash string</returns>
        [DebuggerStepThroughAttribute]
        public static string ToHashString(this byte[] byteArray) {
            if (byteArray == null) throw new ArgumentNullException("byteArray");
            var info = new StringBuilder();

            for (int index = 0; index < byteArray.Length; index++) {
                info.AppendFormat("{0:X2}", byteArray[index]);
            }

            return info.ToString();
        }

        /// <summary>
        /// Print the byte array in a readable format.
        /// </summary>
        /// <param name="array">The byte array</param>
        [DebuggerStepThroughAttribute]
        public static void PrintByteArray(byte[] array) {
            int index = 0;
            for (index = 0; index < array.Length; index++) {
                Console.Write(string.Format("{0:X2}", array[index]));
                if ((index % 4) == 3) Console.Write(" ");
            }
            Console.WriteLine();
        }
        #endregion

        #region Byte Array Conversion
        /// <summary>
        /// Converts the string into a Byte Array using Unicode
        /// </summary>
        /// <param name="text">string to Convert</param>
        /// <returns>Byte Array</returns>
        [DebuggerStepThroughAttribute]
        public static byte[] ToByteArray(this string text) {
            if (text == null) throw new ArgumentNullException("text");
            return ToByteArray(text, UnicodeEncoding.Unicode);
        }

        /// <summary>
        /// Converts the string into a Byte Array
        /// </summary>
        /// <param name="text">string to Convert</param>
        /// <returns>Byte Array</returns>
        [DebuggerStepThroughAttribute]
        public static byte[] ToByteArray(this string text, Encoding encoding) {
            if (text == null) throw new ArgumentNullException("text");
            return encoding.GetBytes(text);
        }

        /// <summary>
        /// Creates a string from a compressed byte array
        /// </summary>
        /// <param name="compressedData">The compressed byte array</param>
        /// <returns>The string</returns>
        [DebuggerStepThroughAttribute]
        public static string FromCompressedByteArray(this byte[] compressedData) {
            if (compressedData == null) throw new ArgumentNullException("compressedData");
            string decompressedText = string.Empty;

            using (var memoryStreamInput = new MemoryStream(compressedData))
            using (var memoryStreamOutput = new MemoryStream()) {
                using (var deflateStream = new DeflateStream(memoryStreamInput, CompressionMode.Decompress, leaveOpen: true)) {
                    deflateStream.CopyTo(memoryStreamOutput);
                }

                decompressedText = Encoding.Unicode.GetString(memoryStreamOutput.ToArray());
            }

            return decompressedText;
        }

        /// <summary>
        /// Creates a compressed byte array from a string
        /// </summary>
        /// <param name="stringToCompress">The string to compress</param>
        /// <returns>The byte array of compressed string</returns>
        [DebuggerStepThroughAttribute]
        public static byte[] ToCompressedByteArray(this string stringToCompress) {
            if (stringToCompress == null) throw new ArgumentNullException("stringToCompress");
            byte[] compressedData = null;
            byte[] uncompressedData = Encoding.Unicode.GetBytes(stringToCompress);

            using (var compressedMemoryStream = new MemoryStream()) {
                using (var deflateStream = new DeflateStream(compressedMemoryStream, CompressionMode.Compress, leaveOpen: true)) {
                    deflateStream.Write(uncompressedData, 0, uncompressedData.Length);
                }

                compressedMemoryStream.Position = 0;
                compressedData = compressedMemoryStream.ToArray();
            }

            return compressedData;
        }
        #endregion

    }
}
