using System.Security.Cryptography.X509Certificates;

namespace RMS.Component.Common
{
    public class CertificateHelper
    {
        public static X509Certificate2 GetSystemCertificateBySubjectName(string subjectName)
        {
            return GetSystemCertificate(X509FindType.FindBySubjectName, subjectName);
        }
        public static X509Certificate2 GetSystemCertificateByThumbprint(string thumbprint)
        {
            return GetSystemCertificate(X509FindType.FindByThumbprint, thumbprint);
        }
        public static X509Certificate2 GetSystemCertificateBySerialNumber(string serialNumber)
        {
            return GetSystemCertificate(X509FindType.FindBySerialNumber, serialNumber);
        }
        public static X509Certificate2 GetSystemCertificate(X509FindType findType, string findValue)
        {
            X509Certificate2 certificate = null;
            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certs = store.Certificates.Find(findType, findValue, true);
                store.Close();
                if (certs.Count > 0)
                    certificate = certs[0];
            }
            catch
            {
                certificate = null;
            }

            return certificate;

        }

        //public static List<X509Certificate> GetSystemCertificateList()
        //{
        //    List<X509Certificate> certificates = new List<X509Certificate>();
        //    try
        //    {
        //        X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
        //        store.Open(OpenFlags.ReadOnly);
        //        X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindByThumbprint, "b6bc0cedbb08d2e1fedbd024dabdd4b0062e1d62", false);
        //        store.Close();
        //        foreach(var cert in certs)
        //        {
        //            certificates.Add(cert);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return certificates;
        //}

    }
}
