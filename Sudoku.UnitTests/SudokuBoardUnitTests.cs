using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using Score;

namespace Score.UnitTests2
{
    [TestClass]
    public class SudokuBoardUnitTests
    {
        private static SudokuBoard board;
        private static string defaultBoard;

        [ClassInitialize]
        public static void TestSetup(TestContext tct)
        {
            var rows = new List<String>(9)
            {
                "0,0,0,0,3,4,9,2,0",
                "9,0,3,0,0,2,0,5,1",
                "0,5,0,1,8,0,0,0,3",
                "2,0,0,0,4,7,6,0,5",
                "0,0,0,0,0,6,7,3,4",
                "4,0,0,0,5,0,2,0,0",
                "7,0,0,3,0,1,0,0,8",
                "0,0,0,0,6,0,3,4,2",
                "3,8,6,4,0,0,1,0,0"
            };

            defaultBoard = rows.Aggregate((curr, next) => curr + "," + next);
            board = new SudokuBoard(defaultBoard);
        }

        [TestMethod, Description("Check that duplicate cells can not be added.")]
        [ExpectedException(typeof(ApplicationException), "This cell already exists.")]
        public void TestAddDuplicateCell()
        {
            Cell cell2 = new Cell(0, 0, 0, 5);
            board.AddCell(cell2);
        }

        [TestMethod, Description("Positive test if column is legal that contains empty cells")]
        public void TestColumnIsLegalWithEmptyCells()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            Assert.AreEqual(true, newBoard.IsColumnLegal(1));
        }

        [TestMethod, Description("Negative test if column is legal")]
        public void TestColumnIsLegal()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            newBoard.SetCell(7, 0, 2);

