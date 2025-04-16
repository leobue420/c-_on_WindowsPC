using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;



class Program
{
    static void Main()
    {
        string connectionString = "Server=.\\LEONIWASHERE;Database=CSharpProjectDB;Trusted_Connection=True;TrustServerCertificate=yes;";
        string name;

        Console.Write("Enter a name to add: ");
        name = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connected to the database.");

                // Insert new user with parameterized query to prevent SQL injection
                string insertQuery = "INSERT INTO Users (Name) VALUES (@Name);";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
                    insertCommand.ExecuteNonQuery();
                    Console.WriteLine("User added successfully.");
                }

                // Retrieve and display all users
                string selectQuery = "SELECT * FROM Users;";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    Console.WriteLine("All users in the database:");
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string userName = reader["Name"].ToString();
                        Console.WriteLine($"ID: {id}, Name: {userName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}