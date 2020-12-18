using RMS.Core.Enumerations;
using System.Collections.Generic;

namespace RMS.Protocols.Configuration
{
    public class ConfigParameter
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public object Value { get; set; }
        public string Format { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double StepSize { get; set; }
        public List<object> Values { get; set; }
        public ConfigParameterControlType ConfigParameterControlType { get; set; }
        public ConfigParameter()
        {
            Values = new List<object>();
        }
    }
}
