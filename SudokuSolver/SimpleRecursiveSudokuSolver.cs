using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class SimpleRecursiveSudokuSolver : ISudokuSolver
    {
        IEnumerable<int> possibleValues = Enumerable.Range(1, Sudoku.Size);

        public SimpleRecursiveSudokuSolver() {}

        public void SolveSudoku(Sudoku sudoku)
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (sudoku.IsEmptyField(x, y))
                    {
                        foreach (int val in possibleValues)
                        {
                            if (sudoku.IsValidValue(x, y, val))
                            {
                                sudoku.SetField(x, y, val);
                                SolveSudoku(sudoku);
                                if (!sudoku.IsSolved())
                                    sudoku.ClearField(x, y);
                            }
                        }
                        return;
                    }
                }
            }
        }
    }
}
