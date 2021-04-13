using System;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Sudoku
    {
        // 600408000403900700090000503009006000002004380000003627940070030070080090310000206

        public static readonly int Size = 9;

        private int[,] sudokuArray = new int[Size, Size];

        public Sudoku() {}

        public Sudoku(string sudokuString)
        {
            ParseSudoku(sudokuString);
        }

        private void ParseSudoku(string sudokuString)
        {
            for (int i = 0; i < sudokuString.Length; i++)
            {
                int[] coordinates = IndexToCoordinates(i);
                int number = int.Parse(sudokuString[i].ToString());
                sudokuArray[coordinates[0], coordinates[1]] = number;
            }
        }

        private static int[] IndexToCoordinates(int index)
        {
            return new int[] {index / Size, index % Size};
        }

        public void SetField(int x, int y, int value)
        {
            sudokuArray[x, y] = value;
        }

        public int GetField(int x, int y)
        {
            return sudokuArray[x, y];
        }

        public int[] GetRow(int x)
        {
            return Enumerable.Range(0, Size)
                .Select(y => sudokuArray[x, y])
                .ToArray();
        }

        public int[] GetColumn(int y)
        {
            return Enumerable.Range(0, Size)
                .Select(x => sudokuArray[x, y])
                .ToArray();
        }

        public override string ToString()
        {
            StringBuilder sudokuAsString = new StringBuilder();
            for (int x = 0; x < Size; x++)
            {
                if (x % 3 == 0)
                {
                    sudokuAsString.Append("-------------------------\n");
                }
                for (int y = 0; y < Size; y++)
                {
                    if (y % 3 == 0)
                    {
                        sudokuAsString.Append("| ");
                    }
                    int fieldValue = sudokuArray[x,y];
                    sudokuAsString.Append(fieldValue == 0 ? "  " : fieldValue + " ");
                }
                sudokuAsString.Append("|\n");
            }
            sudokuAsString.Append("-------------------------");
            return sudokuAsString.ToString();
        }
    }

    class SudokuValidator
    {
        public static Boolean IsValidState(Sudoku sudoku)
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                if (!allDifferent(sudoku.GetRow(x)))
                {
                    return false;
                }
            }
            return true;
        }

        private static Boolean allDifferent(int[] values)
        {
            return values.Distinct().Count() == values.Length;
        }
    }
}
