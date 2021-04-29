using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
using System.Windows.Shapes;

namespace SudokuSolver
{
    /// <summary>
    /// Interaction logic for EnterSudokuStringDialog.xaml
    /// </summary>
    public partial class EnterSudokuStringDialog : Window
    {
        private static readonly string sudokuStringRegex = "^[0-9]{81}$";

        public EnterSudokuStringDialog()
        {
            InitializeComponent();
        }

        public string SudokuString { get; set; }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            string inputString = sudokuStringTextBox.Text;
            if(!Regex.IsMatch(inputString, sudokuStringRegex))
            {
                MessageBox.Show("This string is invalid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SudokuString = inputString;
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
