using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Utility
{
    public class PasswordHash
    {
        //The following constants may be changed without breaking existing hashes.
        private const int SALT_BYTE_SIZE = 24;
        private const int HASH_BYTE_SIZE = 24;

        //计算次数
        private const int PBKDF2_ITERATIONS = 1000;

        private const int ITERATION_INDEX = 0;
        private const int SALT_INDEX = 1;
        private const int PBKDF2_INDEX = 2;

        ///<summary>
        ///仅产生密码的Hash,
        ///</summary>
        ///<param name="password">The password to hash.</param>
        ///<returns>The hash of the password.</returns>
        public static string CreatePasswordHash(string username, string password)
        {
            ///生成Salt
            string salt = GenerateClientSalt(username);

            byte[] passwordAndSaltBytes = new System.Text.ASCIIEncoding().GetBytes(password + salt);
            byte[] passwordHash = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);

            string hashString = string.Empty;
            foreach (byte x in passwordHash)
            {
                hashString += String.Format("{0:x2}", x);
            }

            return Convert.ToBase64String(new System.Text.ASCIIEncoding().GetBytes(hashString));


        }

        ///<summary>
        ///Creates a salted PBKDF2 hash of the password.
        ///</summary>
        ///<param name="password">The password to hash.</param>
        ///<returns>The hash of the password.</returns>
        public static string CreatePasswordAndSaltHash(string password)
        {
            //Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];

            csprng.GetBytes(salt);

            //Hash the password and encode the parameters
            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);

            //将参数拼接起来
            var hashResult = PBKDF2_ITERATIONS + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            return Base64.EncodeBase64(hashResult);


        }

        /// <summary>
        /// 生成用于客户端加密的Salt
        /// </summary>
        /// <returns></returns>
        private static string GenerateClientSalt(string username)
        {
            var salt = username + Common.Password_Client_Salt;
            return salt;
        }

        ///<summary>
        ///Validates a password given a hash of the correct one.
        ///</summary>
        ///<param name="password">The password to check.</param>
        ///<param name="correctHash">A hash of the correct password.</param>
        ///<returns>True if the password is correct. False otherwise.</returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            //Extract the parameters from the hash
            correctHash = Base64.DecodeBase64(correctHash);
            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            int iterations = Int32.Parse(split[ITERATION_INDEX]);


            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);


            return SlowEquals(hash, testHash);


        }

        ///<summary>
        ///Compares two byte arrays in length-constant time. This comparison
        ///method is used so that password hashes cannot be extracted from
        ///on-line systems using a timing attack and then attacked off-line.
        ///</summary>
        ///<param name="a">The first byte array.</param>
        ///<param name="b">The second byte array.</param>
        ///<returns>True if both byte arrays are equal. False otherwise.</returns>

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)

                diff |= (uint)(a[i] ^ b[i]);

            return diff == 0;

        }

        ///<summary>
        ///Computes the PBKDF2-SHA1 hash of a password.
        ///</summary>
        ///<param name="password">The password to hash.</param>
        ///<param name="salt">The salt.</param>
        ///<param name="iterations">The PBKDF2 iteration count.</param>
        ///<param name="outputBytes">The length of the hash to generate, in bytes.</param>
        ///<returns>A hash of the password.</returns>
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);

            pbkdf2.IterationCount = iterations;

            return pbkdf2.GetBytes(outputBytes);

        }


    }
}
