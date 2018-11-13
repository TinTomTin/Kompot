using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using Score;

namespace Score.UnitTests2
{
    [TestClass]
    public class SudokuBoardUnitTests
    {
        private static SudokuBoard board;

        [ClassInitialize]
        public static void TestSetup(TestContext tct)
        {
            string initialBoard = "1,2,3,4,5,6,7,8,9";
            board = new SudokuBoard(initialBoard);
        }

        [TestMethod, Description("Check that duplicate cells can not be added.")]
        [ExpectedException(typeof(ApplicationException), "This cell already exists.")]
        public void TestAddDuplicateCell()
        {
            Cell cell2 = new Cell(0, 0, 5);
            board.AddCell(cell2);
        }

        [TestMethod, Description("Positive test if row is legal")]
        public void TestRowLegal()
        {
            string legalRow = "1,2,3,4,5,6,7,8,9";
            SudokuBoard newBoard = new SudokuBoard(legalRow);
            Assert.AreEqual(true, newBoard.IsRowLegal(0));
        }

        [TestMethod, Description("Positive test if row is legal that contains empty cells")]
        public void TestRowLegalWithEmptyCells()
        {
            string legalRow = "1,2,3,-1,-1,6,7,8,9";
            SudokuBoard newBoard = new SudokuBoard(legalRow);
            Assert.AreEqual(true, newBoard.IsRowLegal(0));
        }

        [TestMethod, Description("Negative test if row is legal that contains empty cells")]
        public void TestRowIllegalWithEmptyCells()
        {
            string legalRow = "1,2,3,-1,-1,6,2,8,9";
            SudokuBoard newBoard = new SudokuBoard(legalRow);
            Assert.AreEqual(false, newBoard.IsRowLegal(0));
        }

        [TestMethod, Description("Negative test if row is legal")]
        public void TestRowIllegal()
        {
            string legalRow = "1,2,3,4,5,6,2,8,9";
            SudokuBoard newBoard = new SudokuBoard(legalRow);
            Assert.AreEqual(false, newBoard.IsRowLegal(0));
        }

        [TestMethod, Description("Load from valid string")]
        public void TestLoadFromValidString()
        {
            SudokuBoard newBoard = new SudokuBoard("2,5,8,1,3,4,6,7,9");

            Assert.AreEqual(9, newBoard.Size());
            Assert.AreEqual(2, newBoard.GetCell(0, 0).Number);
            Assert.AreEqual(5, newBoard.GetCell(0, 1).Number);
            Assert.AreEqual(8, newBoard.GetCell(0, 2).Number);
        }
    }
}
