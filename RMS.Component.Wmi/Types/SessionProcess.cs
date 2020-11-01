using System.Collections.Generic;
using System.Linq;

namespace RMS.Component.Wmi
{
    public class SessionProcess : WmiBaseType
    {
        public SessionProcess() : base(WmiClasses.Win32_SessionProcess)
        {
        }
        [WmiProperty(Ignore = true)]
        public Process Dependent { get { return Dependents().FirstOrDefault(); } }

        public IEnumerable<Process> Dependents()
        {
            return WmiObjectCreator.QueryObject<Process>();
        }

    }
}
