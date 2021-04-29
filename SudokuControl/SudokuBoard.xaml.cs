using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace SudokuControl
{
    /// <summary>
    /// Interaction logic for SudokuBoard.xaml
    /// </summary>
    public partial class SudokuBoard : UserControl
    {
        public SudokuBoard()
        {
            InitializeComponent();
        }

        public void DisplaySudoku(Sudoku sudoku)
        {
            SudokuGrid.ItemsSource = sudoku.Board;
        }
    }
}
