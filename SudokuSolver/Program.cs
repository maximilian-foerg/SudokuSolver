using System;
using System.Text.RegularExpressions;

namespace SudokuSolver
{
    class Program
    {
        private static readonly string sudokuStringRegex = "^\\d{" + (Sudoku.Size * Sudoku.Size) + "}$";

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                Sudoku sudoku;
                switch (args[0])
                {
                    case "-f":
                        sudoku = HandleFileInput(args[1]);
                        SolveSudoku(sudoku);
                        break;
                    case "-s":
                        sudoku = HandleSudokuInput(args[1]);
                        SolveSudoku(sudoku);
                        break;
                    case "-h":
                        Console.WriteLine("Following commands are available:");
                        Console.WriteLine("-f [filename]        Solve Sudoku from specified file [filename]");
                        Console.WriteLine("-s [sudokuString]    Solve Sudoku from string");
                        Console.WriteLine("-h                   Show help (this text)");
                        break;
                    default:
                        Console.Write("Unknown command line argument. Use -h to show help.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("No command line arguments found. Use -h to show help.");
            }   
        }

        static Sudoku HandleFileInput(string filename)
        {
            Sudoku sudoku = SudokuParser.ReadSudokuFromFile(filename);
            if (!sudoku.IsValidState())
            {
                Console.WriteLine("You have entered an invalid Sudoku!");
                System.Environment.Exit(1);
            }
            return sudoku;
        }

        static Sudoku HandleSudokuInput(string input)
        {
            if (!ValidateSudokuInput(input))
            {
                Console.WriteLine("You have entered an invalid string!");
                System.Environment.Exit(1);
            }
            Sudoku sudoku = new Sudoku(input);
            if (!sudoku.IsValidState())
            {
                Console.WriteLine("You have entered an invalid Sudoku!");
                System.Environment.Exit(1);
            }
            return sudoku;
        }

        static void SolveSudoku(Sudoku sudoku)
        {
            Console.WriteLine("You have entered the following Sudoku:");
            Console.WriteLine(sudoku);
            ISudokuSolver solver = new ConstraintPropagationSudokuSolver();
            Sudoku assignment = solver.SolveSudoku(sudoku);
            if (assignment.IsSolved())
            {
                Console.WriteLine("A possible solution of this sudoku:");
                Console.WriteLine(assignment);
            }
            else
            {
                Console.WriteLine("This sudoku can't be solved!");
            }
        }

        static Boolean ValidateSudokuInput(string sudokuString)
        {
            return Regex.IsMatch(sudokuString, sudokuStringRegex);
        }

    }
}
