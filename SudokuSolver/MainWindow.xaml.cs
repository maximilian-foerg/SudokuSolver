﻿using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
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
            var dialog = new OpenFileDialog
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

        private void ClearSudoku(object sender, RoutedEventArgs e)
        {
            sudoku = new();
            sudokuBoard.DisplaySudoku(sudoku);
        }

        private void SolveSudoku(object sender, RoutedEventArgs e)
        {
            if (!sudoku.IsValidState())
            {
                MessageBox.Show("This sudoku is not in an valid state.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
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
