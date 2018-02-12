namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    public static class DateTimeExtensions {

        /// <summary>
        /// Gets the datetime of the first day of the month
        /// </summary>
        /// <param name="dateTime">The datetime for which to provide the first day of the month</param>
        /// <returns>The DateTime</returns>
        [DebuggerStepThroughAttribute]
        public static DateTime GetFirstDayOfMonth(this DateTime datetime) {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }

        /// <summary>
        /// Gets the datetime of the last day of the month
        /// </summary>
        /// <param name="dateTime">The datetime for which to provide the last day of the month</param>
        /// <returns>The DateTime</returns>
       [DebuggerStepThroughAttribute]
        public static DateTime GetLastDayOfMonth(this DateTime dateTime) {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Returns DateTime string formatted in yyyy-MM-dd HH:mm, unless it is equal to SqlDateTime.MinValue, then return "N/A"
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        /// <remarks>For datetime Sql type, not necessary for datetime2</remarks>
        [DebuggerStepThroughAttribute]
        public static string LongDateSqlFriendly(this DateTime dateTime) {
            return dateTime == SqlDateTime.MinValue.Value ? "N/A" : dateTime.YMDHMFriendly();
        }

        /// <summary>
        /// Returns DateTime string formatted in yyyy-MM-dd, unless it is equal to SqlDateTime.MinValue, then return "N/A"
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        /// <remarks>For datetime Sql type, not necessary for datetime2</remarks>
        [DebuggerStepThroughAttribute]
        public static string ShortDateSqlFriendly(this DateTime dateTime) {
            return dateTime == SqlDateTime.MinValue.Value  ? "N/A" : dateTime.YMDFriendly();
        }

        /// <summary>
        /// Returns DateTime string formatted in ddd, dd MMM yyyy hh:mm:ss GMT
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string ToRfc2068DateString(this DateTime datetime) {
            return datetime.ToString("ddd, dd MMM yyyy hh:mm:ss") + " GMT";
        }

        /// <summary>
        /// Returns DateTime string formatted in yyyy-MM-dd HH:mm:ss.FFFFF
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string VerboseString(this DateTime datetime) {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss.FFFFF");
        }

        /// <summary>
        /// Returns DateTime string formatted in yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string YMDFriendly(this DateTime datetime) {
            return datetime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Returns DateTime string formatted in yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string YMDHMFriendly(this DateTime datetime) {
            return datetime.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// Returns DateTime string formatted in yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string YMDHMSFriendly(this DateTime datetime) {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Returns DateTime string formatted in yyyy-MM-dd HH:mm:ss.FFF
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string YMDHMSFFFFriendly(this DateTime datetime) {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF");
        }

        /// <summary>
        /// Returns DateTime string formatted in yyyy-MM-dd HH:mm:ss.FFFFFFF
        /// </summary>
        /// <param name="datetime">The DateTime</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string YMDHMSFFFFFFFFriendly(this DateTime datetime) {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF");
        }
    }
}
