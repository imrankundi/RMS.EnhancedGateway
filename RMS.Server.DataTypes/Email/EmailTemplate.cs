using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Server.DataTypes.Email
{
    public class EmailTemplate
    {
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromName { get; set; }
        public List<string> ToEmailAddresses { get; set; }
        public List<string> CcEmailAddresses { get; set; }
        public List<string> BccEmailAddresses { get; set; }
    }
}