            Assert.AreEqual(false, newBoard.IsColumnLegal(0));
        }

        [TestMethod, Description("Positive test if sector is legal")]
        public void TestSectorLegal()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            Assert.AreEqual(true, newBoard.IsSectorLegal(5));
        }

        [TestMethod, Description("Negative test if sector is legal")]
        public void TestSectorIllLegal()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            newBoard.SetCell(3, 7, 3);
            Assert.AreEqual(false, newBoard.IsSectorLegal(5));
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

        [TestMethod, Description("Positive test if board is legal")]
        public void TestBoardIsLegal()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            Assert.AreEqual(true, newBoard.IsLegal());
        }

        [TestMethod, Description("Negative test if board is legal")]
        public void TestBoardIsIllLegal()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            newBoard.SetCell(5, 1, 5);
            Assert.AreEqual(false, newBoard.IsLegal());
        }

        //TODO: Tests for IsSolved

        [TestMethod, Description("Test annotations")]
        public void TestAnnotations()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            List<int> expectedAnnotations = new List<int>() { 1, 6, 8 };
            CollectionAssert.AreEqual(expectedAnnotations, newBoard.GetCell(0, 0).Possibilities);
            CollectionAssert.AreEqual(new List<int> { 5 }, newBoard.GetCell(6, 6).Possibilities);
            Assert.IsTrue(newBoard.GetCell(0, 4).Possibilities.Count == 0, "Should be empty");
        }

        [TestMethod, Description("Test annotations are updated")]
        public void TestAnnotationsAfterSettingCell()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            List<int> expectedAnnotations = new List<int>() { 1, 6, 8 };
            CollectionAssert.AreEqual(expectedAnnotations, newBoard.GetCell(0, 0).Possibilities);
            newBoard.SetCell(0, 8, 6);
            CollectionAssert.DoesNotContain(newBoard.GetCell(0, 0).Possibilities, 6);
        }

        [TestMethod, Description("Test cell setting")]
        public void TestCellSetter()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            List<int> expectedAnnotations = new List<int>() { 1, 3, 6, 7, 9 };

            Assert.AreEqual(0, newBoard.GetCell(5, 1).Number, "Cell has correct initial value");
            CollectionAssert.Contains(newBoard.GetCell(5, 1).Possibilities, 3, "Cell has right value in annotations");
            newBoard.SetCell(5, 1, 3);
            Cell cellJustSet = newBoard.GetCell(5, 1);

            Assert.AreEqual(3, cellJustSet.Number, "Cell number has been set");
            CollectionAssert.DoesNotContain(cellJustSet.Possibilities, 3, "Cell has been re annotated");
            
        }

        [TestMethod, Description("Test conversion to HTML string")]
        public void TestToHTMLString()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            string htmlString = newBoard.ToHTMLString();
            File.WriteAllText("Sudoku002.html", htmlString);
            Assert.IsTrue(htmlString.Length > 10);
        }

        [TestMethod, Description("Test conversion to string")]
        public void TestToString()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            Assert.AreEqual(defaultBoard, newBoard.ToString());
        }

        [TestMethod, Description("Test Easy solve algorithm")]
        public void TestEasySolve()
        {
            SudokuBoard newBoard = new SudokuBoard(defaultBoard);
            var solvedBoard = newBoard.EasySolve(defaultBoard);

            Assert.IsTrue(solvedBoard.Item1, "Board is solved");
            Assert.IsTrue(solvedBoard.Item2.IsLegal(), "Board is in a legal state");
            Assert.AreEqual(7, solvedBoard.Item2.GetCell(8, 8).Number, "Value of last cell");
        }

        [TestMethod, Description("Test Easy solve algorithm on difficult board")]
        public void TestEasySolveOnDifficultBoard()
        {
            var rows = new List<String>(9)
            {
                "0,0,0,9,0,0,4,6,0",
                "4,0,6,7,0,0,0,3,0",
                "0,0,0,0,6,0,0,7,0",
                "0,5,0,0,0,0,2,0,4",
                "0,8,0,0,0,0,0,5,0",
                "2,0,3,0,0,0,0,9,0",
                "0,2,0,0,7,0,0,0,0",
                "0,4,0,0,0,2,5,0,8",
                "0,9,5,0,0,8,0,0,0"
            };

            var hardBoardString = rows.Aggregate((curr, next) => curr + "," + next);
            var hardBoard = new SudokuBoard(hardBoardString);

            string htmlString = hardBoard.ToHTMLString();
            File.WriteAllText("SudokuHard.html", htmlString);

            var solvedBoard = hardBoard.EasySolve(hardBoardString);

            Assert.IsFalse(solvedBoard.Item1, "Board is notsolved");
            Assert.IsTrue(solvedBoard.Item2.IsLegal(), "Board is in a legal state");
            Assert.AreEqual(2, solvedBoard.Item2.GetCell(8, 7).Number, "Value of second cell");
        }

        [TestMethod, Description("Test recursive backtracking algorithm on difficult board")]
        public void TestRecursiveSolveOnDifficultBoard()
        {
            var rows = new List<String>(9)
            {
                "0,0,0,9,0,0,4,6,0",
                "4,0,6,7,0,0,0,3,0",
                "0,0,0,0,6,0,0,7,0",
                "0,5,0,0,0,0,2,0,4",
                "0,8,0,0,0,0,0,5,0",
                "2,0,3,0,0,0,0,9,0",
                "0,2,0,0,7,0,0,0,0",
                "0,4,0,0,0,2,5,0,8",
                "0,9,5,0,0,8,0,0,0"
            };

            var hardBoardString = rows.Aggregate((curr, next) => curr + "," + next);
            var hardBoard = new SudokuBoard(hardBoardString);

            var foundSolution = hardBoard.RecursiveSolve();

            Assert.IsTrue(foundSolution, "Board is solved");
            Assert.IsTrue(hardBoard.IsLegal(), "Board is in a legal state");
            Assert.IsTrue(hardBoard.IsSolved(), "Board is solved by solved state...err, useless test actually, but anyway");

            string htmlString = hardBoard.ToHTMLString();
            File.WriteAllText("SudokuHardSolved.html", htmlString);
            //Assert.AreEqual(2, solvedBoard.Item2.GetCell(8, 7).Number, "Value of second cell");
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
