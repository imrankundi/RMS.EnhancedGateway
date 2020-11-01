using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RMS.Component.Security.Cryptography.Symmetric
{
    public class SymmetricEncryption
    {
        /// <summary>
        /// Number of iterations for block chain.
        /// </summary>
        public static int Iterations = 2;

        /// <summary>
        /// Key size for encrypting/decrypting.
        /// </summary>
        public static int KeySize = 256;

        /// <summary>
        /// Salt for password hashing.
        /// </summary>
        public static byte[] Salt = {
            0x26, 0xdc, 0xff, 0x00,
            0xad, 0xed, 0x7a, 0xee,
            0xc5, 0xfe, 0x07, 0xaf,
            0x4d, 0x08, 0x22, 0x3c
        };

        /// <summary>
        /// Encrypt data using a password and Rijndael.
        /// </summary>
        /// <param name="bytes">Bytes to encrypt.</param>
        /// <param name="password">Password to encrypt with.</param>
        /// <returns>Encrypted bytes.</returns>
        public static byte[] Encrypt(byte[] bytes, string password)
        {
            return Encrypt<RijndaelManaged>(bytes, password);
        }

        /// <summary>
        /// Encrypt data using a password and a given algorithm.
        /// </summary>
        /// <typeparam name="T">Symmetric algorithm to use.</typeparam>
        /// <param name="bytes">Bytes to encrypt.</param>
        /// <param name="password">Password to encrypt with.</param>
        /// <returns>Encrypted bytes.</returns>
        public static byte[] Encrypt<T>(byte[] bytes, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] encrypted;

            using (var cipher = new T())
            {
                var passwordBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), Salt, Iterations);
                var keyBytes = passwordBytes.GetBytes(KeySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (var encryptor = cipher.CreateEncryptor(keyBytes, passwordBytes.GetBytes(16)))
                {
                    using (var stream = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(bytes, 0, bytes.Length);
                            writer.FlushFinalBlock();

                            encrypted = stream.ToArray();
                        }
                    }
                }

                cipher.Clear();
            }

            return encrypted;
        }

        public static string Encrypt<T>(string plainText, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = Encrypt<T>(bytes, password);
            string base64String = Convert.ToBase64String(encryptedBytes);
            return base64String;
        }
        /// <summary>
        /// Decrypt data using a password and Rijndael.
        /// </summary>
        /// <param name="encrypted">Bytes to decrypt.</param>
        /// <param name="password">Password to decrypt with.</param>
        /// <returns>Decrypted bytes.</returns>
        public static byte[] Decrypt(byte[] encrypted, string password)
        {
            return Decrypt<RijndaelManaged>(encrypted, password);
        }

        /// <summary>
        /// Decrypt data using a password and a given algorithm.
        /// </summary>
        /// <typeparam name="T">Symmetric algorithm to use.</typeparam>
        /// <param name="encrypted">Bytes to decrypt.</param>
        /// <param name="password">Password to decrypt with.</param>
        /// <returns>Decrypted bytes.</returns>
        public static byte[] Decrypt<T>(byte[] encrypted, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] decrypted;

            using (var cipher = new T())
            {
                var passwordBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), Salt, Iterations);
                var keyBytes = passwordBytes.GetBytes(KeySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (var decrypter = cipher.CreateDecryptor(keyBytes, passwordBytes.GetBytes(16)))
                {
                    using (var stream = new MemoryStream(encrypted))
                    {
                        using (var reader = new CryptoStream(stream, decrypter, CryptoStreamMode.Read))
                        {
                            decrypted = new byte[stream.Length];
                            reader.Read(decrypted, 0, decrypted.Length);
                        }
                    }
                }

                cipher.Clear();
            }

            return decrypted
                .Where(b => b != 0)
                .ToArray();
        }

        public static string Decrypt<T>(string cipherText, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] bytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = Decrypt<T>(bytes, password);
            string plainText = Encoding.UTF8.GetString(decryptedBytes);
            return plainText;
        }

        /*--------------------------------------------- 3DES ---------------------------------------------*/
        public static string Encrypt3DES(string plainText, string password, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);

            string key = password;
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt3DES(string cipherText, string password, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] cipherBytes = Convert.FromBase64String(cipherText);


            //Get your key from config file to open the lock!
            string key = password;

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 cipherBytes, 0, cipherBytes.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return Encoding.UTF8.GetString(resultArray);
        }

        /*--------------------------------------------- 3DES ---------------------------------------------*/
    }
}
