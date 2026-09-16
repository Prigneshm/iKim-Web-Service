using System.Security.Cryptography;
using System.Text;
using System;

namespace IKimWebService.Infrastructure.Utils
{
    public static class PasswordEncryptor
    {
        public static string Encrypt(string password, string salt)
        {
            var normalizedPassword = password.Trim().ToLowerInvariant();
            var normalizedSalt = salt.Trim().ToLowerInvariant();

            var hashedBytes = ComputeHashWithSalt(
                Encoding.UTF8.GetBytes(normalizedPassword),
                Encoding.UTF8.GetBytes(normalizedSalt)
            );

            return Convert.ToBase64String(hashedBytes);
        }

        private static byte[] ComputeHashWithSalt(byte[] passwordBytes, byte[] saltBytes)
        {
            var sha256 = SHA256.Create();
            return sha256.ComputeHash(Combine(passwordBytes, saltBytes));
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            var result = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, result, 0, first.Length);
            Buffer.BlockCopy(second, 0, result, first.Length, second.Length);
            return result;
        }
    }
}
