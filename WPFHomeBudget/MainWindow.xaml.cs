using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ZAMWPFHomeBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        public Presenter presenter;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseBudgetButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".png";
            openFileDlg.Filter = "Database Files (*.db)|*.db";
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                presenter = new Presenter(this, openFileDlg.FileName, false);
                presenter.OpenDatabaseForm(openFileDlg.FileName);
            }
            else
            {
                MessageBox.Show("Invalid File Path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PreviousBudgetButton_Click(object sender, RoutedEventArgs e )
        {
            try
            {
                presenter = new Presenter(this);
                string lastFile = presenter.GetLastFile();
                presenter = new Presenter(this, lastFile, false);
                presenter.OpenDatabaseForm(lastFile);
            }
            catch (Exception exception)
            {
                MessageBox.Show("No last file saved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewBudgetButton_Click(object sender, RoutedEventArgs e)
        {
            var regexItem = new Regex("^[a-zA-Z]*$");
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            
            if (result.ToString() != string.Empty && !String.IsNullOrWhiteSpace(FileNameTextBox.Text) && FileNameTextBox.Text.Length <= 12 &&(regexItem.IsMatch(FileNameTextBox.Text)) && openFileDlg.SelectedPath != String.Empty)
            {
                string filePath = openFileDlg.SelectedPath + @$"\{FileNameTextBox.Text}.db";
                presenter = new Presenter(this, filePath, false);
                string lastFile = presenter.GetLastFile();
                if (lastFile == filePath)
                {
                    MessageBox.Show("File duplicate already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    presenter.OpenDatabaseForm(filePath, FileNameTextBox.Text);
                }
            }
            else
            {
                MessageBox.Show("Invalid database file, please enter a file name under 12 Characters with no numbers or special characters and select an existing folder", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Opened Database File", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ColorChange(object sender, RoutedEventArgs e)
        {
            string objname = ((Button)sender).Name;
            if (objname == "Red")
            {
                MainScreen.Background = Brushes.Red;
                bigButton.Background = Brushes.DarkRed;
                Red.Background = Brushes.DarkRed;
                newFileButton.Background = Brushes.DarkRed;
                BlueViolet.Background = Brushes.DarkRed;
                LightSeaGreen.Background = Brushes.DarkRed;
                BackgroundColor.Color = (Color)ColorConverter.ConvertFromString("Red");
                SecondBackgroundColor.Color = (Color)ColorConverter.ConvertFromString("Black");
            }

            else if (objname == "BlueViolet")
            {
                MainScreen.Background = Brushes.BlueViolet;
                bigButton.Background = Brushes.DarkViolet;
                BlueViolet.Background = Brushes.DarkViolet;
                newFileButton.Background = Brushes.DarkViolet;
                Red.Background = Brushes.DarkViolet;
                LightSeaGreen.Background = Brushes.DarkViolet;
                BackgroundColor.Color = (Color)ColorConverter.ConvertFromString("Purple");
                SecondBackgroundColor.Color = (Color)ColorConverter.ConvertFromString("LightSeaGreen");
            }
            else
            {
                MainScreen.Background = Brushes.LightSeaGreen;
                bigButton.Background = Brushes.Green;
                LightSeaGreen.Background = Brushes.Green;
                newFileButton.Background = Brushes.Green;
                BlueViolet.Background = Brushes.Green;
                Red.Background = Brushes.Green;
                BackgroundColor.Color = (Color)ColorConverter.ConvertFromString("LightSeaGreen");
                SecondBackgroundColor.Color = (Color)ColorConverter.ConvertFromString("Purple");
            }
        }
    }
}
