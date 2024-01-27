using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    public class Password_Hasher
    {
        /********
         * Most of this code is given by Microsoft (however, that didn't make it easy to find)
         * I did some modifications so that it has less likely unused functions
         * The original git post is :https://github.com/aspnet/Identity/blob/c7276ce2f76312ddd7fccad6e399da96b9f6fae1/src/Core/PasswordHasher.cs
         * This contains notes of this code, just a bit more complex and probably a bit safer if that route seems like a route we need to take.
         * 
         ********/
        private string username;
        private const int iterCount = 10000;
        private const int saltSize = 16;
        private const int numBytesRequested = 32; 
        public Password_Hasher(string username)
        {
            this.username = username;
        }

        /*****************
         * Purpose: This function will salt the password using a consistent buffer to make it identifiable
         * Modifiable: 
         ****************/

        public string HashPassword(string password)
        {
            var prf = KeyDerivationPrf.HMACSHA256; //Currently runs a more secure 256. Turns out 512 alone is not more secure
            var rng = RandomNumberGenerator.Create();

            var salt = new byte[saltSize];
            rng.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 5, iterCount);
            WriteNetworkByteOrder(outputBytes, 9, saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            //QueryUserData(this.username,Convert.ToBase64String(outputBytes),salt);
            return Convert.ToBase64String(outputBytes);
        }
        /*****************
         * Purpose: This will verify that the password is correct with an inputted password
         * Modified: 
         *****************/
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            if (decodedHashedPassword[0] != 0x01)
                return false;

            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);
            var iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 5);
            var saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 9);

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }
            var salt = new byte[saltLength];
            Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length); //Likely not functioning code for our app. Need to pull from db

            // Read the subkey (the rest of the payload): must be >= 128 bits
            var subkeyLength = decodedHashedPassword.Length - 13 - salt.Length;
            if (subkeyLength < 128 / 8)
            {
                return false;
            }
            var expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            var actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, prf, iterCount, subkeyLength);
            return actualSubkey.SequenceEqual(expectedSubkey);
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value) //These two functions might be removable, will need testing
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }
        /**************
         * Author: Brendon Williams
         * Date: 1/27/2024
         * Purpose: This is a helper function to query user data so that we don't need to have a bunch of db calls open
         * Modified: 
         **************/

        private static void QueryUserData(string username, string salted_password, byte[] salt)
        {
            //Here is for querying user data into the DB. All functions that will be messing with this data will call this function
        }

    }
}
