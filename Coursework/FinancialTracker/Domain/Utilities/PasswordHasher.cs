using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.Utilities
{
    public static class PasswordHasher
    {

        /// <summary>
        /// Calculate password hash using algorithm SHA-256
        /// </summary>
        /// <param name="password"> Password for hashing </param>
        /// <returns> Hashed password in the format string </returns>
        public static string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Encoding.UTF8.GetString(hashedBytes);
        }
        public static bool Verify(string password, string hash)
        {
            var calculatedHash = Hash(password);
            return calculatedHash == hash;
        }
    }
}
