using Xunit;

namespace SudokuSolver.Tests
{
    public class Sudoku_isValidState
    {
        [Fact]
        public void ValidateRows_InputIsValid_ReturnTrue()
        {
            Sudoku sudoku = new Sudoku("600408000463900700090000503009006000002004380000003627940070030070080090310000206");
            bool result = sudoku.ValidateRows();
            Assert.True(result, "Sudoku rows should be valid");
        }

        [Fact]
        public void ValidateRows_InputIsInvalid_ReturnFalse()
        {
            Sudoku sudoku = new Sudoku("600408000403900700090000503009006000002004380000003627940070030070080090310060206");
            bool result = sudoku.ValidateRows();
            Assert.False(result, "Sudoku rows should be invalid");
        }

        [Fact]
        public void ValidateColumns_InputIsValid_ReturnTrue()
        {
            Sudoku sudoku = new Sudoku("600408000463900700090000503009006000002004380000003627940070030070080090310000206");
            bool result = sudoku.ValidateColumns();
            Assert.True(result, "Sudoku columns should be valid");
        }

        [Fact]
        public void ValidateColumns_InputIsinvalid_ReturnFalse()
        {
            Sudoku sudoku = new Sudoku("600408000463900700090000503009006000002004380000003627940070030070080090610000206");
            bool result = sudoku.ValidateColumns();
            Assert.False(result, "Sudoku columns should be invalid");
        }

        [Fact]
        public void ValidateRegions_InputIsValid_ReturnTrue()
        {
            Sudoku sudoku = new Sudoku("600408000403900700090000503009006000002004380000003627940070030070080090310000206");
            bool result = sudoku.ValidateRegions();
            Assert.True(result, "Sudoku regions should be valid");
        }

        [Fact]
        public void ValidateRegions_InputIsinvalid_ReturnFalse()
        {
            Sudoku sudoku = new Sudoku("600408000463900700090000503009006000002004380000003627940070030070080092310000206");
            bool result = sudoku.ValidateRegions();
            Assert.False(result, "Sudoku regions should be invalid");
        }

        [Fact]
        public void IsValidState_InputIsValid_ReturnTrue()
        {
            Sudoku sudoku = new Sudoku("600408000403900700090000503009006000002004380000003627940070030070080090310000206");
            bool result = sudoku.IsValidState();
            Assert.True(result, "Sudoku should be valid");
        }

        [Fact]
        public void IsValidState_InputIsInvalid_ReturnFalse()
        {
            Sudoku sudoku = new Sudoku("600408000403900700090000503009006000002044380000003627940070030070080090310000206");
            bool result = sudoku.IsValidState();
            Assert.False(result, "Sudoku should be invalid");
        }
    }
}
