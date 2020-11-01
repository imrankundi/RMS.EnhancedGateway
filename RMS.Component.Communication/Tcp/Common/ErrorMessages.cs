namespace RMS.Component.Communication.Tcp.Common
{
    public sealed class ErrorMessages
    {
        public static string Message(ErrorType errorType)
        {
            switch (errorType)
            {
                case ErrorType.NullConfiguration:
                    return "No configuration found";
                case ErrorType.ClientInitialization:
                    return "Unable to intialize client";
                case ErrorType.ErrorWhileClosingServerGracefully:
                    return "Unable to close server connection gracefully";
                case ErrorType.NoCertificateFoundWhenTlsEnabled:
                    return "No certificate found when TLS is enabled";
                case ErrorType.ServerInitialization:
                    return "Unable to initialize server";
                case ErrorType.TargetHostDnsNameNotFound:
                    return "Unable to find DNS name";
                default:
                    return "No error message found";
            }
        }
    }
}
