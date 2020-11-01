namespace RMS.Component.Communication.Tcp.Common
{
    public enum ErrorType
    {
        NullConfiguration,
        ServerInitialization,
        ClientInitialization,
        NoCertificateFoundWhenTlsEnabled,
        ErrorWhileClosingServerGracefully,
        TargetHostDnsNameNotFound
    }
}
