using SudokuLibrary;
using Xunit;

namespace SudokuLibrary.Tests
{
    public class BacktrackingSudokuSolver_SolveSudoku
    {
        [Fact]
        public void SolveSudoku_InputIsSolvable_ReturnSolution()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002004380000003627940070030070080090310000206");
            Sudoku solution = SudokuParser.FromString("657438912423915768891267543739826154162754389584193627945672831276381495318549276");
            BacktrackingSudokuSolver solver = new();
            Sudoku assignment = solver.SolveSudoku(sudoku);
            Assert.Equal(assignment, solution);
        }

        [Fact]
        public void SolveSudoku_InputIsUnsolvable_ReturnSolution()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002044380000003627940070030070080090310000206");
            BacktrackingSudokuSolver solver = new();
            Sudoku assignment = solver.SolveSudoku(sudoku);
            Assert.False(assignment.IsSolved(), "This sudoku should be unsolvable...");
        }
    }
}
