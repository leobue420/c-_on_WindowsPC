using Microsoft.Data.SqlClient;// Library for SQL Server connectivity
using System;// Basic system functions like Console

namespace _04._04._25;

class Program
{

     // Replace with your actual connection string
      // Connection string to connect to SQL Server
        // Server=localhost\\SQLEXPRESS: Specifies your local SQL Server instance
        // Database=PeopleDB: The database we created
        // Trusted_Connection=True: Uses Windows Authentication
        // TrustServerCertificate=True: Avoids certificate validation issues in dev
        static string connectionString = "Server=localhost\\LEONIWASHERE;Database=PeopleDB;Trusted_Connection=True;TrustServerCertificate=True;";


    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // Infinite loop to keep the program running until the user chooses to exit

         while (true)
            {
                 // Display a simple menu
                Console.WriteLine("\n1. View all people");
                Console.WriteLine("2. Add a person");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                // Read user input
                string choice = Console.ReadLine();

                 // Switch statement to handle user choice
                switch (choice)
                {
                    case "1":
                        ViewAllPeople();// Call method to display all records
                        break;
                    case "2":
                        AddPerson();// Call method to add a new person
                        break;
                    case "3":
                        return;// Exit the program by ending Main
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }

    }

 // Method to retrieve and display all people from the database
     static void ViewAllPeople()
        {
            try
            {
                 // SqlConnection manages the connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();// Open the connection to the database
                    string query = "SELECT Name, Age FROM Persons";// SQL query to get names and ages
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // ExecuteReader runs the query and returns a data reader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("\nPeople in database:");
                             // Loop through all rows returned by the query
                            while (reader.Read())
                            {
                                // Access columns by name and display them
                                Console.WriteLine($"Name: {reader["Name"]}, Age: {reader["Age"]}");
                            }
                        }
                    }
                }
            }
             // Catch any errors (e.g., connection issues) and display them
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

// Method to add a new person to the database
        static void AddPerson()
        {
            try
            {
                  // Prompt user for input
                Console.Write("Enter name: "); // Read name as a string
                string name = Console.ReadLine();
                Console.Write("Enter age: ");
                // TryParse safely converts string to int, returns false if invalid
                if (!int.TryParse(Console.ReadLine(), out int age))
                {
                    Console.WriteLine("Invalid age!");
                    return;// Exit method if age isn't a valid number
                }
                // Open a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Parameterized query to prevent SQL injection
                    string query = "INSERT INTO Persons (Name, Age) VALUES (@Name, @Age)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                         // Add parameters to the query with user input
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Age", age);
                         // ExecuteNonQuery runs the insert and returns number of rows affected
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors like database connection failures
                Console.WriteLine($"Error: {ex.Message}");
            }
}
}
