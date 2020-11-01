using System;

namespace RMS.Core.Common
{
    public static class ConversionHelper
    {
        public static double ToDouble(string value)
        {
            double result;
            double.TryParse(value, out result);
            return result;
        }
        public static Guid ToGuid(string value)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(value, out result);

            return result;
        }
        public static int ToInteger(string value)
        {
            int result;
            int.TryParse(value, out result);
            return result;
        }
        public static string ToBinary(int value, int length)
        {
            string binary = Convert.ToString(value, 2);
            int len = binary.Length;
            if (len > length)
            {
                if (len <= 8)
                    length = 8;
                else if (len > 8 && len <= 16)
                    length = 16;
                else if (len > 16 && len <= 24)
                    length = 24;
                else
                    length = 32;
            }

            return new string('0', length - binary.Length) + binary;
        }
        public static float ToFloat(string value)
        {
            float result = 0;
            float.TryParse(value, out result);

            return result;
        }
        public static bool ToBoolean(string value)
        {
            bool result = false;
            bool.TryParse(value, out result);

            return result;
        }
        public static bool IsNumeric(string value)
        {
            double result = 0;
            return double.TryParse(value, out result);
        }
        public static byte[] ToByteArray(string[] strArray)
        {
            byte[] bytes = new byte[strArray.Length];

            for (int ii = 0; ii < strArray.Length; ii++)
            {
                bytes[ii] = Convert.ToByte(strArray[ii]);
            }

            return bytes;

        }
        public static float ToFloat(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                throw new ArgumentException("Bytes Length should be 4");
            }

            float floatValue = BitConverter.ToSingle(bytes, 0);
            return floatValue;
        }
        public static float ToMicrochipFloat(byte[] bytes)
        {
            /* Float 1 bit for sign 8 bits for exponent and 23 bits for mantissa
             * Microchip Float
             *     a         b       c        d
             * eeeeeeee Sxxxxxxx xxxxxxxx xxxxxxxx where x = 0 or 1 and S is sign bit
             * 
             * Standard Float
             *     a         b       c        d
             * Seeeeeee exxxxxxx xxxxxxxx xxxxxxxx where x = 0 or 1 and S is sign bit
             * 
             * conversion performed to make microchip float to standard float
             * f = a & 0x80
             * e = a & 0x01
             * a >>= 1
             * a |= f
             * e <<= 7
             * b |= e
             */

            if (bytes.Length != 4)
            {
                throw new ArgumentException("Bytes Length should be 4");
            }
            byte f = Convert.ToByte(bytes[0] & Convert.ToByte(0x80));
            byte e = Convert.ToByte(bytes[0] & Convert.ToByte(0x01));

            bytes[0] >>= 1;
            bytes[0] |= f;

            e <<= 7;
            bytes[1] |= e;

            float floatValue = BitConverter.ToSingle(bytes, 0);
            return floatValue;
        }
        public static double ToSignedValue(double unsignedValue, int count)
        {
            int power = (count * 8) - 1;
            double maxValue = Math.Pow(2, power) - 1;
            unsignedValue = unsignedValue > maxValue ? unsignedValue - maxValue * 2 : unsignedValue;
            return unsignedValue;
        }
        public static int[] ToIntArray(char[] bitwiseArray)
        {
            int[] intArray = new int[bitwiseArray.Length];
            for (int ii = 0; ii < bitwiseArray.Length; ii++)
            {
                int.TryParse(bitwiseArray[ii].ToString(), out int bit);
                intArray[ii] = bit;
            }

            return intArray;
        }
    }
}
