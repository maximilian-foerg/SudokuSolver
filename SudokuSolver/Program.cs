using System;
using System.Text.RegularExpressions;

namespace SudokuSolver
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("*** SUDOKU SOLVER ***");
            do
            {
                Console.WriteLine("Please enter a Sudoku in one line or type 'exit' to end application.");
                var input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }
                HandleSudokuInput(input);
            } while(true);
        }

        static void HandleSudokuInput(string input)
        {
            if (!ValidateSudokuInput(input))
            {
                Console.WriteLine("You have entered an invalid string!");
                return;
            }
            Sudoku sudoku = new Sudoku(input);
            if (!sudoku.IsValidState())
            {
                Console.WriteLine("You have entered an invalid Sudoku!");
                return;
            }
            Console.WriteLine("You have entered the following Sudoku:");
            Console.WriteLine(sudoku);
            SimpleRecursiveSudokuSolver solver = new SimpleRecursiveSudokuSolver();
            solver.SolveSudoku(sudoku);
            Console.WriteLine("The solution:");
            Console.WriteLine(sudoku);
        }

        static Boolean ValidateSudokuInput(string sudokuString)
        {
            return Regex.IsMatch(sudokuString, "^[0-9]{81}$");
        }

    }
}
