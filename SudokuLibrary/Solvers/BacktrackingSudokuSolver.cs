using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuLibrary.Solvers
{
    public class BacktrackingSudokuSolver : ISudokuSolver
    {
        private CancellationTokenSource _cts;

        public BacktrackingSudokuSolver()
        {
        }

        public async Task SolveSudokuAsync(Sudoku sudoku)
        {
            _cts = new();
            Task task = Task.Run(() => SolveSudoku(sudoku), _cts.Token);
            await task;
        }

        public void Cancel()
        {
            if (_cts != null)
                _cts.Cancel();
        }

        private void SolveSudoku(Sudoku sudoku)
        {
            List<Field> unassignedFields = GetUnassignedFields(sudoku);
            SolveSudoku(sudoku, unassignedFields, 0);
        }

        private void SolveSudoku(Sudoku sudoku, List<Field> unassignedFields, int fieldIndex)
        {
            if (_cts.IsCancellationRequested)
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