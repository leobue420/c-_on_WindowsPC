using System.Diagnostics;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace WpfApp1
{
    public class DatabaseTester
    {
        public void TestConnectionAndQuery()
        {
            string connectionString = "Server=LAPTOP-4G7SNHH9\\LEONIWASHERE;Database=ZooProject;Trusted_Connection=True;ConnectRetryCount=0;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Attempting to connect to the database...");
                    connection.Open();
                    Debug.WriteLine("Connection successful!");

                    // Query the dbo.ZooTable table
                    string query = "SELECT * FROM dbo.ZooTable";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Debug.WriteLine("Reading data from dbo.ZooTable...");
                            while (reader.Read())
                            {
                                // Assuming the table has columns ID, Spaltenname, and Typ
                                int id = reader.GetInt32(reader.GetOrdinal("ID"));
                                string spaltenname = reader.GetString(reader.GetOrdinal("Spaltenname"));
                                string typ = reader.GetString(reader.GetOrdinal("Typ"));
                                Debug.WriteLine($"ID: {id}, Spaltenname: {spaltenname}, Typ: {typ}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    Debug.WriteLine("Inner Exception: " + (ex.InnerException?.Message ?? "None"));
                    Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                }
            }
        }
    }

    public partial class MainWindow : Window
    {
        private readonly DatabaseTester _dbTester;

        public MainWindow()
        {
            InitializeComponent();
            _dbTester = new DatabaseTester();
        }

        private void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            _dbTester.TestConnectionAndQuery();
        }
    }

}

