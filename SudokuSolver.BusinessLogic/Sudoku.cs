using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SudokuSolver.BusinessLogic
{
    public class Sudoku
    {
        private static readonly int s_size = 9;
        private static readonly int s_regionSize = 3;
        private static readonly IEnumerable<int> s_possibleDigits = Enumerable.Range(1, Sudoku.s_size);

        private readonly List<List<Cell>> _board = new();

        public Sudoku()
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                _board.Add(new());
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    _board[x].Add(new());
                }
            }
        }

        public Sudoku Clone()
        {
            return (Sudoku)this.MemberwiseClone();
        }

        public static int Size
        {
            get { return s_size; }
        }

        public static int RegionSize
        {
            get { return s_regionSize; }
        }

        public static IEnumerable<int> PossibleDigits
        {
            get { return s_possibleDigits; }
        }

        public void SetCellDigit(int x, int y, int digit)
        {
            _board[x][y].Digit = digit;
        }

        public void SetCellDigit(Field f, int digit)
        {
            SetCellDigit(f.X, f.Y, digit);
        }

        public int GetCellDigit(int x, int y)
        {
            return _board[x][y].Digit.GetValueOrDefault();
        }

        public int? GetCellDigit(Field f)
        {
            return GetCellDigit(f.X, f.Y);
        }

        public void ClearCell(int x, int y)
        {
            _board[x][y].Digit = null;
        }

        public void ClearCell(Field f)
        {
            ClearCell(f.X, f.Y);
        }

        public Cell GetCell(int x, int y)
        {
            return _board[x][y];
        }

        public List<List<Cell>> Board
        {
            get
            {
                return _board;
            }
        }

        public bool IsUnassigned(int x, int y)
        {
            return _board[x][y].Digit == null;
        }

        public bool IsUnassigned(Field f)
        {
            return IsUnassigned(f.X, f.Y);
        }

        public bool IsEmpty()
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (!IsUnassigned(x, y)) return false;
                }
            }
            return true;
        }

        public bool IsValidDigit(int x, int y, int digit)
        {
            for (int i = 0; i < Sudoku.Size; i++)
            {
                if (_board[x][i].Digit == digit ^ _board[i][y].Digit == digit)
                    return false;
            }
            for (int rx = 0; rx < Sudoku.RegionSize; rx++)
            {
                for (int ry = 0; ry < Sudoku.RegionSize; ry++)
                {
                    int globalX = (x / Sudoku.RegionSize) * Sudoku.RegionSize + rx;
                    int globalY = (y / Sudoku.RegionSize) * Sudoku.RegionSize + ry;
                    if (_board[globalX][globalY].Digit == digit)
                        return false;
                }
            }
            return true;
        }

        public bool IsValidDigit(Field f, int digit)
        {
            return this.IsValidDigit(f.X, f.Y, digit);
        }

        public bool IsValidState()
        {
            return ValidateRows() & ValidateColumns() & ValidateRegions();
        }

        public bool IsValidRow(int x)
        {
            IEnumerable<Cell> row = _board[x];
            IEnumerable<int?> digits = from cell in row where cell.Digit != null select cell.Digit;
            return Util.AllDifferent(digits);
        }

        public bool ValidateRows()
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                if (!IsValidRow(x))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidColumn(int y)
        {
            IEnumerable<Cell> column = from row in _board select row[y];
            IEnumerable<int?> digits = from cell in column where cell.Digit != null select cell.Digit;
            return Util.AllDifferent(digits);
        }

        public bool ValidateColumns()
        {
            for (int y = 0; y < Sudoku.Size; y++)
            {
                if (!IsValidColumn(y))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidRegion(int regionX, int regionY)
        {
            int fromX = regionX * Sudoku.RegionSize;
            int toX = fromX + Sudoku.RegionSize;
            int fromY = regionY * Sudoku.RegionSize;
            int toY = fromY + Sudoku.RegionSize;
            List<Cell> region = new();
            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    region.Add(GetCell(x, y));
                }
            }
            IEnumerable<int?> digits = from cell in region where cell.Digit != null select cell.Digit;
            return Util.AllDifferent(digits);
        }

        public bool ValidateRegions()
        {
            for (int x = 0; x < Sudoku.RegionSize; x++)
            {
                for (int y = 0; y < Sudoku.RegionSize; y++)
                {
                    if (!IsValidRegion(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsCompletelyFilled()
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (IsUnassigned(x, y))
                        return false;
                }
            }
            return true;
        }

        public bool IsSolved()
        {
            return IsCompletelyFilled() & IsValidState();
        }

        public override string ToString()
        {
            StringBuilder sudokuAsString = new();
            string lineSeparator = new('-', Sudoku.Size * 2 + Sudoku.RegionSize * 2 + 1);
            for (int x = 0; x < Sudoku.Size; x++)
            {
                if (x % Sudoku.RegionSize == 0)
                {
                    sudokuAsString.Append(lineSeparator + Environment.NewLine);
                }
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (y % Sudoku.RegionSize == 0)
                    {
                        sudokuAsString.Append("| ");
                    }
                    int? digit = _board[x][y].Digit;
                    sudokuAsString.Append(digit is null ? "  " : digit + " ");
                }
                sudokuAsString.Append($"|{Environment.NewLine}");
            }
            sudokuAsString.Append(lineSeparator);
            return sudokuAsString.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is not Sudoku other)
            {
                return false;
            }
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (GetCellDigit(x, y) != other.GetCellDigit(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return _board.GetHashCode();
        }
    }

    /// <summary>Represents a cell of a sudoku.</summary>
    public class Cell : INotifyPropertyChanged
    {
        private static readonly IEnumerable<int> s_possibleDigits = Sudoku.PossibleDigits;

        private int? _digit = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public Cell()
        {
        }

        public int? Digit
        {
            get
            {
                return _digit;
            }
            set
            {
                if (value != _digit)
                {
                    _digit = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Digit"));
                }
            }
        }

        public static IEnumerable<int> PossibleDigits
        {
            get
            {
                return s_possibleDigits;
            }
        }
    }

    /// <summary>A immutable wrapper class for x/y coordinates.</summary>
    public class Field
    {
        private Field()
        {
        }

        public Field(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X
        {
            get;
        }

        public int Y
        {
            get;
        }

        public override bool Equals(object obj)
        {
            if (obj is Field other)
                return GetHashCode() == other.GetHashCode();
            return false;
        }

        public override int GetHashCode()
        {
            return X * Sudoku.Size + Y;
        }
    }
}