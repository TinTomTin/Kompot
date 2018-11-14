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
        private int sectorSize;

        public SudokuBoard()
        {
            cells = new List<Cell>(81);
            sectorSize = (int) Math.Sqrt(boardSize);
        }

        //TODO: throw exception if size is less than boardSize^2
        public SudokuBoard(string stringRepresentation):this()
        {
            Load(stringRepresentation);
        }

        public void Load(string boardRepresentation)
        {
            ClearCells();
            string[] cellValues = boardRepresentation.Split(',');
            int counter = 0;
            foreach(string c in cellValues)
            {
                int col = counter % boardSize;
                int row = counter / boardSize;
                int sector = DetermineSector(row, col);
                cells.Add(new Cell(row, col, sector, int.Parse(c)));
                counter++;
            }
        }

        private int DetermineSector(int row, int col)
        {
            int sectorCol = ((col * sectorSize) / (boardSize));
            int sectorRow = ((row * sectorSize) / (boardSize));
            return sectorCol + (sectorSize * sectorRow);
        }

        public int Size()
        {
            return cells.Count();
        }

        //private Cell FromString(string cellString)
        //{
        //    if(!(cellString.StartsWith('(') && cellString.EndsWith(')')))
        //    {
        //        string errorMessage = string.Format("Cell representation must start with ( and end with ), this cell started with {0} and ended with {1}.",
        //            cellString[0], cellString[cellString.Length - 1]);
        //        throw new ApplicationException(errorMessage);
        //    }

        //    string[] vals = cellString.TrimStart('(').TrimEnd(')').Split(',');
        //    if(vals.Length != 3)
        //    {
        //        throw new ApplicationException(string.Format("Cell representation must have three values {0} had {1}", cellString, vals.Length));
        //    }

        //    return new Cell(int.Parse(vals[0]), int.Parse(vals[1]), 0, int.Parse(vals[2]));
        //}

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

        public void ClearCell(int row, int col)
        {
            Cell c = GetCell(row, col);
            c.Number = 0;
            c.IsGuess = false;
        }

        //TODO: check if whole board is legal
        //TODO: to HTML string
        //TODO: get illegal row numbers
        //TODO: get illegal column numbers
        //TODO: get illegal sector numbers
        //TODO: annotation system...

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

        //public bool Tada(int rowNumber)
        //{
        //    return isPartLegal(rowNumber, )
        //}

        //private bool isPartLegal(int number, Func<Cell, int> predicate)
        //{
        //    return cells.Where(c => predicate(c) == number && c.Number > 0)
        //                .GroupBy(k => k.Number)
        //                .All(g => g.Count() == 1);
        //}

        public bool IsRowLegal(int rowNumber)
        {
            return cells.Where(c => c.Row == rowNumber && c.Number > 0)
                        .GroupBy(k => k.Number)
                        .All(g => g.Count() == 1);
        }

        public bool IsColumnLegal(int colNumber)
        {
            return cells.Where(c => c.Column == colNumber && c.Number > 0)
                        .GroupBy(k => k.Number)
                        .All(g => g.Count() == 1);
        }

        public bool IsSectorLegal(int sectNumber)
        {
            return cells.Where(c => c.Sector == sectNumber && c.Number > 0)
                        .GroupBy(k => k.Number)
                        .All(g => g.Count() == 1);
        }


    }
}
