using SudokuSolver.BusinessLogic;
using System.Windows.Controls;

namespace SudokuSolver.UI
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
