using RMS.Core.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace RMS.Protocols
{
    public class Protocol
    {
        public string Device { get; set; }
        public string ProcotoclHeader { get; set; }
        public int PageNumberIndex { get; set; }
        public DeviceType DeviceType { get; set; }
        public ProtocolType ProtocolType { get; set; }
        public IEnumerable<Page> Pages { get; set; }

        public Page FindPage(int pageNumber)
        {
            if (Pages == null)
                return null;

            return Pages.Where(x => x.PageNumber == pageNumber).FirstOrDefault();
        }

    }
}
