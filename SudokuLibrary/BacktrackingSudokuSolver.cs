using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class BacktrackingSudokuSolver : ISudokuSolver
    {
        public BacktrackingSudokuSolver() {}

        public Sudoku SolveSudoku(Sudoku sudoku)
        {
            List<Field> unassignedFields = GetUnassignedFields(sudoku);
            return this.SolveSudoku(sudoku, unassignedFields, 0);
        }

        private Sudoku SolveSudoku(Sudoku sudoku, List<Field> unassignedFields, int fieldIndex)
        {
            if (fieldIndex == unassignedFields.Count)
            {
                return sudoku;
            }
            Field nextField = unassignedFields.ElementAt(fieldIndex);
            foreach (int val in Sudoku.possibleDigits)
            {
                if (sudoku.IsValidDigit(nextField, val))
                {
                    sudoku.SetCellDigit(nextField, val);
                    SolveSudoku(sudoku, unassignedFields, fieldIndex + 1);
                    if (sudoku.IsSolved())
                    {
                        return sudoku;
                    }
                    else
                    {
                        sudoku.ClearCell(nextField);
                    }
                }
            }
            return sudoku;
        }

        private static List<Field> GetUnassignedFields(Sudoku sudoku)
        {
            List<Field> unassignedFields = new();
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
