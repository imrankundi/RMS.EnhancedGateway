using DotNetty.Handlers.Tls;
using RMS.Component.Communication.Tcp.Client;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace RMS.Component.Communication.Tcp.Common
{
    public class TlsHandlerFactory
    {
        string className = nameof(TlsHandlerFactory);
        public TlsHandler CreateStandardTlsHandler(string targetHost)
        {
            return new TlsHandler(stream => CreateSslStream(stream), CreateClientSettings(targetHost));
        }
        public TlsHandler CreateStandardTlsHandler(string targetHost, List<X509Certificate> certificates)
        {
            return new TlsHandler(stream => CreateSslStream(stream), CreateClientSettings(targetHost, certificates));
        }
        private SslStream CreateSslStream(Stream innerStream)
        {
            return new SslStream(innerStream, true, ValidateCertificate);
        }
        private ClientTlsSettings CreateClientSettings(string targetHost, List<X509Certificate> certificates)
        {
            return new ClientTlsSettings(true, certificates, targetHost);
        }
        private ClientTlsSettings CreateClientSettings(string targetHost)
        {
            return new ClientTlsSettings(targetHost);
        }

        private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {

            if (!ClientChannelConfigurationManager.Instance.Configurations.
                ValidateCertificate)
            {
                Logging.ClientChannelLogger.Instance.Log.Verbose(className, "ValidateCertificate", "Certificate Validation Disabled");
                return true;
            }

            Logging.ClientChannelLogger.Instance.Log.Verbose(className, "ValidateCertificate", "Validating Certificate");
            Logging.ClientChannelLogger.Instance.Log.Information(className, "ValidateCertificate", string.Format("Certificate Policy Error: {0}", sslPolicyErrors.ToString()));

            switch (sslPolicyErrors)
            {
                case SslPolicyErrors.None:
                    Logging.ClientChannelLogger.Instance.Log.Information(className, "ValidateServerCertificate", "Certificated validated");
                    return true;
                default:
                    Logging.ClientChannelLogger.Instance.Log.Information(className, "ValidateServerCertificate", "Invalid Certificate");
                    return false;

            }

        }

        private bool ValidateClientCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
