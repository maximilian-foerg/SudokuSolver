using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.BusinessLogic.IO;
using SudokuSolver.BusinessLogic.Solvers;
using System.Threading.Tasks;

namespace SudokuSolver.BusinessLogic.Tests
{
    [TestClass]
    public class ConstraintPropagationSudokuSolver_SolveSudoku
    {
        [TestMethod]
        public async Task SolveSudoku_InputIsSolvable_ReturnSolution()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002004380000003627940070030070080090310000206");
            ConstraintPropagationSudokuSolver solver = new();
            await solver.SolveSudokuAsync(sudoku);
            Assert.IsTrue(sudoku.IsSolved());
        }

        [TestMethod]
        public async Task SolveSudoku_InputIsUnsolvable_ReturnSolution()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002044380000003627940070030070080090310000206");
            ConstraintPropagationSudokuSolver solver = new();
            await solver.SolveSudokuAsync(sudoku);
            Assert.IsFalse(sudoku.IsSolved());
        }
    }
}