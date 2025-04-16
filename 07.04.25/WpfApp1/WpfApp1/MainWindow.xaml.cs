using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;




namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ConnectButton_Click(object sender, RoutedEventArgs e)
    {
        string connectionString = "Server=LAPTOP-4G7SNHH9\\LEONIWASHERE;Database=ZooProject;Trusted_Connection=True;Encrypt=False;ConnectRetryCount=0;";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                Debug.WriteLine("Attempting to connect to the database...");
                connection.Open();
                Debug.WriteLine("Connection successful!");

                // Verify that we can access dbo.ZooTable by running a simple query
                string query = "SELECT TOP 1 ID, location FROM dbo.ZooTable";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("ID"));
                            string location = reader.GetString(reader.GetOrdinal("location"));
                            StatusTextBlock.Text = $"Connection successful! Found row: ID={id}, Location={location}";
                            Debug.WriteLine($"Found row: ID={id}, Location={location}");
                        }
                        else
                        {
                            StatusTextBlock.Text = "Connection successful, but no data found in dbo.ZooTable.";
                            Debug.WriteLine("No data found in dbo.ZooTable.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Connection failed: " + ex.Message;
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Inner Exception: " + (ex.InnerException?.Message ?? "None"));
                Debug.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }


    }
}