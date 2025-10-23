using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOMATO
{
    internal class DBConnection
    {
        public void DBaseConnection()
        {
            Console.WriteLine("Getting Connection ...");
            var datasource = @"LAPTOP-M86ENGOL"; // your server
            var database = "FomatoDB"; // your database name           
            // Create your connection string
            string connString = @"Data Source=" + datasource +
                ";Initial Catalog=" + database + "; Trusted_Connection=True;";
            Console.WriteLine(connString);

            SqlConnection conn = new SqlConnection(connString);
            try
            {
                Console.WriteLine("Opening Connection ...");
                conn.Open();
                Console.WriteLine("Connection successful!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                // Close the connection
                conn.Close();
            }
        }
    }
}
