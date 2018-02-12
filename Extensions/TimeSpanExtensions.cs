namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    public static class TimeSpanExtensions {

        /// <summary>
        /// Returns a TimeSpan formatted in DD.HH:mm:ss
        /// </summary>
        /// <param name="timeSpan">The TimeSpan</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string DHMSFriendly(this TimeSpan timeSpan) {
            return string.Format("{0:00}.{1:00}:{2:00}:{3:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        /// <summary>
        /// Returns a TimeSpan formatted in HH:mm:ss
        /// </summary>
        /// <param name="timeSpan">The TimeSpan</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string HMSFriendly(this TimeSpan timeSpan) {
            return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        /// <summary>
        /// Returns a TimeSpan formatted in HH:mm
        /// </summary>
        /// <param name="timeSpan">The TimeSpan</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThroughAttribute]
        public static string HMFriendly(this TimeSpan timeSpan) {
            return string.Format("{0:00}:{1:00}", timeSpan.Hours, timeSpan.Minutes);
        }
    }
}
