using System;
using System.Security.Cryptography;
using System.Text;

namespace RMS.Component.Security
{
    public static class EncryptionUtility
    {

        public static string CreateSaltFull(int numBytes)
        {
            if (numBytes < 1)
                throw new ArgumentException(nameof(numBytes));

            var buff = new byte[numBytes];
            new RNGCryptoServiceProvider().GetNonZeroBytes(buff);
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        ///     Create a salt of exactly the number of characters required.
        ///     Under the hood, it calls CreateSaltFull() and trims the string to the required length.
        /// </summary>
        /// <param name="numChars">The number of characters required in the salt</param>
        /// <returns>A salt</returns>
        public static string CreateSalt(int numChars)
        {
            if (numChars < 1)
                throw new ArgumentException(nameof(numChars));

            return CreateSaltFull(numChars).Substring(0, numChars);
        }

        /// <summary>
        ///     Creates a password with the required length. You can specify if you want to allow punctuation characters in the
        ///     retuned password.
        ///     For more information on punctuation characters, see http://msdn.microsoft.com/en-us/library/6w3ahtyy.aspx
        /// </summary>
        /// <param name="size">The number of characters in the returned password</param>
        /// <param name="allowPunctuation">If true allows letters, digits and puctuation. If false only allows letters and digits.</param>
        /// <returns>Password</returns>
        public static string CreatePassword(int size, bool allowPunctuation)
        {
            if (size < 1)
                throw new ArgumentException(nameof(size));

            var s = new StringBuilder();
            const int saltLen = 100;

            var pass = 0;
            while (pass < size)
            {
                var salt = CreateSaltFull(saltLen);
                for (var n = 0; n < saltLen; n++)
                {
                    var ch = salt[n];
                    var punctuation = char.IsPunctuation(ch);
                    if (!allowPunctuation && punctuation)
                        continue;

                    if (!char.IsLetterOrDigit(ch) && !punctuation)
                        continue;

                    s.Append(ch);
                    if (++pass == size)
                        break;
                }
            }
            return s.ToString();
        }


        public static int BufferLen = 4096;
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();
        private static PaddingMode _paddingMode = PaddingMode.ISO10126;
        private static CipherMode _cipherMode = CipherMode.CBC;

        public enum BlockSize
        {
            Default = 256,
            Size128 = 128,
            Size192 = 192,
            Size256 = 256
        }

        public enum KeySize
        {
            Default = 256,
            Size128 = 128,
            Size192 = 192,
            Size256 = 256
        }

        public static void ResetPaddingAndCipherModes()
        {
            _paddingMode = PaddingMode.ISO10126;
            _cipherMode = CipherMode.CBC;
        }

        public static bool SetPaddingAndCipherModes(PaddingMode paddingMode, CipherMode cipherMode)
        {
            if (paddingMode == PaddingMode.PKCS7 && (cipherMode == CipherMode.OFB || cipherMode == CipherMode.CTS))
                return false; // invalid
            if (paddingMode == PaddingMode.Zeros)
                return false; // invalid and/or encrypt/decrypt will mismatch
            if (paddingMode == PaddingMode.ANSIX923 && (cipherMode == CipherMode.OFB || cipherMode == CipherMode.CTS))
                return false; // invalid
            if (paddingMode == PaddingMode.ISO10126 && (cipherMode == CipherMode.OFB || cipherMode == CipherMode.CTS))
                return false; // invalid

            _paddingMode = paddingMode;
            _cipherMode = cipherMode;

            return true;
        }

        private static RijndaelManaged GetRijndaelManaged(byte[] key, byte[] iv, KeySize keySize, BlockSize blockSize)
        {
            var rm = new RijndaelManaged
            {
                KeySize = (int)keySize,
                BlockSize = (int)blockSize,
                Padding = _paddingMode,
                Mode = _cipherMode
            };

            if (key != null)
                rm.Key = key;

            if (iv != null)
                rm.IV = iv;

            return rm;
        }

        /// <summary>
        ///     Returns an encryption key to be used with the Rijndael algorithm
        /// </summary>
        public static byte[] GenerateKey()
        {
            return GenerateKey(KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        ///     Returns an encryption key to be used with the Rijndael algorithm
        /// </summary>
        public static byte[] GenerateKey(KeySize keySize, BlockSize blockSize)
        {
            using (var rm = GetRijndaelManaged(null, null, keySize, blockSize))
            {
                rm.GenerateKey();
                return rm.Key;
            }
        }

        /// <summary>
        ///     Returns an encryption key to be used with the Rijndael algorithm
        /// </summary>
        /// <param name="password">Password to create key with</param>
        /// <param name="salt">Salt to create key with</param>
        /// <param name="keySize">Can be 128, 192, or 256</param>
        /// <param name="iterationCount">The number of iterations to derive the key.</param>
        public static byte[] GenerateKey(string password, string salt, KeySize keySize, int iterationCount)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(salt)) throw new ArgumentNullException(nameof(salt));

            var saltValueBytes = Encoding.Unicode.GetBytes(salt);
            if (saltValueBytes.Length < 8)
                throw new ArgumentException("Salt is not at least eight bytes");

            var derivedPassword = new Rfc2898DeriveBytes(password, saltValueBytes, iterationCount);
            return derivedPassword.GetBytes((int)keySize / 8);
        }

        /// <summary>
        ///     Returns the encryption IV to be used with the Rijndael algorithm
        /// </summary>
        public static byte[] GenerateIV()
        {
            return GenerateIV(KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        ///     Returns the encryption IV to be used with the Rijndael algorithm
        /// </summary>
        public static byte[] GenerateIV(KeySize keySize, BlockSize blockSize)
        {
            using (var rm = GetRijndaelManaged(null, null, keySize, blockSize))
            {
                rm.GenerateIV();
                return rm.IV;
            }
        }

        /// <summary>
        /// Converts HEX string to byte array.
        /// Opposite of ByteArrayToHex.
        /// </summary>
        public static byte[] HexToByteArray(string hexString)
        {
            if (hexString == null) throw new ArgumentNullException(nameof(hexString));

            if ((hexString.Length % 2) != 0)
                throw new ApplicationException("Hex string must be multiple of 2 in length");

            var byteCount = hexString.Length / 2;
            var byteValues = new byte[byteCount];
            for (var i = 0; i < byteCount; i++)
            {
                byteValues[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return byteValues;
        }

        /// <summary>
        /// Convert bytes to 2 hex characters per byte, "-" separators are removed.
        /// Opposite of HexToByteArray
        /// </summary>
        public static string ByteArrayToHex(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return BitConverter.ToString(data).Replace("-", "");
        }

        /// <summary>
        /// Use cryptographically strong random number generator to fill buffer with random data.
        /// </summary>
        public static void GetRandomBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            Rng.GetBytes(buffer);
        }
    }
}
