using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RMS.Core.Security;
using RMS.Parser;
using RMS.Protocols;
using RMS.Protocols.GT;
using RMS.Server.WebApi.Configuration;
using System;

namespace RMS.Test
{
    [TestClass]
    public class GenericTest
    {

        [TestMethod]
        public void LoadWebApiConfiguration()
        {
            var jwtSettings = WebApiServerConfigurationManager.Instance.Configurations.JwtSettings;
        }
        [TestMethod]
        public void LoadUsers()
        {
            var users = UserManager.Instance.Users;
        }
        [TestMethod]
        public void GenerateHash()
        {
            var hash = HashGenerator.ComputeHash("admin");
        }
    }
}
