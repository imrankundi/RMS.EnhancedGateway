using RMS.Component.Security.Cryptography.Symmetric;
using System.Security.Cryptography;

namespace RMS.Component.DataAccess
{
    public class DatabaseEncryptor
    {
        private const string password = "PKPSAssetsSymetricCryptography";
        public static string Encrypt(string plainText)
        {
            return SymmetricEncryption.Encrypt<AesManaged>(plainText, password);
        }
        public static string Decrypt(string plainText)
        {
            return SymmetricEncryption.Decrypt<AesManaged>(plainText, password);
        }
    }
}
