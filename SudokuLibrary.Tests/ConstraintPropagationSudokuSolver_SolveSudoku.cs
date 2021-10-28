using SudokuLibrary.IO;
using SudokuLibrary.Solvers;
using Xunit;

namespace SudokuLibrary.Tests
{
    public class ConstraintPropagationSudokuSolver_SolveSudoku
    {
        [Fact]
        public async void SolveSudoku_InputIsSolvable_ReturnSolution()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002004380000003627940070030070080090310000206");
            ConstraintPropagationSudokuSolver solver = new();
            await solver.SolveSudokuAsync(sudoku);
            Assert.True(sudoku.IsSolved());
        }

        [Fact]
        public async void SolveSudoku_InputIsUnsolvable_ReturnSolution()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002044380000003627940070030070080090310000206");
            ConstraintPropagationSudokuSolver solver = new();
            await solver.SolveSudokuAsync(sudoku);
            Assert.False(sudoku.IsSolved());
        }
    }
}