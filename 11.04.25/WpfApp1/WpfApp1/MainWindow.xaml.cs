using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using System.Linq;
using System.Diagnostics;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly ZooProjectContext _context;
        private ZooTable _selectedZoo;

        public MainWindow(ZooProjectContext context)
        {
            InitializeComponent();
            _context = context;
            LoadZoos();
            LoadDeletedZoos();
        }

        private void LoadZoos()
        {
            var activeZoos = _context.ZooTables.Where(z => !z.IsDeleted).ToList();
            ZooListBox.ItemsSource = activeZoos;
            Debug.WriteLine("Zoos loaded into ListBox.");
        }

        private void ZooListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ZooListBox.SelectedItem is ZooTable selectedZoo)
            {
                _selectedZoo = selectedZoo;
                var animals = _context.ZooAnimals
                    .Where(za => za.ZooId == selectedZoo.Id)
                    .Select(za => za.Animal)
                    .ToList();
                AnimalDataGrid.ItemsSource = animals;
                Debug.WriteLine($"Animals loaded for zoo: {selectedZoo.Location}");
                PopulateExistingAnimalsComboBox();
            }
        }

        private void AddZooConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string input = ZooInputTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(input))
            {
                var newZoo = new ZooTable { Location = input, IsDeleted = false };
                _context.ZooTables.Add(newZoo);
                _context.SaveChanges();
                Debug.WriteLine($"Added zoo '{input}' with Id {newZoo.Id}.");
                LoadZoos();
                ZooInputTextBox.Text = ""; // Clear the input
            }
        }

        private void AddAnimalConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string input = AnimalInputTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(input))
            {
                if (_selectedZoo != null)
                {
                    var newAnimal = new Animal { Name = input };
                    _context.Animals.Add(newAnimal);
                    _context.SaveChanges();
                    var newZooAnimal = new ZooAnimal { ZooId = _selectedZoo.Id, AnimalId = newAnimal.Id };
                    _context.ZooAnimals.Add(newZooAnimal);
                    _context.SaveChanges();
                    Debug.WriteLine($"Added animal '{input}' to zoo '{_selectedZoo.Location}' with Id {newAnimal.Id}.");
                    RefreshAnimalDataGrid();
                    AnimalInputTextBox.Text = ""; // Clear the input
                }
                else
                {
                    MessageBox.Show("Please select a zoo first.", "No Zoo Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void DeleteAnimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalDataGrid.SelectedItem is Animal selectedAnimal && _selectedZoo != null)
            {
                var zooAnimal = _context.ZooAnimals
                    .FirstOrDefault(za => za.ZooId == _selectedZoo.Id && za.AnimalId == selectedAnimal.Id);
                if (zooAnimal != null)
                {
                    _context.ZooAnimals.Remove(zooAnimal);
                    _context.SaveChanges();
                    Debug.WriteLine($"Deleted animal '{selectedAnimal.Name}' from zoo '{_selectedZoo.Location}'.");
                    RefreshAnimalDataGrid();
                }
            }
        }

        private void PopulateExistingAnimalsComboBox()
        {
            if (_selectedZoo != null)
            {
                var animals = _context.Animals
                    .Where(a => !_context.ZooAnimals
                        .Any(za => za.AnimalId == a.Id && za.ZooId == _selectedZoo.Id))
                    .ToList();
                ExistingAnimalsComboBox.ItemsSource = animals;
                ExistingAnimalsComboBox.DisplayMemberPath = "Name";
            }
        }

        private void AddExistingAnimalButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ExistingAnimalsComboBox.SelectedItem is Animal selectedAnimal && _selectedZoo != null)
                {
                    _context.ZooAnimals.Add(new ZooAnimal
                    {
                        ZooId = _selectedZoo.Id,
                        AnimalId = selectedAnimal.Id
                    });
                    _context.SaveChanges();
                    RefreshAnimalDataGrid();
                    PopulateExistingAnimalsComboBox();
                }
                else if (_selectedZoo == null)
                {
                    MessageBox.Show("Please select a zoo first.", "No Zoo Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding animal: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine($"Exception in AddExistingAnimalButton_Click: {ex}");
            }
        }

        private void RefreshAnimalDataGrid()
        {
            if (_selectedZoo != null)
            {
                var animals = _context.ZooAnimals
                    .Where(za => za.ZooId == _selectedZoo.Id)
                    .Select(za => za.Animal)
                    .ToList();
                AnimalDataGrid.ItemsSource = animals;
                Debug.WriteLine($"Refreshed AnimalDataGrid for zoo: {_selectedZoo.Location}");
            }
        }

        private void ExistingAnimalsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // No action needed here for now
        }

        private void LoadDeletedZoos()
        {
            var deletedZoos = _context.ZooTables.Where(z => z.IsDeleted).ToList();
            DeletedZoosComboBox.ItemsSource = deletedZoos;
        }

        private void DeleteZooButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedZoo != null)
            {
                if (MessageBox.Show($"Are you sure you want to delete zoo '{_selectedZoo.Location}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _selectedZoo.IsDeleted = true;
                    _context.SaveChanges();
                    LoadZoos();
                    LoadDeletedZoos();
                    AnimalDataGrid.ItemsSource = null;
                    _selectedZoo = null;
                }
            }
            else
            {
                MessageBox.Show("Please select a zoo to delete.", "No Zoo Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RestoreZooButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeletedZoosComboBox.SelectedItem is ZooTable selectedDeletedZoo)
            {
                selectedDeletedZoo.IsDeleted = false;
                _context.SaveChanges();
                LoadZoos();
                LoadDeletedZoos();
            }
            else
            {
                MessageBox.Show("Please select a deleted zoo to restore.", "No Deleted Zoo Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}