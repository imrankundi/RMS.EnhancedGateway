using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace RMS.Component.Common
{
    /// <summary>
    /// This is class contains extension function(s) for enum objects
    /// </summary>
    public static class EnumExtentsion
    {
        /// <summary>
        /// Returns description of enum value if attached with enum member through DiscriptionAttribute
        /// </summary>        
        /// <returns>Description of enum value</returns>
        /// <see cref="DescriptionAttribute"/>
        public static string ToDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] descriptions = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (descriptions != null && descriptions.Length > 0) ? descriptions[0].Description : fieldInfo.Name;
        }

        /// <summary>
        /// Returns true if enum matches any of the given values
        /// </summary>
        /// <param name="value">Value to match</param>
        /// <param name="values">Values to match against</param>
        /// <returns>Return true if matched</returns>
        public static bool In(this Enum value, params Enum[] values)
        {
            return values.Any(v => v == value);
        }
    }
}
