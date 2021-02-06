using System.Collections.Generic;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
