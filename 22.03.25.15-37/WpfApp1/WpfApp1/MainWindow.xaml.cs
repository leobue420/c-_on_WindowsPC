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
using System.Diagnostics; // Für Debug.WriteLine

namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    // Wechselkurse relativ zum USD
    private Dictionary<string, double> exchangeRates = new Dictionary<string, double>
    {
        { "USD", 1.0 },   // 1 USD = 1 USD
        { "EUR", 0.85 },  // 1 USD = 0.85 EUR
        { "GBP", 0.75 }   // 1 USD = 0.75 GBP
    };


    public MainWindow()
    {
        InitializeComponent();

        // ComboBox-Elemente mit Beispielwährungen füllen
        ComboboxNameWTF.Items.Add("USD");
        ComboboxNameWTF.Items.Add("EUR");
        ComboboxNameWTF.Items.Add("GBP");

        ToCurrency.Items.Add("USD");
        ToCurrency.Items.Add("EUR");
        ToCurrency.Items.Add("GBP");
    }


    private void NumberValidationCheckbox(object sender, TextCompositionEventArgs e)
    {
        // Prüfen, ob die Eingabe eine Zahl ist
        if (!double.TryParse(e.Text, out _))
        {
            Debug.WriteLine("youarean idiot");
            Debug.WriteLine($"Inhalt der TextBox: {Teatime.Text}");
            e.Handled = true; // Eingabe verwerfen, wenn es keine Zahl ist
        }
    }

    private void ConvertClick(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Youareynidiot2");
        Debug.WriteLine("Convert-Button wurde geklickt!");


        // Ausgewählte Währungen abrufen
        string fromCurrency = ComboboxNameWTF.SelectedItem?.ToString();
        string toCurrency = ToCurrency.SelectedItem?.ToString();

        // Überprüfen, ob Währungen ausgewählt wurden
        if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
        {
            ConvertedValue.Text = "Bitte wähle Währungen aus.";
            return;
        }



        // Konvertierungslogik
        if (double.TryParse(Teatime.Text, out double inputValue))
        {
            // Wechselkurs berechnen
            double fromRate = exchangeRates[fromCurrency];
            double toRate = exchangeRates[toCurrency];
            double exchangeRate = toRate / fromRate; // Wechselkurs von fromCurrency zu toCurrency

            // Konvertierung durchführen
            double convertedValue = inputValue * exchangeRate;

            // Den umgewandelten Wert in der neuen TextBox anzeigen
            ConvertedValue.Text = convertedValue.ToString("F2"); // Mit 2 Dezimalstellen formatieren
        }
        else
        {
            // Falls die Eingabe keine gültige Zahl ist
            ConvertedValue.Text = "Ungültige Eingabe";
        }

    }

    // Ereignishandler für den Reset-Button
    private void ResetClick(object sender, RoutedEventArgs e)
    {
        // Hier kommt die Logik zum Zurücksetzen
        Debug.WriteLine("Yopuarenidiotaswell");
        Debug.WriteLine($"Inhalt der TextBox: {Teatime.Text}"); // Inhalt der TextBox ausgeben
        MessageBox.Show("Reset-Button wurde geklickt!");


        Teatime.Text = string.Empty;
        ConvertedValue.Text = string.Empty;

        // ComboBox-Elemente zurücksetzen (kein Eintrag ausgewählt)
        ComboboxNameWTF.SelectedIndex = -1;
        ToCurrency.SelectedIndex = -1;
    }


}