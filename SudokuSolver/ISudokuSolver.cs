using System;

namespace SudokuSolver
{
    public interface ISudokuSolver
    {
        Sudoku SolveSudoku(Sudoku sudoku);
    }
}
