//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SmartMeterProject
//{
//    public class UserInserter
//    {
//        public void Insert2000Users()
//        {
//            string connectionString = @"Data Source=LAPTOP-M86ENGOL;Initial Catalog=FomatoDB;Trusted_Connection=True;";

//            using var connection = new SqlConnection(connectionString);
//            connection.Open();

//            for (int i = 1; i <= 2000; i++)
//            {
//                string username = $"user{i}";
//                string displayName = $"User {i}";
//                string email = $"user{i}@example.com";
//                string phone = $"+1-555-{i:D04}";

//                // Create a simple password hash
//                string password = $"Password{i}!";
//                byte[] passwordHash = HashPassword(password);

//                string sql = @"
//                    INSERT INTO [User] (Username, PasswordHash, DisplayName, Email, Phone, LastLoginUtc, IsActive)
//                    VALUES (@Username, @PasswordHash, @DisplayName, @Email, @Phone, @LastLoginUtc, @IsActive)";

//                using (var command = new SqlCommand(sql, connection))
//                {
//                    command.Parameters.AddWithValue("@Username", username);
//                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
//                    command.Parameters.AddWithValue("@DisplayName", displayName);
//                    command.Parameters.AddWithValue("@Email", email);
//                    command.Parameters.AddWithValue("@Phone", phone);
//                    command.Parameters.AddWithValue("@LastLoginUtc", i % 10 == 0 ? DateTime.UtcNow : (object)DBNull.Value); // Some users have last login
//                    command.Parameters.AddWithValue("@IsActive", i % 50 != 0); // Make some users inactive

//                    command.ExecuteNonQuery();
//                }

//                if (i % 100 == 0)
//                    Console.WriteLine($"Inserted {i} users...");
//            }

//            Console.WriteLine("2000 users inserted successfully!");
//        }

//        //private byte[] HashPassword(string password)
//        //{
//        //    using (var sha256 = SHA256.Create())
//        //    {
//        //        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//        //    }
//        //}
//    }
//}
