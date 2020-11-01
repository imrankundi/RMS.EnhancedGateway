using RMS.Core.Enumerations;

namespace RMS.Core.Common
{
    public class DataTypeHelper
    {
        public static object Convert(DataType dataType, string value)
        {
            switch (dataType)
            {
                case DataType.DateTime:
                    return DateTimeHelper.Parse(value, DateTimeFormat.UK.DateTime);
                case DataType.Integer:
                    return ConversionHelper.ToInteger(value);
                case DataType.Double:
                    return ConversionHelper.ToDouble(value);
                case DataType.String:
                    return value;
                default:
                    break;
            }
            return null;
        }


    }
}
