using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Score
{
    public class SudokuBoard
    {
        private List<Cell> cells;
        private int boardSize = 9;

        public SudokuBoard()
        {
            cells = new List<Cell>(81);
        }

        public SudokuBoard(string stringRepresentation):this()
        {
            this.Load(stringRepresentation);
        }

        public void Load(string boardRepresentation)
        {
            ClearCells();
            string[] cellValues = boardRepresentation.Split(',');
            int counter = 0;
            //var newCells = cellStrings.Select(s => FromString(s));
            foreach(string c in cellValues)
            {
                int col = counter % boardSize;
                int row = counter / boardSize;
                cells.Add(new Cell(row, col, int.Parse(c)));
            }
        }

        public int Size()
        {
            return cells.Count();
        }

        private Cell FromString(string cellString)
        {
            if(!(cellString.StartsWith('(') && cellString.EndsWith(')')))
            {
                string errorMessage = string.Format("Cell representation must start with ( and end with ), this cell started with {0} and ended with {1}.",
                    cellString[0], cellString[cellString.Length - 1]);
                throw new ApplicationException(errorMessage);
            }

            string[] vals = cellString.TrimStart('(').TrimEnd(')').Split(',');
            if(vals.Length != 3)
            {
                throw new ApplicationException(string.Format("Cell representation must have three values {0} had {1}", cellString, vals.Length));
            }

            return new Cell(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]));
        }

        public void AddCell(Cell newCell)
        {
            if (!cells.Any(c => c.Row == newCell.Row && c.Column == newCell.Column))
            { cells.Add(newCell); }
            else
            {
                throw new ApplicationException("This cell already exists.");
            }
        }

        public void SetCell(int row, int col, int value)
        {
            cells.Single(c => c.Row == row && c.Column == col).Number = value;
        }

        public Cell GetCell(int row, int col)
        {
            return cells.Single(c => c.Row == row && c.Column == col);
        }

        public void GuessCell(int row, int col, int value)
        {
            Cell cellToSet = cells.Single(c => c.Row == row && c.Column == col);
            cellToSet.Number = value;
            cellToSet.IsGuess = true;
        }

        public void ClearCells()
        {
            cells.Clear();
        }

        public bool IsLegal()
        {
            return true;
        }

        public bool IsRowLegal(int rowNumber)
        {
            return cells.Where(c => c.Row == rowNumber && c.Number >= 0)
                        .GroupBy(k => k.Number)
                        .All(g => g.Count() == 1);
        }


    }
}
