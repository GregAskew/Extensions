namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    public static class SecurityExtensions {

        /// <summary>
        /// Generate a SHA Hash of a string
        /// </summary>
        /// <param name="stringToHash">String to Hash</param>
        /// <returns>Hash Byte Array</returns>
        [DebuggerStepThroughAttribute]
        public static byte[] GenerateHash(this string stringToHash) {
            if (stringToHash == null) throw new ArgumentNullException("stringToHash");
            return GenerateHash<SHA512Managed>(stringToHash.ToByteArray());
        }

        /// <summary>
        /// Generate a SHA Hash of a string
        /// </summary>
        /// <param name="stringToHash">String to Hash</param>
        /// <returns>Hash Byte Array</returns>
        [DebuggerStepThroughAttribute]
        public static byte[] GenerateHash<T>(this string stringToHash) where T : HashAlgorithm, new() {
            if (stringToHash == null) throw new ArgumentNullException("stringToHash");
            return GenerateHash<T>(stringToHash.ToByteArray());
        }

        /// <summary>
        /// Generate a SHA512 Hash of a Byte Array
        /// </summary>
        /// <param name="byteArray">Byte Array</param>
        /// <returns>Hash Byte Array</returns>
        [DebuggerStepThroughAttribute]
        public static byte[] GenerateHash<T>(this byte[] byteArray) where T : HashAlgorithm, new() {
            if (byteArray == null) throw new ArgumentNullException("byteArray");
            return new T().ComputeHash(byteArray);
        }

        /// <summary>
        /// Returns the Public Key as an XML String
        /// </summary>
        /// <param name="algorithm">The cryptographic service provider</param>
        /// <returns>XML String of the public key</returns>
        [DebuggerStepThroughAttribute]
        public static string GetPublicKey(this AsymmetricAlgorithm algorithm) {
            if (algorithm == null) throw new ArgumentNullException("algorithm");
            return algorithm.ToXmlString(includePrivateParameters: false);
        }

        #region Random Extensions
        /// <summary>
        /// returns a uniformly random long between long.Min inclusive and long.Max inclusive
        /// </summary>
        /// <param name="rng">The Random object</param>
        /// <returns>The random long number.</returns>
        [DebuggerStepThroughAttribute]
        public static long NextLong(this Random rng) {
            byte[] buffer = new byte[8];
            rng.NextBytes(buffer);
            return BitConverter.ToInt64(value: buffer, startIndex: 0);
        }

        /// <summary>
        /// returns a uniformly random long between Min and Max without modulo bias
        /// </summary>
        /// <param name="rng">The Random object</param>
        /// <param name="min">The minimum value of the range</param>
        /// <param name="max">The maximum value of the range</param>
        /// <param name="inclusiveUpperBound">True to include the maximum value of the range</param>
        /// <returns>The random long number.</returns>
        [DebuggerStepThroughAttribute]
        public static long NextLong(this Random rng, long min = long.MinValue, long max = long.MaxValue, bool inclusiveUpperBound = false) {
            ulong range = (ulong)(max - min);

            if (inclusiveUpperBound) {
                if (range == ulong.MaxValue) {
                    return rng.NextLong();
                }

                range++;
            }

            if (range <= 0) {
                throw new ArgumentOutOfRangeException("Max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true", "max");
            }

            ulong limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do {
                r = rng.NextULong();
            } while (r > limit);
            return (long)(r % range + (ulong)min);
        }

        /// <summary>
        /// returns a uniformly random ulong between long.Min inclusive and long.Max inclusive
        /// </summary>
        /// <param name="rng">The Random object</param>
        /// <returns>The random ulong number.</returns>
        [DebuggerStepThroughAttribute]
        public static ulong NextULong(this Random rng) {
            byte[] buffer = new byte[8];
            rng.NextBytes(buffer);
            return BitConverter.ToUInt64(value: buffer, startIndex: 0);
        }

        /// <summary>
        /// returns a uniformly random ulong between Min and Max without modulo bias
        /// </summary>
        /// <param name="rng">The Random object</param>
        /// <param name="min">The minimum value of the range</param>
        /// <param name="max">The maximum value of the range</param>
        /// <param name="inclusiveUpperBound">True to include the maximum value of the range</param>
        /// <returns>The random ulong number.</returns>
        [DebuggerStepThroughAttribute]
        public static ulong NextULong(this Random rng, ulong min = ulong.MinValue, ulong max = ulong.MaxValue, bool inclusiveUpperBound = false) {
            ulong range = max - min;

            if (inclusiveUpperBound) {
                if (range == ulong.MaxValue) {
                    return rng.NextULong();
                }

                range++;
            }

            if (range <= 0) {
                throw new ArgumentOutOfRangeException("Max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true", "max");
            }

            ulong limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do {
                r = rng.NextULong();
            } while (r > limit);

            return r % range + min;
        }
        #endregion

        #region SecureString Extensions
        /// <summary>
        /// Returns the Contents of a Secure String
        /// </summary>
        /// <param name="securedString">Secure String</param>
        /// <returns>The decrypted String</returns>
        [DebuggerStepThroughAttribute]
        public static string ReadString(this SecureString securedString) {
            if (securedString == null) return string.Empty;
            IntPtr ptr = Marshal.SecureStringToBSTR(securedString);
            return Marshal.PtrToStringUni(ptr);
        }

        /// <summary>
        /// Creates a Secure String
        /// </summary>
        /// <param name="plainText">Non Secured String</param>
        /// <returns>SecureString</returns>
        [DebuggerStepThroughAttribute]
        public static SecureString ToSecureString(this string plainText) {
            if (plainText == null) throw new ArgumentNullException("plainText");
            var secureString = new SecureString();
            foreach (char characterToSecure in plainText.ToCharArray()) {
                secureString.AppendChar(characterToSecure);
            }
            secureString.MakeReadOnly();
            return secureString;
        }
        #endregion
    }
}
