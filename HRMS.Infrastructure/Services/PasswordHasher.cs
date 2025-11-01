using HRMS.Application.Interfaces.Security;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace HRMS.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] key = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, KeySize);

            byte[] combined = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, combined, 0, SaltSize);
            Buffer.BlockCopy(key, 0, combined, SaltSize, KeySize);

            return Convert.ToBase64String(combined);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            var combined = Convert.FromBase64String(storedHash);

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(combined, 0, salt, 0, SaltSize);

            var storedKey = new byte[KeySize];
            Buffer.BlockCopy(combined, SaltSize, storedKey, 0, KeySize);

            var keyToCheck = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, KeySize);

            return CryptographicOperations.FixedTimeEquals(storedKey, keyToCheck);
        }
    }
}
