namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    #endregion

    public static class XElementExtensions {

        /// <summary>
        /// Gets the value of the specified attribute in an Xml element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="ignoreCase"></param>
        /// <returns>The value of the XAttribute</returns>
        [DebuggerStepThroughAttribute]
        public static T GetAttributeValue<T>(this XElement element, XName attributeName, bool ignoreCase = true) {
            if (element == null) throw new ArgumentNullException("element");
            if (attributeName == null) throw new ArgumentNullException("attributeName");
            if (string.IsNullOrWhiteSpace(attributeName.LocalName)) throw new ArgumentNullException("attributeName.LocalName");

            var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            XAttribute extractedAttribute = element.Attributes()
                .Where(x => string.Equals(x.Name.LocalName, attributeName.LocalName, stringComparison))
                .SingleOrDefault();

            if (extractedAttribute == null) return default(T);
            T convertedAttribute = default(T);
            try {
                convertedAttribute = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(extractedAttribute.Value);
            }
            catch { }
            return convertedAttribute;
        }

        /// <summary>
        /// Gets a single value of an XElement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The element with a single value</param>
        /// <param name="elementName">The element name</param>
        /// <param name="ignoreCase">True to ignore case when comparing names</param>
        /// <returns>The XElement value</returns>
        /// <remarks>
        /// Xml is case-sensitive
        /// If multiple elements with the same name exist, and exception will be thrown.
        /// </remarks>
        /// <exception>
        /// Throws InvalidOperationException if multiple elements exist with the same name
        /// </exception>
        [DebuggerStepThroughAttribute]
        public static T GetElementValue<T>(this XElement element, XName elementName, bool ignoreCase = true) {
            if (element == null) throw new ArgumentNullException("element");
            if (elementName == null) throw new ArgumentNullException("elementName");
            if (string.IsNullOrWhiteSpace(elementName.LocalName)) throw new ArgumentNullException("elementName.LocalName");

            var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            XElement extractedElement = element.Elements()
                .Where(x => string.Equals(x.Name.LocalName, elementName.LocalName, stringComparison))
                .SingleOrDefault();

            if (extractedElement == null) return default(T);
            T convertedType = default(T);
            try {
                if (typeof(T).IsEnum) {
                    convertedType = (T)Enum.Parse(typeof(T), extractedElement.Value, ignoreCase: true);
                }
                else {
                    convertedType = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(extractedElement.Value);
                }
            }
            catch { }
            return convertedType;
        }

        /// <summary>
        /// Gets a single value of an XElement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The element with a single value</param>
        /// <param name="elementName">The element name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="ignoreCase">True to ignore case when comparing names</param>
        /// <returns>The XElement value</returns>
        /// <remarks>
        /// Xml is case-sensitive
        /// If multiple elements with the same name exist, and exception will be thrown.
        /// </remarks>
        /// <exception>
        /// Throws InvalidOperationException if multiple elements exist with the same name
        /// </exception>
        [DebuggerStepThroughAttribute]
        public static T GetElementValueForAttributeName<T>(this XElement element, XName elementName, XName attributeName, string attributeValue, bool ignoreCase = true) {
            if (element == null) throw new ArgumentNullException("element");
            if (elementName == null) throw new ArgumentNullException("elementName");
            if (attributeName == null) throw new ArgumentNullException("attributeName");
            if (string.IsNullOrWhiteSpace(elementName.LocalName)) throw new ArgumentNullException("elementName.LocalName");
            if (string.IsNullOrWhiteSpace(attributeName.LocalName)) throw new ArgumentNullException("attributeName.LocalName");
            if (string.IsNullOrWhiteSpace(attributeValue)) throw new ArgumentNullException("attributeValue");

            var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            List<XElement> extractedElements = element.Elements()
                .Where(x => 
                    (x != null) && (x.Name != null) && !string.IsNullOrWhiteSpace(x.Name.LocalName)
                    && string.Equals(x.Name.LocalName, elementName.LocalName, stringComparison))
                .ToList();

            if ((extractedElements == null) || (extractedElements.Count == 0)) return default(T);
            T convertedType = default(T);

            try {
                var extractedElement = extractedElements
                    .Where(x => x.Attributes()
                        .Any(y => 
                            (y != null) && (y.Name != null) && !string.IsNullOrWhiteSpace(y.Name.LocalName)
                            && string.Equals(y.Name.LocalName, attributeName.LocalName, stringComparison)
                            && !string.IsNullOrWhiteSpace(y.Value)
                            && string.Equals(y.Value, attributeValue, stringComparison)))
                    .SingleOrDefault();

                if (typeof(T).IsEnum) {
                    convertedType = (T)Enum.Parse(typeof(T), extractedElement.Value, ignoreCase: true);
                }
                else {
                    convertedType = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(extractedElement.Value);
                }
            }
            catch { }
            return convertedType;
        }

        /// <summary>
        /// Gets values of XElement collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The element with multiple values</param>
        /// <param name="elementName">The element name</param>
        /// <returns>The XElement value</returns>
        /// <remarks>
        /// Xml is case-sensitive
        /// </remarks>
        [DebuggerStepThroughAttribute]
        public static List<T> GetElementValues<T>(this XElement element, XName elementName, bool ignoreCase = true) {
            if (element == null) throw new ArgumentNullException("element");
            if (elementName == null) throw new ArgumentNullException("elementName");
            if (string.IsNullOrWhiteSpace(elementName.LocalName)) throw new ArgumentNullException("elementName.LocalName");

            var elementValues = new List<T>();
            var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            List<XElement> extractedElements = extractedElements = element.Elements()
                .Where(x => string.Equals(x.Name.LocalName, elementName.LocalName, stringComparison))
                .ToList();

            if (extractedElements != null) {
                foreach (var extractedElement in extractedElements) {
                    T convertedType = default(T);
                    try {
                        if (typeof(T).IsEnum) {
                            convertedType = (T)Enum.Parse(typeof(T), extractedElement.Value, ignoreCase: true);
                        }
                        else {
                            convertedType = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(extractedElement.Value);
                        }

                        elementValues.Add(convertedType);
                    }
                    catch { }
                }
            }

            return elementValues;
        }
    }
}
