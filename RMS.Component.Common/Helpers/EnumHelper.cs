using System;
using System.ComponentModel;
using System.Linq;

namespace RMS.Component.Common
{
    /// <summary>
    /// public enum EnumGrades
    /// {
    ///     [Description("Passed")]
    ///     Pass,
    ///     [Description("Failed")]
    ///     Failed,
    ///     [Description("Promoted")]
    ///     Promoted
    /// }

    /// string description = EnumHelper<EnumGrades>.GetEnumDescription("pass");
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EnumHelper<T>
    {
        public static string GetEnumDescription(string value)
        {
            Type type = typeof(T);
            var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            if (name == null)
                return string.Empty;

            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0])
                .Description : name;
        }
    }
}
