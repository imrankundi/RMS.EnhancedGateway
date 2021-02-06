using RMS.Component.DataAccess.SQLite.Repositories;
using RMS.Server.DataTypes;
using System;
using System.Collections.Generic;

namespace RMS.Parser
{

    public class UserManager
    {
        public Users Users { get; set; }
        public static UserManager Instance { get; } = new UserManager();
        private UserManager()
        {
            ReadFromDatabase();
        }
        private void ReadFromDatabase()
        {
            try
            {
                var users = LoadUsers();
                Users = new Users();
                Users.UserList = new Dictionary<string, User>();
                if (users != null)
                {
                    foreach (User user in users)
                    {
                        Users.UserList.Add(user.UserName, user);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private IEnumerable<User> LoadUsers()
        {
            var repo = new GatewayConfigRepository();
            var sites = repo.GetUsers();
            var configuration = Component.Mappers.ConfigurationMapper.Map(sites);
            return configuration;
        }

        public void Reload()
        {
            ReadFromDatabase();
        }
        public User FindUserByUserName(string userName)
        {
            if(Users.UserList.ContainsKey(userName))
            {
                return Users.UserList[userName];
            }

            return null;
        }
    }
}
