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

using SudokuLibrary;

namespace SudokuSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Sudoku sudoku = new();

        public MainWindow()
        {
            InitializeComponent();
            sudokuBoard.DisplaySudoku(sudoku);
        }

        private void LoadSudokuFromFile(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Sudoku files (.sdk)|*.sdk"
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                sudoku = SudokuParser.ReadSudokuFromFile(filename);
                sudokuBoard.DisplaySudoku(sudoku);
            }
        }

        private void LoadSudokuFromString(object sender, RoutedEventArgs e)
        {

        }

        private void SolveSudoku(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem) solverComboBox.SelectedItem;
            string solverName = selectedItem.Content.ToString();
            ISudokuSolver solver;
            Sudoku solution;
            switch(solverName)
            {
                case "Backtracking":
                    solver = new BacktrackingSudokuSolver();
                    solution = solver.SolveSudoku(sudoku);
                    sudokuBoard.DisplaySudoku(solution);
                    break;
                case "Constraint Propagation":
                    solver = new ConstraintPropagationSudokuSolver();
                    solution = solver.SolveSudoku(sudoku);
                    sudokuBoard.DisplaySudoku(solution);
                    break;
                default:
                    break;
            }
        }
    }
}
