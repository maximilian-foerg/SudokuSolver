using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public class Sudoku
    {
        public static readonly int Size = 9;
        public static readonly int RegionSize = 3;

        private static readonly int Empty = 0;

        private int[,] board = new int[Size, Size];

        public Sudoku() {}

        public Sudoku(string sudokuString)
        {
            ParseSudoku(sudokuString);
        }

        private void ParseSudoku(string sudokuString)
        {
            for (int i = 0; i < sudokuString.Length; i++)
            {
                int[] coordinates = Util.IndexToCoordinates(i, Size);
                int number = int.Parse(sudokuString[i].ToString());
                SetField(coordinates[0], coordinates[1], number);
            }
        }

        public void SetField(int x, int y, int value)
        {
            board[x, y] = value;
        }

        public int GetField(int x, int y)
        {
            return board[x, y];
        }

        public void ClearField(int x, int y)
        {
            board[x, y] = Empty;
        }

        public Boolean IsEmptyField(int x, int y)
        {
            return board[x,y] == Empty;
        }

        public Boolean IsValidValue(int x, int y, int value)
        {
            for (int i = 0; i < Size; i++)
            {
                if (board[x, i] == value ^ board[i, y] == value)
                    return false;
            }
            for (int rx = 0; rx < RegionSize; rx++)
            {
                for (int ry = 0; ry < RegionSize; ry++)
                {
                    if (board[(x / RegionSize) * RegionSize + rx, (y / RegionSize) * RegionSize + ry] == value)
                        return false;
                }
            }
            return true;
        }

        public Boolean IsValidState()
        {
            return ValidateRows() & ValidateColumns() & ValidateRegions();
        }

        public Boolean IsValidRow(int x)
        {
            IEnumerable<int> row = Util.SliceRow(board, x);
            IEnumerable<int> values = from val in row where val != 0 select val;
            return Util.AllDifferent(values);
        }

        public Boolean ValidateRows()
        {
            for (int x = 0; x < Size; x++)
            {
                if (!IsValidRow(x))
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean IsValidColumn(int y)
        {
            IEnumerable<int> column = Util.SliceColumn(board, y);
            IEnumerable<int> values = from val in column where val != 0 select val;
            return Util.AllDifferent(values);
        }

        public Boolean ValidateColumns()
        {
            for (int y = 0; y < Size; y++)
            {
                if (!IsValidColumn(y))
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean IsValidRegion(int regionX, int regionY)
        {
            int fromX = regionX * RegionSize;
            int toX = fromX + RegionSize;
            int fromY = regionY * RegionSize;
            int toY = fromY + RegionSize;
            IEnumerable<int> region = Util.SliceRegion(board, fromX, toX, fromY, toY);
            IEnumerable<int> values = from val in region where val != 0 select val;
            return Util.AllDifferent(values);
        }

        public Boolean ValidateRegions()
        {
            for (int x = 0; x < RegionSize; x++)
            {
                for (int y = 0; y < RegionSize; y++)
                {
                    if (!IsValidRegion(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Boolean IsCompletelyFilled()
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (this.IsEmptyField(x, y))
                        return false;
                }
            }
            return true;
        }

        public Boolean IsSolved()
        {
            return this.IsCompletelyFilled() & this.IsValidState();
        }

        public override string ToString()
        {
            StringBuilder sudokuAsString = new StringBuilder();
            String lineSeparator = new String('-', Size * 2 + RegionSize * 2 + 1);
            for (int x = 0; x < Size; x++)
            {
                if (x % RegionSize == 0)
                {
                    sudokuAsString.Append(lineSeparator + "\n");
                }
                for (int y = 0; y < Size; y++)
                {
                    if (y % RegionSize == 0)
                    {
                        sudokuAsString.Append("| ");
                    }
                    int fieldValue = board[x,y];
                    sudokuAsString.Append(fieldValue == Empty ? "  " : fieldValue + " ");
                }
                sudokuAsString.Append("|\n");
            }
            sudokuAsString.Append(lineSeparator);
            return sudokuAsString.ToString();
        }

        public override bool Equals(System.Object obj)
        {
            Sudoku other = obj as Sudoku;
            if (other == null)
            {
                return false;
            }
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (this.GetField(x, y) != other.GetField(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return board.GetHashCode();
        }
    }
}
