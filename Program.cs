using System;

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
            if (!SudokuValidator.IsValidState(sudoku))
            {
                Console.WriteLine("You have entered an invalid Sudoku!");
                return;
            }
            Console.WriteLine("You have entered the following Sudoku:");
            Console.WriteLine(sudoku);
        }

        static Boolean ValidateSudokuInput(string sudokuString)
        {
            return sudokuString.Length == Sudoku.Size * Sudoku.Size;
        }

    }
}
