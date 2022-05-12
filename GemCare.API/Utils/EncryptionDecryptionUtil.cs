using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.API.Utils
{
    public interface IEncryptionDecryptionHelper
    {
        string Encrypt(string text);
        bool IsValidApiKey(string encryptedText);
        int GetUserId(string refreshToken);
    }
    public class EncryptionDecryptionHelper : IEncryptionDecryptionHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string EncryptionKey;
        private readonly string EncryptionIV;
        private const string APIKEYS_SECTION = "ApiKeys";
        public EncryptionDecryptionHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            EncryptionKey = _configuration.GetSection(APIKEYS_SECTION).GetSection(nameof(EncryptionKey)).Value;
            EncryptionIV = _configuration.GetSection(APIKEYS_SECTION).GetSection(nameof(EncryptionIV)).Value;
        }

        public string Encrypt(string text)
        {
            try
            {
                var _key = Encoding.UTF8.GetBytes(EncryptionKey);
                var _iv = Encoding.UTF8.GetBytes(EncryptionIV);
                using var aes = Aes.Create();
                //var _iv = aes.IV;
                using var encryptor = aes.CreateEncryptor(_key, _iv);
                using var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using var sw = new StreamWriter(cs);
                    sw.Write(text);
                }

                var iv = _iv;
                var encrypted = ms.ToArray();

                var result = new byte[iv.Length + encrypted.Length];

                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

                return Convert.ToBase64String(result);
            }
            catch { return string.Empty; }
        }

        public bool IsValidApiKey(string encryptedText)
        {
            var result = Decrypt(encryptedText);
            var apiText = _configuration.GetSection(APIKEYS_SECTION).GetSection("API_Text").Value;
            return result.Equals(apiText);
        }

        public int GetUserId(string refreshToken)
        {
            var result = Decrypt(refreshToken);
            return int.Parse(result);
        }

        private string Decrypt(string encryptedText)
        {
            try
            {
                var b = Convert.FromBase64String(encryptedText);

                var iv = new byte[16];
                var cipher = new byte[16];

                Buffer.BlockCopy(b, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(b, iv.Length, cipher, 0, iv.Length);

                var _key = Encoding.UTF8.GetBytes(EncryptionKey);

                using var aes = Aes.Create();
                using var decryptor = aes.CreateDecryptor(_key, iv);
                var result = string.Empty;
                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                    using var sr = new StreamReader(cs);
                    result = sr.ReadToEnd();
                }

                return result;
            }
            catch { return string.Empty; }
        }
    }
}
