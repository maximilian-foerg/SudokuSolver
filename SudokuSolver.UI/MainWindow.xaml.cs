using Microsoft.Win32;
using SudokuSolver.BusinessLogic;
using SudokuSolver.BusinessLogic.IO;
using SudokuSolver.BusinessLogic.Solvers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SudokuSolver.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Sudoku sudoku = new();
        private ISudokuSolver runningSolver;

        public MainWindow()
        {
            InitializeComponent();
            sudokuBoard.DisplaySudoku(sudoku);
        }

        private void NewSudoku(object sender, RoutedEventArgs e)
        {
            sudoku = new();
            sudokuBoard.DisplaySudoku(sudoku);
        }

        private void LoadSudokuFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                DefaultExt = ".sdk",
                Filter = "Sudoku files (.sdk)|*.sdk"
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                try
                {
                    sudoku = SudokuParser.FromFile(filename);
                }
                catch (InvalidSudokuFormatException except)
                {
                    MessageBox.Show(except.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                sudokuBoard.DisplaySudoku(sudoku);
            }
        }

        private void LoadSudokuFromString(object sender, RoutedEventArgs e)
        {
            var dialog = new EnterSudokuStringDialog
            {
                Owner = this
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string sudokuString = dialog.SudokuString;
                sudoku = SudokuParser.FromString(sudokuString);
                sudokuBoard.DisplaySudoku(sudoku);
            }
        }

        private async void SaveSudoku(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new()
            {
                Filter = "Sudoku files (*.sdk)|*.sdk|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".sdk",
                RestoreDirectory = true
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string fileName;
                if ((fileName = dialog.FileName) != null)
                {
                    await SudokuParser.ToFile(sudoku, fileName);
                }
            }
        }

        private void SolveSudoku(object sender, RoutedEventArgs e)
        {
            if (runningSolver != null)
            {
                runningSolver.Cancel();
                return;
            }
            if (sudoku.IsEmpty())
            {
                MessageBox.Show("This sudoku is empty.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!sudoku.IsValidState())
            {
                MessageBox.Show("This sudoku is not in a valid state.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (sudoku.IsSolved())
            {
                MessageBox.Show("This Sudoku is already solved.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ComboBoxItem selectedItem = (ComboBoxItem)solverComboBox.SelectedItem;
            string solverName = selectedItem.Content.ToString();
            switch (solverName)
            {
                case "Backtracking":
                    SolveSudoku(new BacktrackingSudokuSolver());
                    break;

                case "Constraint Propagation":
                    SolveSudoku(new ConstraintPropagationSudokuSolver());
                    break;

                default:
                    break;
            }
        }

        private async void SolveSudoku(ISudokuSolver solver)
        {
            solveButton.Content = "Cancel";
            EnableMenuItems(false);
            runningSolver = solver;
            await Task.Run(() => solver.SolveSudokuAsync(sudoku));
            runningSolver = null;
            EnableMenuItems(true);
            solveButton.Content = "Solve";
            if (sudoku.IsSolved())
            {
                MessageBox.Show("The Sudoku was solved successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EnableMenuItems(bool enabled)
        {
            menuItemNew.IsEnabled = enabled;
            menuItemLoad.IsEnabled = enabled;
            menuItemLoadFromString.IsEnabled = enabled;
            menuItemSave.IsEnabled = enabled;
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            if (runningSolver != null)
                runningSolver.Cancel();
            Application.Current.Shutdown();
        }

        private void ShowInstructions(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You can either set a cell's digit with a right click on a cell, " +
                "or load a Sudoku from a file or from a string using the menu. After adding some " +
                "digits to the Sudoku, choose a solver and click 'Solve', to see the Sudoku " +
                "being solved automatically. If you'd like to stop the solution search, " +
                "click 'Cancel'.", "Instructions", MessageBoxButton.OK);
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Sudoku solver written in C# was created by Maximilian Förg " +
                "as part of the Open Doors Day at baramundi software AG on 04/13/21.",
                "About", MessageBoxButton.OK);
        }
    }
}