using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SudokuLibrary
{
    public class Sudoku
    {
        public static readonly int Size = 9;
        public static readonly int RegionSize = 3;
        public static readonly IEnumerable<int> possibleDigits = Enumerable.Range(1, Sudoku.Size);

        private readonly List<List<Cell>> board = new();

        public Sudoku()
        {
            for (int x = 0; x < Size; x++)
            {
                board.Add(new());
                for (int y = 0; y < Size; y++)
                {
                    board[x].Add(new());
                }
            }
        }

        public Sudoku Clone()
        {
            return (Sudoku)this.MemberwiseClone();
        }

        public void SetCellDigit(int x, int y, int digit)
        {
            board[x][y].Digit = digit;
        }

        public void SetCellDigit(Field f, int digit)
        {
            this.SetCellDigit(f.X, f.Y, digit);
        }

        public int GetCellDigit(int x, int y)
        {
            return board[x][y].Digit.GetValueOrDefault();
        }

        public int? GetCellDigit(Field f)
        {
            return this.GetCellDigit(f.X, f.Y);
        }

        public void ClearCell(int x, int y)
        {
            board[x][y].Digit = null;
        }

        public void ClearCell(Field f)
        {
            this.ClearCell(f.X, f.Y);
        }

        public Cell GetCell(int x, int y)
        {
            return this.board[x][y];
        }

        public List<List<Cell>> Board
        {
            get
            {
                return board;
            }
        }

        public bool IsUnassigned(int x, int y)
        {
            return board[x][y].Digit == null;
        }

        public bool IsUnassigned(Field f)
        {
            return this.IsUnassigned(f.X, f.Y);
        }

        public bool IsValidDigit(int x, int y, int digit)
        {
            for (int i = 0; i < Size; i++)
            {
                if (board[x][i].Digit == digit ^ board[i][y].Digit == digit)
                    return false;
            }
            for (int rx = 0; rx < RegionSize; rx++)
            {
                for (int ry = 0; ry < RegionSize; ry++)
                {
                    int globalX = (x / RegionSize) * RegionSize + rx;
                    int globalY = (y / RegionSize) * RegionSize + ry;
                    if (board[globalX][globalY].Digit == digit)
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
            IEnumerable<Cell> row = board[x];
            IEnumerable<int?> digits = from cell in row where cell.Digit != null select cell.Digit;
            return Util.AllDifferent(digits);
        }

        public bool ValidateRows()
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

        public bool IsValidColumn(int y)
        {
            IEnumerable<Cell> column = from row in board select row[y];
            IEnumerable<int?> digits = from cell in column where cell.Digit != null select cell.Digit;
            return Util.AllDifferent(digits);
        }

        public bool ValidateColumns()
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

        public bool IsValidRegion(int regionX, int regionY)
        {
            int fromX = regionX * RegionSize;
            int toX = fromX + RegionSize;
            int fromY = regionY * RegionSize;
            int toY = fromY + RegionSize;
            List<Cell> region = new();
            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    region.Add(this.GetCell(x, y));
                }
            }
            IEnumerable<int?> digits = from cell in region where cell.Digit != null select cell.Digit;
            return Util.AllDifferent(digits);
        }

        public bool ValidateRegions()
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

        public bool IsCompletelyFilled()
        {
            for (int x = 0; x < Sudoku.Size; x++)
            {
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (this.IsUnassigned(x, y))
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
            StringBuilder sudokuAsString = new();
            string lineSeparator = new('-', Size * 2 + RegionSize * 2 + 1);
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
                    int? digit = board[x][y].Digit;
                    sudokuAsString.Append(digit == null ? "  " : digit + " ");
                }
                sudokuAsString.Append("|\n");
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
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (this.GetCellDigit(x, y) != other.GetCellDigit(x, y))
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

    public class Cell : INotifyPropertyChanged
    {
        private bool isEditable;
        private int? digit = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public Cell() {}

        public bool IsEditable
        {
            get
            {
                return this.isEditable;
            }
            set
            {
                if (this.isEditable != value)
                {
                    this.isEditable = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEditable"));
                }
            }
        }

        public int? Digit
        {
            get
            {
                return this.digit;
            }
            set
            {
                if (value != this.digit)
                {
                    this.digit = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Digit"));
                }
            }
        }
    }

    public class Field
    {
        private int x;
        private int y;

        public Field() {}

        public Field(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }
        
        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not Field other)
            {
                return false;
            }
            return this.GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.X * Sudoku.Size + this.Y;
        }
    }
}
