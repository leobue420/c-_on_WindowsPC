using Avalonia.Controls;
using System;
using System.Windows.Input;
using MyAvaloniaApp.ViewModels;


namespace MyAvaloniaApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
             DataContext = new MainWindowViewModel();
        }


    }
}

