using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        //private const string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        public Password_Hasher(string username)
        {
            this.username = username;
        }

        /*****************
         * Purpose: This function will salt the password using a consistent buffer to make it identifiable
         * Modified: 
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
            string output = Convert.ToBase64String(outputBytes);
            string saltstring = Convert.ToBase64String(salt);
            QueryUserData(this.username,output,saltstring);
            return output;
        }
        /*
        * Function: VerifyHashedPassword (takes 1 string)
        * Author: Brendon Williams
        * Purpose: Takes the password a user inputs, and then correctly calls the verify hashed password function
        * Last Modified: 2/11/2024 2:17pm by Author
        */
        public bool VerifyHashedPassword(string providedPassword)
        {
            Connection connection = Connection.connectionSingleton;
            string query = "SELECT SaltedPassword FROM dbo.Users" +
                " WHERE Username =  @Username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Username", username);
                            if (cmd.ExecuteScalar() == null)
                            {
                               return false;
                            }
                            string saltedpassword = (string)cmd.ExecuteScalar();
                            if (VerifyHashedPassword(saltedpassword, providedPassword))
                            {
                                Preferences.Default.Set("Username", username);
                                UpdateUserId();
                                return true;
                            }
                            else return false;
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
                return false;
            }
            return false;
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
            Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);

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

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
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
        /*
        * Function: QueryUserData
        * Author: Brendon Williams
        * Purpose: This function queries all the users data in one bunch
        * Last Modified: 1/27/2024 by Author
        */
        private static void QueryUserData(string username, string salted_password, string salt)
        {
            

            string query = "INSERT INTO dbo.Users (Username,SaltedPassword, Salt)" +
                "VALUES (@username,@saltedpassword,@salt);";

            try
            {
                using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@saltedpassword", salted_password);
                            cmd.Parameters.AddWithValue("@salt", salt);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }
        /*
        * Function: GrabUserId
        * Author: Brendon Williams
        * Purpose: Updates our static class's UserId. Only called if password matches
        * Last Modified: 2/11/2024 2:33pm by Author
        */
        private void UpdateUserId()
        {


            string query = "SELECT UID from dbo.Users" +
                " WHERE Username = @Username";

            try
            {
                using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Username", username);
                            Preferences.Default.Set("UserId",(int)cmd.ExecuteScalar());
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }

    }
}
