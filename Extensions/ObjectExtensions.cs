namespace Extensions {

    #region Usings
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    #endregion

    public static class ObjectExtensions {

        /// <summary>
        /// Makes a copy from the object.
        /// Doesn't copy the reference memory, only data.
        /// </summary>
        /// <typeparam name="T">Type of the return object.</typeparam>
        /// <param name="objectToClone">Object to be copied.</param>
        /// <returns>Returns the copied object.</returns>
        /// <remarks>
        /// For data transfer objects, use XmlSerializer or DataContractSerializer - avoid BinaryFormatter
        /// </remarks>
        [DebuggerStepThroughAttribute]
        public static T Clone<T>(this object objectToClone) {
            if (objectToClone != null) {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream()) {

                    binaryFormatter.Serialize(memoryStream, objectToClone);
                    memoryStream.Position = 0;

                    T result = (T)binaryFormatter.Deserialize(memoryStream);
                    return result;
                }
            }
            else
                return default(T);
        }

        /// <summary>
        /// Gets the name of the current method on the stack
        /// </summary>
        /// <returns>The method name</returns>
        /// <remarks>Not an extension, placed here for convenience</remarks>
        [DebuggerStepThroughAttribute]
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static string CurrentMethodName() {
            var frame = new StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;

            return type + "::" + name + "(): ";
        }

        /// <summary>
        /// Gets the string value for the specified property name
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>The string value of the property</returns>
        [DebuggerStepThroughAttribute]
        public static object GetPropertyValue(this object obj, string propertyName) {
            if (string.IsNullOrWhiteSpace(propertyName)) {
                throw new ArgumentNullException("propertyName");
            }

            object attributeValue = null;

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in properties) {
                // if not readable then cannot check it's value
                if (!propertyInfo.CanRead) { continue; }

                MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: false);

                // Get method must be public
                if (getMethod == null) { continue; }

                if (string.Equals(propertyName, propertyInfo.Name, StringComparison.OrdinalIgnoreCase)) {
                    attributeValue = propertyInfo.GetValue(obj, new object[] { });
                    break;
                }
            }

            return attributeValue;
        }

        /// <summary>
        /// Gets the string value for the specified property name
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>The collection of string values of the property</returns>
        [DebuggerStepThroughAttribute]
        public static List<T> GetPropertyValues<T>(this object obj, string propertyName) {
            if (string.IsNullOrWhiteSpace(propertyName)) {
                throw new ArgumentNullException("propertyName");
            }

            var attributeValue = new List<T>();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in properties) {
                // if not readable then cannot check it's value
                if (!propertyInfo.CanRead) { continue; }

                MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: false);

                // Get method must be public
                if (getMethod == null) { continue; }

                if (string.Equals(propertyName, propertyInfo.Name, StringComparison.OrdinalIgnoreCase)) {
                    attributeValue = (List<T>)propertyInfo.GetValue(obj, null);
                    break;
                }
            }

            return attributeValue;
        }

        /// <summary>
        /// Reads a value from a database field/parameter.  Returns a typed equivalent if DbNull.
        /// </summary>
        /// <typeparam name="T">The target type stored in a data reader field or command parameter.</typeparam>
        /// <param name="thisObject">The object that could potentially contain a DBNull.Value</param>
        /// <param name="dbNullEquivalent">The typed equivalent of DBNull.Value for the data</param>
        /// <returns>
        /// A value of T from the database
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/en-us/library/system.data.datacolumn.datatype.aspx
        /// The DataColumn.DataType property supports the following base .NET Framework data types: 
        /// Boolean, Byte, Char, DateTime, Decimal, Double, Guid, Int16, Int32, Int64, SByte, Single, String, TimeSpan, UInt16, UInt32, UInt64, Byte[]
        /// </remarks>
        [DebuggerStepThroughAttribute]
        public static T HandleDbNull<T>(this object thisObject) {
            return HandleDbNull<T>(thisObject, default(T));
        }
        [DebuggerStepThroughAttribute]
        public static T HandleDbNull<T>(this object thisObject, T dbNullEquivalent) {
            T returnValue;
            if (thisObject is DBNull) {
                returnValue = dbNullEquivalent;
            }
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition().Equals(typeof(Nullable<>))) {
                returnValue = (T)TypeDescriptor.GetConverter(Nullable.GetUnderlyingType(typeof(T))).ConvertFromInvariantString(thisObject.SafeString(trimSpaces: true));
            }
            else {
                returnValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(thisObject.SafeString(trimSpaces: true));
            }
            return returnValue;
        }

        /// <summary>
        /// Return a non-null string from a value, optionally trimming
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="trimSpaces">Specifies if leading/trailing spaces should be removed</param>
        /// <returns>The string</returns>
        [DebuggerStepThroughAttribute]
        public static string SafeString(this object value, bool trimSpaces) {
            return (value == null ? string.Empty : value.ToString()).SafeString(trimSpaces);
        }

        /// <summary>
        /// Creates a shallow copy of a parent object's writable properties to an object of the same type or a derived child object
        /// </summary>
        /// <typeparam name="TParent">The parent object type</typeparam>
        /// <typeparam name="TChild">The child object type</typeparam>
        /// <param name="parent">The parent object</param>
        /// <param name="child">The child object</param>
        [DebuggerStepThroughAttribute]
        public static void ShallowCopy<TParent, TChild>(this TParent parent, TChild child) where TChild : TParent {
            foreach (PropertyInfo parentProperty in parent.GetType().GetProperties()) {
                if (parentProperty.CanWrite) {
                    parentProperty.SetValue(child, parentProperty.GetValue(parent, null), null);
                }
            }
        }

        /// <summary>
        /// Serializes an object to Xml
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="objectToSeralize"></param>
        /// <returns>The Xml string</returns>
        [DebuggerStepThroughAttribute]
        public static string ToXml<T>(this T objectToSeralize) where T : new() {
            if (objectToSeralize == null) throw new ArgumentNullException("objectToSeralize");
            string serializedXml;
            using (var memoryStream = new MemoryStream()) {
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(memoryStream, objectToSeralize);
                memoryStream.Flush();
                memoryStream.Position = 0;
                using (var streamReader = new StreamReader(memoryStream)) {
                    serializedXml = streamReader.ReadToEnd();
                }
            }
            return serializedXml;
        }

        /// <summary>
        /// Trim string values of objects
        /// </summary>
        /// <param name="objectToTrim">The object to trim string values</param>
        [DebuggerStepThroughAttribute]
        public static void TrimStringValues(this object objectToTrim) {
            PropertyInfo[] properties = objectToTrim.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (PropertyInfo propertyInfo in properties) {
                // Only work with strings
                if (propertyInfo.PropertyType != typeof(string)) { continue; }

                // If not writable then cannot null it; if not readable then cannot check it's value
                if (!propertyInfo.CanWrite || !propertyInfo.CanRead) { continue; }

                MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: false);
                MethodInfo setMethod = propertyInfo.GetSetMethod(nonPublic: false);

                // Get and set methods have to be public
                if (getMethod == null) { continue; }
                if (setMethod == null) { continue; }

                string currentValue = propertyInfo.GetValue(objectToTrim, new string[] { }) as string;
                if (currentValue != null) {
                    propertyInfo.SetValue(objectToTrim, currentValue.Trim(), new object[] { });
                }
            }
        }
    }
}
