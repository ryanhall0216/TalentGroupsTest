using TalentGroupsTest.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TalentGroupsTest.Data.Services
{
    public class CryptoService
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("18374726482736262847263716273627"); // 32 bytes for AES-256
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("2748462046063738"); // 16 bytes for AES block size

        public static User EncryptUser(User user)
        {
            var encryptedUser = new User()
            {
                Id = user.Id,
                FirstName = EncryptString(user.FirstName),
                LastName = EncryptString(user.LastName),
                DOB = user.DOB,
                EmailAddress = EncryptString(user.EmailAddress),
                Address = EncryptString(user.Address),
                DateCreated = user.DateCreated,
                DateUpdated = user.DateUpdated
            };
            return encryptedUser;
        }

        public static User DecryptUser(User user)
        {
            var decryptedUser = new User()
            {
                Id = user.Id,
                FirstName = DecryptString(user.FirstName),
                LastName = DecryptString(user.LastName),
                DOB = user.DOB,
                EmailAddress = DecryptString(user.EmailAddress),
                Address = DecryptString(user.Address),
                DateCreated = user.DateCreated,
                DateUpdated = user.DateUpdated
            };
            return decryptedUser;
        }
        public static string EncryptString(string plainText)
        {
            if (plainText == null) return null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return null;

            try
            {
                cipherText = cipherText.Trim();
                int mod4 = cipherText.Length % 4;
                if (mod4 != 0)
                {
                    cipherText += new string('=', 4 - mod4);
                }

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    aesAlg.Padding = PaddingMode.PKCS7;  // Ensure padding is set to PKCS7

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Decryption failed: {ex.Message}");
                return null; // Or handle the error as needed, potentially rethrowing the exception or logging it
            }
        }
    }
}
