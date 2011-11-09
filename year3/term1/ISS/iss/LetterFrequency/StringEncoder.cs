using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace LetterFrequency
{
    public static class StringEncoder
    {
        public static string Encode(string source, string password)
        {
            byte[] source_data = Encoding.UTF8.GetBytes(source);

            SymmetricAlgorithm symAlgorithmIn = Rijndael.Create();

            ICryptoTransform cryptoTransformIn = symAlgorithmIn.CreateEncryptor((new PasswordDeriveBytes(password, null)).GetBytes(16), new byte[16]);
            MemoryStream memoryStreamIn = new MemoryStream();

            CryptoStream cryptoSystemIn = new CryptoStream(memoryStreamIn, cryptoTransformIn, CryptoStreamMode.Write);

            cryptoSystemIn.Write(source_data, 0, source_data.Length);
            cryptoSystemIn.FlushFinalBlock();
            cryptoSystemIn.Close();

            return Convert.ToBase64String(memoryStreamIn.ToArray());
        }

        public static string Decode(string crypt_str, string password)
        {
            byte[] crypt_data = Convert.FromBase64String(crypt_str);

            SymmetricAlgorithm symAlgorithmOut = Rijndael.Create();

            ICryptoTransform cryptoTransformOut = symAlgorithmOut.CreateDecryptor((new PasswordDeriveBytes(password, null)).GetBytes(16), new byte[16]);

            MemoryStream memoryStreamOut = new MemoryStream(crypt_data);

            CryptoStream cryptoSystemOut = new CryptoStream(memoryStreamOut, cryptoTransformOut, CryptoStreamMode.Read);
            StreamReader streamReaderOut = new StreamReader(cryptoSystemOut);
            string target = streamReaderOut.ReadToEnd();
            streamReaderOut.Close();
            return target;
        }
    }
}
