using System.Threading.Tasks;

namespace SudokuSolver.BusinessLogic.Solvers
{
    public interface ISudokuSolver
    {
        Task SolveSudokuAsync(Sudoku sudoku);

        void Cancel();
    }
}