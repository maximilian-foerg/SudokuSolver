using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class SudokuParser
    {
        private static readonly string lineRegex = "^[1-" + Sudoku.Size + "\\.]{" + Sudoku.Size + "}$"; // f.e. "^[1-9\.]{9}$" for Sudoku.Size = 9

        public static Sudoku FromFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            if (lines.Length != Sudoku.Size)
            {
                throw new InvalidSudokuFormatException(String.Format("Wrong number of rows ({0}, should be {1})", lines.Length, Sudoku.Size));
            }
            Sudoku sudoku = new();
            for (int x = 0; x < Sudoku.Size; x++)
            {
                String line = lines[x];
                if (line.Length != Sudoku.Size)
                {
                    throw new InvalidSudokuFormatException(String.Format("Wrong number of columns on line {0} ({1}, should be {2})", x + 1, line.Length, Sudoku.Size));
                }
                if (!CheckLineFormat(line))
                {
                    throw new InvalidSudokuFormatException(String.Format("Line {0} is of invalid form", x + 1));
                }
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    char c = line[y];
                    if (!c.Equals('.'))
                    {
                        sudoku.SetCellDigit(x, y, (int) Char.GetNumericValue(c));
                    }
                }
            }
            return sudoku;
        }

        private static bool CheckLineFormat(string line)
        {
            return Regex.IsMatch(line, lineRegex);
        }

        public static async Task ToFile(Sudoku sudoku, String filename)
        {
            string[] lines = new string[Sudoku.Size];
            for (int x = 0; x < Sudoku.Size; x++)
            {
                StringBuilder sb = new();
                for (int y = 0; y < Sudoku.Size; y++)
                {
                    if (sudoku.IsUnassigned(x, y))
                    {
                        sb.Append('.');
                    }
                    else
                    {
                        sb.Append(sudoku.GetCellDigit(x, y));
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
                int number = int.Parse(sudokuString[i].ToString());
                if (number != 0)
                    sudoku.SetCellDigit(f.X, f.Y, number);
            }
            return sudoku;
        }
    }

    [System.Serializable]
    public class InvalidSudokuFormatException : System.Exception
    {
        public InvalidSudokuFormatException(String msg) : base ("The Sudoku has an invalid form: " + msg) {}
    }
}
