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

    private void AddToDoButton_Click(object sender, RoutedEventArgs e )
    {
        string ToDoText = ToDo_Textbox.Text;

        MessageBox.Show(ToDoText);

        if (!string.IsNullOrEmpty(ToDoText))
        {
            TextBlock ToDo_Item = new TextBlock
            {
                Text = ToDoText,

                 Margin = new Thickness(0, 5, 0, 5), // Add some spacing between items

                 Foreground = new SolidColorBrush(Colors.White)
            };

            ToDo_StackPanel.Children.Add(ToDo_Item);

            ToDo_Textbox.Clear();


        }
    }
}

