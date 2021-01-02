using System.Collections.Generic;

namespace RMS.Server.DataTypes.Email
{
    public class EmailTemplate
    {
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public bool IsHtml { get; set; }
        public List<string> ToEmailAddresses { get; set; }
        public List<string> CcEmailAddresses { get; set; }
        public List<string> BccEmailAddresses { get; set; }
    }
}
