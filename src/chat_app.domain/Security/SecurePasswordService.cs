using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace chat_app.domain
{
    public class SecurePasswordService
    {
        private const int SALT_LENGTH = 255;
        private static readonly RNGCryptoServiceProvider _rng;

        static SecurePasswordService()
        {
            _rng = new RNGCryptoServiceProvider ();
        }

        public (byte[] passwordHash, byte[] salt) ComputeForUser(string userName, string password)
        {
            var nameAndPassword = MergeArrays (Encoding.UTF8.GetBytes (userName), Encoding.UTF8.GetBytes (password));
            var salt = GenerateSalt ();
            var nameAndPasswordSalted = MergeArrays (nameAndPassword, salt);

            using (HashAlgorithm alg = SHA512.Create())
            {
                var passwordHash = alg.ComputeHash (nameAndPasswordSalted);
                return (passwordHash, salt);
            }
        }

        public bool ChallengePassword(string userName, string password, byte[] passwordHash, byte[] salt)
        {
            var nameAndPassword = MergeArrays (Encoding.UTF8.GetBytes (userName), Encoding.UTF8.GetBytes (password));
            var nameAndPasswordSalted = MergeArrays (nameAndPassword, salt);

            using (HashAlgorithm alg = SHA512.Create ())
            {
                var toChallenge = alg.ComputeHash (nameAndPasswordSalted);
                return Enumerable.SequenceEqual (passwordHash, toChallenge);
            }
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[SALT_LENGTH];
            _rng.GetNonZeroBytes (salt);
            return salt;
        }

        private static byte[] MergeArrays(byte[] arr1, byte[] arr2)
        {
            var result = new byte[arr1.Length + arr2.Length];
            Array.Copy (arr1, result, arr1.Length);
            Array.Copy (arr2, 0, result, arr1.Length, arr2.Length);
            return result;
        }
    }
}
