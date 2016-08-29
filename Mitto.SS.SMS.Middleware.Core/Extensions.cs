using System;

namespace Mitto.SS.SMS.Middleware.Core
{

    /// <summary>
    /// Provides base conversion and formatting operations
    /// </summary>
    public static class Extensions
    {
        private static readonly string DbDateTimeFormat = "yyyy-MM-dd";

        /// <summary>
        /// Safe convert an object to integer
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <param name="defaultValue">Default value if conversion fails</param>
        /// <returns>Returns converted integer</returns>
        public static int ToInt(this object obj, int defaultValue = 0)
        {
            var str = obj.ToString();

            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            int target = defaultValue;

            int.TryParse(str, out target);

            return target;
        }

        /// <summary>
        /// Safe convert an object to DateTime
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <returns>Returns the object converted to dateTime</returns>
        public static DateTime ToDateTime(this object obj)
        {
            var str = obj.ToString();

            if (string.IsNullOrEmpty(str))
            {
                return default(DateTime);
            }

            DateTime target = default(DateTime);

            DateTime.TryParse(str, out target);

            return target;
        }

        /// <summary>
        /// Safe convert an object to decimal
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <param name="defaultValue">Default value if conversion fails</param>
        /// <returns>Returns converted decimal</returns>
        public static decimal ToDecimal(this object obj, decimal defaultValue = 0)
        {
            var str = obj.ToString();

            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            decimal target = defaultValue;

            decimal.TryParse(str, out target);

            return target;
        }

        /// <summary>
        /// Safe convert an object to long
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <param name="defaultValue">Default value if conversion fails</param>
        /// <returns>Returns converted long</returns>
        public static long ToLong(this object obj, long defaultValue = 0)
        {
            var str = obj.ToString();

            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            long target = defaultValue;

            long.TryParse(str, out target);

            return target;
        }

        /// <summary>
        /// Formats given dateTime according to the database accepted format. The format is specified as a readonly value under the Core assembly.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDatabaseDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString(DbDateTimeFormat);
        }

        /// <summary>
        /// Safe convert an object to boolean
        /// </summary>
        /// <param name="obj">Input object, type is <see cref="object"/> for supporting all primitives</param>
        /// <returns>Returns converted boolean value if conversion was successful. Returns "false" boolean if conversion failed.</returns>
        public static bool ToBoolean(this object obj)
        {
            var str = obj.ToString();
            bool retVal; //Which is "false"

            bool.TryParse(str, out retVal);

            return retVal;
        }
    }
}
