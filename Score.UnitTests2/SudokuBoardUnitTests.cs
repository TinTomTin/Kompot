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
            board = new SudokuBoard();
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
            Assert.AreEqual(true, board.IsRowLegal(0));
        }

        [TestMethod, Description("Negative test if row is legal")]
        public void TestRowIllegal()
        {
            board.SetCell(0, 2, 2);

            Assert.AreEqual(false, board.IsRowLegal(0));
        }

        [TestMethod, Description("Load from valid string")]
        public void TestLoadFromValidString()
        {
            string smallBoardRep = "2,5,8";
            SudokuBoard newBoard = new SudokuBoard(smallBoardRep);

            Assert.AreEqual(3, newBoard.Size());
            Assert.AreEqual(2, newBoard.GetCell(0, 0).Number);
            Assert.AreEqual(5, newBoard.GetCell(0, 1).Number);
            Assert.AreEqual(8, newBoard.GetCell(0, 2).Number);
        }
    }
}
