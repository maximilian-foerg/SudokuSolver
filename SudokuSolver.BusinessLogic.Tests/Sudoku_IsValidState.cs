using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.BusinessLogic.IO;

namespace SudokuSolver.BusinessLogic.Tests
{
    [TestClass]
    public class Sudoku_IsValidState
    {
        [TestMethod]
        public void ValidateRows_InputIsValid_ReturnTrue()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000463900700090000503009006000002004380000003627940070030070080090310000206");
            bool result = sudoku.ValidateRows();
            Assert.IsTrue(result, "Sudoku rows should be valid");
        }
        
        [TestMethod]
        public void ValidateRows_InputIsInvalid_ReturnFalse()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002004380000003627940070030070080090310060206");
            bool result = sudoku.ValidateRows();
            Assert.IsFalse(result, "Sudoku rows should be invalid");
        }

        [TestMethod]
        public void ValidateColumns_InputIsValid_ReturnTrue()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000463900700090000503009006000002004380000003627940070030070080090310000206");
            bool result = sudoku.ValidateColumns();
            Assert.IsTrue(result, "Sudoku columns should be valid");
        }

        [TestMethod]
        public void ValidateColumns_InputIsinvalid_ReturnFalse()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000463900700090000503009006000002004380000003627940070030070080090610000206");
            bool result = sudoku.ValidateColumns();
            Assert.IsFalse(result, "Sudoku columns should be invalid");
        }

        [TestMethod]
        public void ValidateRegions_InputIsValid_ReturnTrue()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002004380000003627940070030070080090310000206");
            bool result = sudoku.ValidateRegions();
            Assert.IsTrue(result, "Sudoku regions should be valid");
        }

        [TestMethod]
        public void ValidateRegions_InputIsinvalid_ReturnFalse()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000463900700090000503009006000002004380000003627940070030070080092310000206");
            bool result = sudoku.ValidateRegions();
            Assert.IsFalse(result, "Sudoku regions should be invalid");
        }

        [TestMethod]
        public void IsValidState_InputIsValid_ReturnTrue()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002004380000003627940070030070080090310000206");
            bool result = sudoku.IsValidState();
            Assert.IsTrue(result, "Sudoku should be valid");
        }

        [TestMethod]
        public void IsValidState_InputIsInvalid_ReturnFalse()
        {
            Sudoku sudoku = SudokuParser.FromString("600408000403900700090000503009006000002044380000003627940070030070080090310000206");
            bool result = sudoku.IsValidState();
            Assert.IsFalse(result, "Sudoku should be invalid");
        }
    }
}