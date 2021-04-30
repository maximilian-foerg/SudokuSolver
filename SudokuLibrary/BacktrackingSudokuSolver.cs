using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class BacktrackingSudokuSolver : ISudokuSolver
    {
        private CancellationTokenSource cts;

        public BacktrackingSudokuSolver() {}

        public async Task SolveSudokuAsync(Sudoku sudoku)
        {
            cts = new();
            Task task = Task.Run(() => SolveSudoku(sudoku), cts.Token);
            await task;
        }

        public void Cancel()
        {
            if (cts != null)
                cts.Cancel();
        }

        private void SolveSudoku(Sudoku sudoku)
        {
            List<Field> unassignedFields = GetUnassignedFields(sudoku);
            this.SolveSudoku(sudoku, unassignedFields, 0);
        }

        private void SolveSudoku(Sudoku sudoku, List<Field> unassignedFields, int fieldIndex)
        {
            if (cts.IsCancellationRequested)
            {
                return;
            }
            else if (fieldIndex == unassignedFields.Count)
            {
                return;
            }
            Field nextField = unassignedFields.ElementAt(fieldIndex);
            foreach (int val in Sudoku.PossibleDigits)
            {
                if (sudoku.IsValidDigit(nextField, val))
                {
                    sudoku.SetCellDigit(nextField, val);
                    SolveSudoku(sudoku, unassignedFields, fieldIndex + 1);
                    if (sudoku.IsSolved())
                    {
                        return;
                    }
                    else
                    {
                        sudoku.ClearCell(nextField);
                    }
                }
            }
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
