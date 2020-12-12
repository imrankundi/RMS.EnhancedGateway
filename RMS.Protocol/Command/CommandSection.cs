using System.Collections.Generic;

namespace RMS.Protocols
{
    public class CommandSection
    {
        public string SectionKey { get; private set; }
        private List<string> parameterValues;

        public CommandSection(string sectionKey)
        {
            SectionKey = sectionKey;
            parameterValues = new List<string>();
        }
        public void AddParameterValue(string value)
        {
            parameterValues.Add(value);
        }
        public void ClearParameterValues()
        {
            parameterValues.Clear();
        }
        public void RemoveParameterValue(string value)
        {
            if (parameterValues.Contains(value))
            {
                parameterValues.Remove(value);
            }
        }
        public override string ToString()
        {
            return string.Format("{0}({1})", SectionKey, string.Join(",", parameterValues));
        }
    }
}
