using System.Threading.Tasks;

namespace SudokuLibrary
{
    public interface ISudokuSolver
    {
        Task SolveSudokuAsync(Sudoku sudoku);

        void Cancel();
    }
}
