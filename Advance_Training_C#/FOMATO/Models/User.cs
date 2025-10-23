using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOMATO.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public List<Address> Addresses { get; set; }

        string connectionString = @"Data Source=LAPTOP-M86ENGOL;Initial Catalog=FomatoDB;Trusted_Connection=True;";
        public User currentUser;
        public User() { }
        public User(int userId, string name, string email, string phone, string password, bool isAdmin)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Phone = phone;
            Password = password;
            IsAdmin = isAdmin;
            Addresses = new List<Address>();
        }

        public void AddAddress(Address address)
        {
            Addresses.Add(address);
        }

        public void DisplayUserInfo()
        {
            Console.WriteLine($"User ID: {UserId}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Phone: {Phone}");
            Console.WriteLine("Addresses:");
            Console.WriteLine($"Role: {(IsAdmin ? "Admin" : "Customer")}");
            foreach (var address in Addresses)
            {
                Console.WriteLine($"  - {address.GetFullAddress()} ({address.AddressType})");
            }
        }
        public bool Login()
        {
            Console.WriteLine("\n=== LOGIN ===");
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Email and password are required!");
                return false;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    //string query = "SELECT UserId, Name, Email, Phone, Password FROM Users WHERE Email = @Email AND Password = @Password";
                    string query = "SELECT UserId, Name, Email, Phone, Password, IsAdmin FROM Users WHERE Email = @Email AND Password = @Password";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // User found and password matches
                        currentUser = new User(
                            userId: reader.GetInt32(0),
                            name: reader.GetString(1),
                            email: reader.GetString(2),
                            phone: reader.GetString(3),
                            password: reader.GetString(4),
                            isAdmin: reader.GetBoolean(5)
                        );

                        string role = currentUser.IsAdmin ? "Admin" : "Customer";
                        Console.WriteLine($"\n🎉 Login successful! Welcome back, {currentUser.Name} ({role})!");
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid email or password! Please try again.");
                        reader.Close();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during login: " + ex.Message);
                    return false;
                }
            }
        }

        public void CallStoredProcedureAdoNet(string connectionString, string procedureName, string name, string email, string phone, string password)
        {

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Name, email and password are required!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(procedureName, conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Password", password);

                    conn.Open();
                   int rowsAffected=  command.ExecuteNonQuery();
                    Console.WriteLine("Rowsaffected value: ", rowsAffected);
                    if (rowsAffected > 0)
                    {
                        //string role = isAdmin ? "Admin" : "Customer";
                        Console.WriteLine($"\n🎉 Registration successful! Please login with your credentials.");
                    }
                    else
                    {
                        Console.WriteLine("Registration failed! Please try again.");
                    }
                }
            }
        }
            public void Register()
        {
            Console.WriteLine("\n=== REGISTER ===");
            Console.Write("Enter Full Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            Console.Write("Register as Admin? (y/n): ");
            bool isAdmin = Console.ReadLine().ToLower() == "y";

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Name, email and password are required!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // First check if user already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Email", email);

                    int userCount = (int)checkCmd.ExecuteScalar();

                    if (userCount > 0)
                    {
                        Console.WriteLine("User with this email already exists! Please login.");
                        return;
                    }

                    // Insert new user
                    string insertQuery = "INSERT INTO Users (Name, Email, Phone, Password, IsAdmin) VALUES (@Name, @Email, @Phone, @Password, @IsAdmin)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Name", name);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Phone", phone);
                    insertCmd.Parameters.AddWithValue("@Password", password);
                    insertCmd.Parameters.AddWithValue("@IsAdmin", isAdmin);

                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string role = isAdmin ? "Admin" : "Customer";
                        Console.WriteLine($"\n🎉 Registration successful as {role}! Please login with your credentials.");
                    }
                    else
                    {
                        Console.WriteLine("Registration failed! Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during registration: " + ex.Message);
                }
            }
        }
    }
}
