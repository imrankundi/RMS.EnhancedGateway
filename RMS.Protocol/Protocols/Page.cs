using System.Collections.Generic;

namespace RMS.Protocols
{
    public class Page
    {
        public string PageName { get; set; }
        public int PageNumber { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}
