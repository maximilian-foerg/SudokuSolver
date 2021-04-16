using Xunit;

namespace SudokuSolver.Tests
{
    public class SimpleSudokuSolver_SolveSudoku
    {
        [Fact]
        public void SolveSudoku_InputIsSolvable_ReturnSolution()
        {
            Sudoku sudoku = new Sudoku("600408000403900700090000503009006000002004380000003627940070030070080090310000206");
            Sudoku solution = new Sudoku("657438912423915768891267543739826154162754389584193627945672831276381495318549276");
            SimpleRecursiveSudokuSolver solver = new SimpleRecursiveSudokuSolver();
            solver.SolveSudoku(sudoku);
            Assert.Equal(sudoku, solution);
        }

        [Fact]
        public void SolveSudoku_InputIsUnsolvable_ReturnSolution()
        {
            Sudoku sudoku = new Sudoku("600408000403900700090000503009006000002044380000003627940070030070080090310000206");
            Sudoku solution = new Sudoku("600408000403900700090000503009006000002044380000003627940070030070080090310000206");
            SimpleRecursiveSudokuSolver solver = new SimpleRecursiveSudokuSolver();
            solver.SolveSudoku(sudoku);
            Assert.Equal(sudoku, solution);
        }
    }
}
