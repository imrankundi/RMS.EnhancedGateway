using System;
using System.Net.Sockets;
using System.Text;

namespace RMS.Server.WebApi
{

    public class TouchlessTcpClient
    {
        public static string InitiateTouchlessSession(string ip, int port, string message, Encoding encoding,
            int bufferSize = 2048, int receiveTimeoutInSeconds = 30)
        {
            try
            {
                TcpClient client = new TcpClient(ip, port);
                client.ReceiveTimeout = receiveTimeoutInSeconds * 1000;
                byte[] data = encoding.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                data = new byte[bufferSize];
                string responseData = string.Empty;
                int bytes = stream.Read(data, 0, data.Length);
                responseData = encoding.GetString(data, 0, bytes);
                stream.Close();
                client.Close();
                return responseData;
            }
            catch (ArgumentNullException)
            {
                return string.Empty;
            }
            catch (SocketException)
            {
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }


    //public class TouchlessTcpClient
    //{
    //    /// <summary>
    //    /// Need to manually create Pop3Client.Connect TcpStream to force tls1.2 to be used since not on .net 4.6 yet 
    //    /// </summary>
    //    /// <param name="client"></param>
    //    /// <param name="host"></param>
    //    /// <param name="port"></param>
    //    /// <param name="useSsl"></param>
    //    private void OpenConnect(string host, int port, bool useSsl, int receiveTimeout, int sendTimeout, RemoteCertificateValidationCallback certificateValidator)
    //    {
    //        TcpClient clientSocket = new TcpClient();
    //        clientSocket.ReceiveTimeout = receiveTimeout;
    //        clientSocket.SendTimeout = sendTimeout;

    //        try
    //        {
    //            clientSocket.Connect(host, port);
    //        }
    //        catch (SocketException e)
    //        {
    //            // Close the socket - we are not connected, so no need to close stream underneath
    //            clientSocket.Close();
    //        }

    //        Stream stream;
    //        if (useSsl)
    //        {
    //            // If we want to use SSL, open a new SSLStream on top of the open TCP stream.
    //            // We also want to close the TCP stream when the SSL stream is closed
    //            // If a validator was passed to us, use it.
    //            SslStream sslStream;
    //            if (certificateValidator == null)
    //            {
    //                sslStream = new SslStream(clientSocket.GetStream(), false);
    //            }
    //            else
    //            {
    //                sslStream = new SslStream(clientSocket.GetStream(), false, certificateValidator);
    //            }
    //            sslStream.ReadTimeout = receiveTimeout;
    //            sslStream.WriteTimeout = sendTimeout;

    //            // Authenticate the server
    //            sslStream.AuthenticateAsClient(host, null, System.Security.Authentication.SslProtocols.Tls12, true);

    //            stream = sslStream;
    //        }
    //        else
    //        {
    //            // If we do not want to use SSL, use plain TCP
    //            stream = clientSocket.GetStream();
    //        }

    //        // Now do the connect with the same stream being used to read and write to
    //        //client.Connect(stream);
    //    }


    //}
}
