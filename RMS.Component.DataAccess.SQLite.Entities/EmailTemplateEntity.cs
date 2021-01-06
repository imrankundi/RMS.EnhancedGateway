using System.Collections.Generic;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class EmailTemplateEntity
    {
        public long Id { get; set; }
        public string TemplateKey { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateMessage { get; set; }
        public bool IsHtml { get; set; }
        public virtual IEnumerable<EmailSubscriberEntity> Subscribers { get; set; }
    }
}
