using System;
using System.Security.Cryptography;
using System.Text;

namespace RMS.Component.Security.Cryptography.Asymetric
{
    public class RsaEncryption
    {
        /// <summary>
        /// Create public and private keys.
        /// </summary>
        /// <param name="publicKey">The created public key.</param>
        /// <param name="privateKey">The created private key.</param>
        /// <param name="keySize">Size of keys.</param>
        /// <returns>Success</returns>
        public static RsaPublicPrivateKeyPair CreateKeys(int keySize = 4096)
        {
            var csp = new CspParameters
            {
                ProviderType = 1,
                Flags = CspProviderFlags.UseArchivableKey,
                KeyNumber = (int)KeyNumber.Exchange
            };

            using (var rsa = new RSACryptoServiceProvider(keySize, csp))
            {
                try
                {
                    RsaPublicPrivateKeyPair keyPair = new RsaPublicPrivateKeyPair();
                    keyPair.PublicKey = rsa.ToXmlString(false);
                    keyPair.PrivateKey = rsa.ToXmlString(true);

                    return keyPair;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// Encrypt data using a public key.
        /// </summary>
        /// <param name="bytes">Bytes to encrypt.</param>
        /// <param name="publicKey">Public key.</param>
        /// <returns>Encrypted bytes.</returns>
        public byte[] Encrypt(byte[] bytes, string publicKey)
        {
            var csp = new CspParameters
            {
                ProviderType = 1
            };

            byte[] encrypted;

            using (var rsa = new RSACryptoServiceProvider(csp))
            {
                try
                {
                    rsa.FromXmlString(publicKey);
                    encrypted = rsa.Encrypt(bytes, false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return encrypted;
        }

        public string EncryptAndEncodeString(string plainText, string publicKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = Encrypt(bytes, publicKey);
            var base64String = Convert.ToBase64String(encryptedBytes);
            return base64String;
        }

        /// <summary>
        /// Decrypt data using a private key.
        /// </summary>
        /// <param name="encrypted">Bytes to decrypt.</param>
        /// <param name="privateKey">Private key.</param>
        /// <returns>Decrypted bytes.</returns>

        public byte[] Decrypt(byte[] encrypted, string privateKey)
        {
            var csp = new CspParameters
            {
                ProviderType = 1
            };

            byte[] bytes;

            using (var rsa = new RSACryptoServiceProvider(csp))
            {
                try
                {
                    rsa.FromXmlString(privateKey);
                    bytes = rsa.Decrypt(encrypted, false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return bytes;
        }

        public string DecodeAndDecryptString(string encryptedText, string privateKey)
        {
            byte[] bytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = Decrypt(bytes, privateKey);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
