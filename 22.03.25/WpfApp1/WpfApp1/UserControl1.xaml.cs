using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            var passwordEntered = PasswordBox.Password;

            string? envPw = Environment.GetEnvironmentVariable("InvoiceManagement");


            if (envPw != null)
            {
                if (passwordEntered == envPw)
                {
                    MessageBox.Show("Entered correct Password");
                }
                else
                {
                    MessageBox.Show("Wrong password entered");
                }
            }
            else
            {
                MessageBox.Show("Environment variable not found");
            }
        }

        public void OnPasswordChangedButtonActivation(object sender, EventArgs e)
        {
            LoginButton.IsEnabled = !string.IsNullOrEmpty(PasswordBox.Password)  
        }
    }
}