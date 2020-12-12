using RMS.Core.Enumerations;
using System.Collections.Generic;

namespace RMS.Protocols
{
    public class Parameter
    {
        public string Name { get; set; }
        public bool Signed { get; set; }
        public DataType DataType { get; set; }
        public bool EnableDataType { get; set; }
        public float MinVal { get; set; }
        public float MaxVal { get; set; }
        public ParameterType ParameterType { get; set; }
        public string Field { get; set; }
        public string Expression { get; set; }
        public int Precision { get; set; }
        public bool Evaluate { get; set; }
        public List<int> ParameterIndexes { get; set; }
        public List<BitwiseLabel> BitwiseLabels { get; set; }
        public float Factor { get; set; }
    }
}
