namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    public static class CollectionExtensions {

        /// <summary>
        /// Perform an Action on a collection
        /// </summary>
        /// <typeparam name="T">The collection Type</typeparam>
        /// <param name="items">The collection</param>
        /// <param name="work">The Action</param>
        [DebuggerStepThroughAttribute]
        public static void Each<T>(this IEnumerable<T> items, Action<T> work) {
            if (items == null) throw new ArgumentNullException("items");
            if (work == null) throw new ArgumentNullException("work");

            foreach (T item in items) {
                work(item);
            }
        }

        /// <summary>
        /// Return N number of random items from a generic list
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">Generic list we wish to retrieve from</param>
        /// <param name="count">number of items to return</param>
        /// <param name="exclusions">items to exclude from returning</param>
        /// <returns></returns>
        [DebuggerStepThroughAttribute]
        internal static IEnumerable<T> Randomize<T>(this List<T> list, int count = 1, List<T> exclusions = null) {
            if (list == null) throw new ArgumentNullException("list");

            var randomList = new List<T>();
            var random = new Random(Guid.NewGuid().GetHashCode());
            if (exclusions == null) {
                exclusions = new List<T>();
            }

            while (list.Where(x => !exclusions.Contains(x)).ToList().Count > 0) {
                //get the next random number between 0 and the list count
                int index = random.Next(0, list.Where(x => !exclusions.Contains(x)).ToList().Count);

                //get that index
                randomList.Add(list[index]);

                //remove that item so it cant be added again
                list.RemoveAt(index);
            }

            //return the specified number of items
            return randomList.Take(count);
        }

        #region Dictionary
        /// <summary>
        /// Adds an item to a TValue List for a dictionary with the specified key type
        /// </summary>
        /// <param name="dictionary">The Dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="listItem">The list item to add</param>
        [DebuggerStepThroughAttribute]
        public static void AddListItem<TKey, TValue>(
            this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue listItem) {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            if (key == null) throw new ArgumentNullException("key");

            if (!dictionary.ContainsKey(key)) {
                dictionary.Add(key, new List<TValue>());
            }
            dictionary[key].Add(listItem);
        }

        /// <summary>
        /// Returns a SortedDictionary based on the provided Dictionary
        /// </summary>
        /// <typeparam name="TKey">The key Type</typeparam>
        /// <typeparam name="TValue">The value Type</typeparam>
        /// <param name="dictionary">The dictionary to sort</param>
        /// <returns>The SortedDictionary</returns>
        [DebuggerStepThroughAttribute]
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            return new SortedDictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Returns a SortedDictionary based on the provided Dictionary
        /// </summary>
        /// <typeparam name="TKey">The key Type</typeparam>
        /// <typeparam name="TValue">The value Type</typeparam>
        /// <param name="dictionary">The dictionary to sort</param>
        /// <param name="comparer">The Comparer to use when creating the SortedDictionary</param>
        /// <returns>The SortedDictionary</returns>
        [DebuggerStepThroughAttribute]
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer) {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            if (comparer == null) throw new ArgumentNullException("comparer");

            return new SortedDictionary<TKey, TValue>(dictionary, comparer);
        } 
        #endregion

        /// <summary>
        /// Returns a string delimited with a semicolon
        /// </summary>
        /// <param name="list">The list</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string ToDelimitedString<T>(this IEnumerable<T> list, string delimiter = ";") {
            if (list == null) return string.Empty;
            return string.Join(delimiter, list);
        }

        /// <summary>
        /// Gets a newline-formatted string for a collection
        /// </summary>
        /// <typeparam name="T">The collection type</typeparam>
        /// <param name="list">The collection</param>
        /// <returns>The newline-formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string ToFormattedString<T>(this IEnumerable<T> list) {
            if (list == null) return string.Empty;
            return string.Join(Environment.NewLine, list);
        }

        /// <summary>
        /// Returns a string delimited with ","
        /// </summary>
        /// <param name="list">The list</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string ToQuotedCSVString<T>(this IEnumerable<T> list) {
            if (list == null) return string.Empty;
            var csv = string.Join("\",\"", list);

            if (!string.IsNullOrWhiteSpace(csv)) {
                csv = $"\"{csv}\"";
            }
            return csv;
        }
    }
}
