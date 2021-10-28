using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SudokuLibrary.IO
{
    public class SudokuParser
    {
        // f.e. "^[1-9\.]{9}$" for Sudoku.Size = 9
        private static readonly Regex s_lineRegex = new("^[1-" + Sudoku.Size + "\\.]{" + Sudoku.Size + "}$", RegexOptions.Compiled);

        public static Sudoku FromFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            if (lines.Length != Sudoku.Size)
            {
                throw new InvalidSudokuFormatException($"Wrong number of rows ({lines.Length}, should be {Sudoku.Size})");
            }
            Sudoku sudoku = new();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                string line = lines[x];
                if (line.Length != Sudoku.Size)
                {
                    throw new InvalidSudokuFormatException($"Wrong number of columns on line {x + 1} ({line.Length}, should be {Sudoku.Size})");
                }
                if (!CheckLineFormat(line))
                {
                    throw new InvalidSudokuFormatException($"Line {x + 1} is of invalid form");
                }
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    char c = line[y];
                    if (!c.Equals('.'))
                    {
                        sudoku.SetCellDigit(x, y, (int)char.GetNumericValue(c));
                    }
                }
            }
            return sudoku;
        }

        private static bool CheckLineFormat(string line)
        {
            return s_lineRegex.IsMatch(line);
        }

        public static async Task ToFile(Sudoku sudoku, string filename)
        {
            string[] lines = new string[Sudoku.Size];
            for (int x = 0; x < Sudoku.Size; x++)
            {
                StringBuilder sb = new();
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (sudoku.IsUnassigned(x, y))
                    {
                        _ = sb.Append('.');
                    }
                    else
                    {
                        _ = sb.Append(sudoku.GetCellDigit(x, y));
                    }
                }
                lines[x] = sb.ToString();
            }
            await File.WriteAllLinesAsync(filename, lines);
        }

        public static Sudoku FromString(string sudokuString)
        {
            Sudoku sudoku = new();
            for (int i = 0; i < sudokuString.Length; i++)
            {
                Field f = Util.IndexToField(i, Sudoku.Size);
                int number = int.Parse(sudokuString[i].ToString(), System.Globalization.NumberStyles.None);
                if (number != 0)
                    sudoku.SetCellDigit(f.X, f.Y, number);
            }
            return sudoku;
        }
    }

    public class InvalidSudokuFormatException : Exception
    {
        public InvalidSudokuFormatException(string msg) : base("The Sudoku has an invalid form: " + msg)
        {
        }
    }
}