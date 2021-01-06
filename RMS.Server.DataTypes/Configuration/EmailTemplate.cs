using System.Collections.Generic;

namespace RMS.Server.DataTypes
{
    public class EmailTemplate
    {
        public long Id { get; set; }
        public string TemplateKey { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateMessage { get; set; }
        public bool IsHtml { get; set; }
        public virtual IEnumerable<EmailSubscriber> Subscribers { get; set; }
    }
}
