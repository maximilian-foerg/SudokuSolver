using System.Threading.Tasks;

namespace SudokuLibrary.Solvers
{
    public interface ISudokuSolver
    {
        Task SolveSudokuAsync(Sudoku sudoku);

        void Cancel();
    }
}