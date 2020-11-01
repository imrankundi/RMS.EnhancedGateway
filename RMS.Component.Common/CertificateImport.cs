namespace RMS.Component.Common
{
    public class CertificateImport
    {

        private static string applicationName = "cmd.exe";
        public static string ImportCertificate(string certificateStore, string certificatePath)
        {
            string command = string.Format("certutil -addstore \"{0}\" \"{1}\"", certificateStore, certificatePath);
            string output = CommandLine.RunExternalExe(applicationName, string.Format(@"/C {0}", command));
            return output;
        }

        public static string ImportPfxCertificate(string certificatePath)
        {
            string command = string.Format("certutil -importpfx \"{0}\"", certificatePath);
            string output = CommandLine.RunExternalExe(applicationName, string.Format(@"/C {0}", command));
            return output;
        }

    }
}
