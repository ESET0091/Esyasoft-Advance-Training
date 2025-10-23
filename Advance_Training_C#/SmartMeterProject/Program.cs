using System.Text;
using System;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SmartMeterProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("User Bulk Insert Application");
            Console.WriteLine("=============================");

            // Replace with your actual connection string
            string connectionString = @"Data Source=LAPTOP-M86ENGOL;Initial Catalog=SmartMeterDB;Trusted_Connection=True;TrustServerCertificate=true;";

            try
            {
                Console.WriteLine("Starting to insert 2000 users...");

                var userInserter = new UserInserter();

                // Choose one method:

                // Method 1: Simple insert (slower but simpler)
                // userInserter.InsertUsersSimple(connectionString);

                // Method 2: Bulk insert (faster - recommended)
                userInserter.InsertUsersSimple(connectionString);

                Console.WriteLine("\n✅ Completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                Console.WriteLine($"Details: {ex.InnerException?.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    public class UserInserter
    {
        // Method 1: Simple sequential insert
        public void InsertUsersSimple(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            for (int i = 1; i <= 2000; i++)
            {
                string sql = @"
                    INSERT INTO [User] (Username, PasswordHash, DisplayName, Email, Phone, LastLoginUtc, IsActive)
                    VALUES (@Username, @PasswordHash, @DisplayName, @Email, @Phone, @LastLoginUtc, @IsActive)";

                using var command = new SqlCommand(sql, connection);

                command.Parameters.Add("@Username", SqlDbType.NVarChar,100).Value = $"person{i}";
                command.Parameters.AddWithValue("@PasswordHash", HashPassword($"Password{i}!"));
                command.Parameters.AddWithValue("@DisplayName", $"Test User {i}");
                command.Parameters.AddWithValue("@Email", $"user{i}@example.com");
                command.Parameters.AddWithValue("@Phone", $"+1-555-{i:D04}");
                command.Parameters.AddWithValue("@LastLoginUtc", i % 5 == 0 ? DateTime.UtcNow.AddDays(-i) : (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", i % 20 != 0); // 5% of users inactive

                command.ExecuteNonQuery();

                if (i % 100 == 0)
                    Console.WriteLine($"Inserted {i} users...");
            }
        }

        // Method 2: Bulk insert (much faster)
        //public async Task InsertUsersBulkAsync(string connectionString)
        //{
        //    using var connection = new SqlConnection(connectionString);
        //    await connection.OpenAsync();

        //    var dataTable = new System.Data.DataTable();
        //    dataTable.Columns.Add("Username", typeof(byte[]));
        //    dataTable.Columns.Add("PasswordHash", typeof(byte[]));
        //    dataTable.Columns.Add("DisplayName", typeof(string));
        //    dataTable.Columns.Add("Email", typeof(string));
        //    dataTable.Columns.Add("Phone", typeof(string));
        //    dataTable.Columns.Add("LastLoginUtc", typeof(DateTime));
        //    dataTable.Columns.Add("IsActive", typeof(bool));

        //    Console.WriteLine("Generating user data...");

        //    var random = new Random();
        //    for (int i = 1; i <= 2000; i++)
        //    {
        //        dataTable.Rows.Add(
        //            $"user{i}",
        //            HashPassword($"Password{i}!"),
        //            $"Test User {i}",
        //            $"user{i}@example.com",
        //            $"+1-555-{i:D04}",
        //            i % 5 == 0 ? DateTime.UtcNow.AddDays(-random.Next(1, 365)) : (object)DBNull.Value,
        //            i % 20 != 0 // 5% of users inactive
        //        );

        //        if (i % 500 == 0)
        //            Console.WriteLine($"Generated {i} user records...");
        //    }

        //    Console.WriteLine("Starting bulk insert...");

        //    using var bulkCopy = new SqlBulkCopy(connection);
        //    bulkCopy.DestinationTableName = "[User]";
        //    bulkCopy.BatchSize = 1000; // Insert in batches of 1000
        //    bulkCopy.BulkCopyTimeout = 30; // 30 seconds timeout

        //    await bulkCopy.WriteToServerAsync(dataTable);
        //}

        private byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
