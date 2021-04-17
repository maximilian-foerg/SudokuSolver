using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class BacktrackingSudokuSolver : ISudokuSolver
    {
        IEnumerable<int> possibleValues = Enumerable.Range(1, Sudoku.Size);

        public BacktrackingSudokuSolver() {}

        public Boolean SolveSudoku(Sudoku sudoku)
        {
            List<Field> unassignedFields = this.GetUnassignedFields(sudoku);
            return this.SolveSudoku(sudoku, unassignedFields, 0);
        }

        private Boolean SolveSudoku(Sudoku sudoku, List<Field> unassignedFields, int fieldIndex)
        {
            if (fieldIndex == unassignedFields.Count())
            {
                return sudoku.IsSolved();
            }
            Field nextField = unassignedFields.ElementAt(fieldIndex);
            foreach (int val in possibleValues)
            {
                if (sudoku.IsValidValue(nextField, val))
                {
                    sudoku.SetFieldValue(nextField, val);
                    SolveSudoku(sudoku, unassignedFields, fieldIndex + 1);
                    if (sudoku.IsSolved())
                    {
                        return true;
                    }
                    else
                    {
                        sudoku.ClearField(nextField);
                    }
                }
            }
            return false;
        }

        private List<Field> GetUnassignedFields(Sudoku sudoku)
        {
            List<Field> unassignedFields = new List<Field>();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (sudoku.IsUnassigned(x, y))
                    {
                        unassignedFields.Add(new Field(x, y));
                    }
                }
            }
            return unassignedFields;
        }
    }
}
