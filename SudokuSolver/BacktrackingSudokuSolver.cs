using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class BacktrackingSudokuSolver : ISudokuSolver
    {
        public BacktrackingSudokuSolver() {}

        public Sudoku SolveSudoku(Sudoku sudoku)
        {
            List<Field> unassignedFields = this.GetUnassignedFields(sudoku);
            return this.SolveSudoku(sudoku, unassignedFields, 0);
        }

        private Sudoku SolveSudoku(Sudoku sudoku, List<Field> unassignedFields, int fieldIndex)
        {
            if (fieldIndex == unassignedFields.Count())
            {
                return sudoku;
            }
            Field nextField = unassignedFields.ElementAt(fieldIndex);
            foreach (int val in Sudoku.PossibleValues)
            {
                if (sudoku.IsValidValue(nextField, val))
                {
                    sudoku.SetFieldValue(nextField, val);
                    SolveSudoku(sudoku, unassignedFields, fieldIndex + 1);
                    if (sudoku.IsSolved())
                    {
                        return sudoku;
                    }
                    else
                    {
                        sudoku.ClearField(nextField);
                    }
                }
            }
            return sudoku;
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
