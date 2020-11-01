using System;

namespace RMS.Component.Common
{
    public class Commands
    {
        private static string applicationName = "cmd.exe";
        public static string ChangePassword(string username, string password)
        {
            string output = string.Empty;
            try
            {
                string command = string.Format("net user {0} {1}", username, password);
                output = CommandLine.RunExternalExe(applicationName, string.Format(@"/C {0}", command));
            }
            catch (Exception)
            {
            }

            return output;
        }

        public static string GetUserAccounts()
        {
            string command = "wmic useraccount get user";
            string output = CommandLine.RunExternalExe(applicationName, string.Format(@"/C {0}", command));
            return output;
        }

        public static string DisableUserAccounts(string username)
        {
            string command = string.Format("wmic useraccount where name='{0}' set disabled=false", username);
            string output = CommandLine.RunExternalExe(applicationName, string.Format(@"/C {0}", command));
            return output;
        }

        public static string EnableUserAccounts(string username)
        {
            string command = string.Format("wmic useraccount where name='{0}' set disabled=false", username);
            string output = CommandLine.RunExternalExe(applicationName, string.Format(@"/C {0}", command));
            return output;
        }

        public static string RestartSystem(int seconds)
        {
            string command = string.Format("shutdown -r /t {0}", seconds);//string.Format("wmic useraccount where name='{0}' set disabled=false", username);
            string output = CommandLine.RunExternalExe(applicationName, string.Format(@"/C {0}", command));
            return output;
        }


    }
}
