﻿using System;
using System.Globalization;

namespace RMS.Component.Common
{
    /// <summary>
    /// This class contain extension functions for string objects
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>        
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (string.Compare(value, otherValue) == 0)
                    return true;

            return false;
        }

        /// <summary>
        /// Converts string to enum object
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }

        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="arg0">An System.Object to format</param>
        /// <returns>A copy of format in which the first format item has been replaced by the
        /// System.String equivalent of arg0</returns>
        public static string Format(this string value, object arg0)
        {
            return string.Format(value, arg0);
        }

        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the System.String
        /// equivalent of the corresponding instances of System.Object in args.</returns>
        public static string Format(this string value, params object[] args)
        {
            return string.Format(value, args);
        }
        /// <summary>
        /// Convert hex String to bytes representation
        /// </summary>
        /// <param name="hexString">Hex string to convert into bytes</param>
        /// <returns>Bytes of hex string</returns>
        public static byte[] HexToBytes(this string hexString)
        {
            if (hexString.Length % 2 != 0)
                throw new ArgumentException(string.Format("HexString cannot be in odd number: {0}", hexString));

            var retVal = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
                retVal[i / 2] = byte.Parse(hexString.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);

            return retVal;
        }

        public static DateTime FromUkDateString(this string datetimeString)
        {
            return DateTime.Parse(datetimeString, CultureInfo.GetCultureInfo("en-GB"));
        }

        public static DateTime FromUSDateString(this string datetimeString)
        {
            return DateTime.Parse(datetimeString, CultureInfo.GetCultureInfo("en-US"));
        }

        public static DateTime ParseFormat(this string datetimeString, string format)
        {
            return DateTime.ParseExact(datetimeString, format, CultureInfo.InvariantCulture);
        }
    }
}
