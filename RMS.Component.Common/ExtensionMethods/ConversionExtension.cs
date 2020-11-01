using System;

namespace RMS.Component.Common
{
    public static class ConversionExtension
    {
        public static double ToDouble(this string value)
        {
            double result = 0.0;
            double.TryParse(value, out result);
            return result;
        }
        public static Guid ToGuid(this string value)
        {
            Guid empty = Guid.Empty;
            Guid.TryParse(value, out empty);
            return empty;
        }
        public static int ToInteger(this string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }
        public static string ToBinaryString(this int value)
        {
            int length = 8;
            string text = Convert.ToString(value, 2);
            int stringLength = text.Length;

            if (stringLength < 8)
            {
                length = 8;
            }
            else if (stringLength > 8 && stringLength < 16)
            {
                length = 16;
            }
            else if (stringLength > 16 && stringLength < 24)
            {
                length = 24;
            }
            else
            {
                length = 32;
            }

            return new string('0', length - text.Length) + text;
        }
        public static float ToFloat(this string value)
        {
            float result = 0f;
            float.TryParse(value, out result);
            return result;
        }
        public static bool ToBoolean(this string value)
        {
            bool result = false;
            bool.TryParse(value, out result);
            return result;
        }
        public static bool IsNumeric(this string value)
        {
            double num = 0.0;
            return double.TryParse(value, out num);
        }
    }
}
