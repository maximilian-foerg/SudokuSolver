using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Sudoku
    {
        // 600408000403900700090000503009006000002004380000003627940070030070080090310000206

        public static readonly int Size = 9;

        private int[,] sudokuArray = new int[Size, Size];

        private List<int>[] rows = new List<int>[Size];
        private List<int>[] columns = new List<int>[Size];
        private List<int>[,] regions = new List<int>[3,3];

        public Sudoku() {
            InitLists();
        }

        public Sudoku(string sudokuString)
        {
            InitLists();
            ParseSudoku(sudokuString);
        }

        private void InitLists()
        {
            for (int i = 0; i < Size; i++)
            {
                rows[i] = new List<int>();
                columns[i] = new List<int>();
                int[] coordinates = IndexToCoordinates(i, 3);
                regions[coordinates[0], coordinates[1]] = new List<int>();
            }
        }

        private void ParseSudoku(string sudokuString)
        {
            for (int i = 0; i < sudokuString.Length; i++)
            {
                int[] coordinates = IndexToCoordinates(i, Size);
                int number = int.Parse(sudokuString[i].ToString());
                SetField(coordinates[0], coordinates[1], number);
            }
        }

        private static int[] IndexToCoordinates(int index, int size)
        {
            return new int[] {index / size, index % size};
        }

        public void SetField(int x, int y, int value)
        {
            sudokuArray[x, y] = value;
            if (value != 0)
            {
                rows[x].Add(value);
                columns[y].Add(value);
                regions[x / 3, y / 3].Add(value);
            }
        }

        public int GetField(int x, int y)
        {
            return sudokuArray[x, y];
        }

        private static Boolean AllDifferent(List<int> values)
        {
            return values.Distinct().Count() == values.Count;
        }

        public Boolean IsValidState()
        {
            for (int i = 0; i < Size; i++)
            {
                int[] regionCoordinates = IndexToCoordinates(i, 3);
                if (!ValidateRow(i) ^ !ValidateColumn(i) ^ !ValidateRegion(regionCoordinates[0], regionCoordinates[1]))
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean ValidateRow(int x)
        {
            return AllDifferent(rows[x]);
        }

        public Boolean ValidateColumn(int y)
        {
            return AllDifferent(columns[y]);
        }

        public Boolean ValidateRegion(int regionX, int regionY)
        {

            return AllDifferent(regions[regionX, regionY]);
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
}
