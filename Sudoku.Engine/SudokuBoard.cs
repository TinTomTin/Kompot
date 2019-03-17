using System;
using System.IO;
using System.Linq;
using System.Reflection;
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

        //TODO: throw exception if size not boardSize^2
        public SudokuBoard(string stringRepresentation):this()
        {
            Load(stringRepresentation);
            AnnotateCells();
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

        private List<int> DeterminePossibilities(int row, int col)
        {
            Cell cell = GetCell(row, col);
            var rowNumbers = cells.Where(c => c.Row == row).Select(c => c.Number);
            var colNumbers = cells.Where(c => c.Column == col).Select(c => c.Number);
            var secNumbers = cells.Where(c => c.Sector == cell.Sector).Select(c => c.Number);
            var usedNumbers = rowNumbers.Concat(colNumbers).Concat(secNumbers).Distinct();
            var allNumbers = Enumerable.Range(1, boardSize);
            return allNumbers.Except(usedNumbers).ToList();

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
            AnnotateCells();
        }

        public void ClearCell(int row, int col)
        {
            Cell c = GetCell(row, col);
            c.Number = 0;
            c.IsGuess = false;
        }

        //TODO: this needs to be a seperate class that does this.
        public (bool, SudokuBoard) EasySolve(string inputBoard)
        {
            SudokuBoard board = new SudokuBoard(inputBoard);

            while(board.cells.FirstOrDefault(f => f.Possibilities.Count == 1) != null)
            {
                Cell c = board.cells.FirstOrDefault(f => f.Possibilities.Count == 1);
                board.SetCell(c.Row, c.Column, c.Possibilities.First());
            }

            return (board.IsSolved(), board);
        }

        //TODO: what if you backtrack? You have to put back the number that was guessed into annotations
        //TODO: what about recording the result of easy solve?
        public bool RecursiveSolve()
        {
            var solvedBoard = EasySolve(this.ToString());
            if (solvedBoard.Item1)
            {
                Load(solvedBoard.Item2.ToString());
                return true;
            } else
            {
                Cell c = cells.FirstOrDefault(k => k.Number == 0);

                if (c != null)
                {
                    foreach (int i in c.Possibilities)
                    {
                        c.SetNumber(i);
                        if (IsLegal())
                        {
                            RecursiveSolve();
                        } else
                        {
                            c.SetNumber(0);
                        }
                    }
                    if (!IsSolved()) c.SetNumber(0);
                }
                return IsSolved();
            }

            // if (cell == boardSize * boardSize) return IsSolved();

            

            //if (cells[cell].Number == 0)
            //{
            //    if (guessDepth >= cells[cell].Possibilities.Count)
            //    {
            //        return RecursiveSolve(cell++, 0);
            //    }
            //    else
            //    {
            //        var guess = cells[cell].Possibilities[guessDepth];
            //        cells[cell].Number = guess;
            //        AnnotateCells();
            //        return RecursiveSolve(cell, ++guessDepth);
            //    }
            //}
            //else
            //{
            //    return RecursiveSolve(++cell, 0);
            //}
        }

        //TODO: get illegal row numbers
        //TODO: get illegal column numbers
        //TODO: get illegal sector numbers
        
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
            return Enumerable.Range(0, boardSize).All(i => IsRowLegal(i) && IsColumnLegal(i) && IsSectorLegal(i));
        }

        public bool IsSolved()
        {
            return cells.All(c => c.Number != 0);
        }

        private void AnnotateCells()
        {
            cells.Where(c => c.Number == 0).ToList().ForEach(c => c.SetPossibilities(DeterminePossibilities(c.Row, c.Column)));
        }

        private bool isPartLegal(int number, Func<Cell, int> partGetter)
        {
            return cells.Where(c => partGetter(c) == number && c.Number > 0)
                        .GroupBy(k => k.Number)
                        .All(g => g.Count() == 1);
        }

        public bool IsRowLegal(int rowNumber)
        {
            return isPartLegal(rowNumber, a => a.Row);
        }

        public bool IsColumnLegal(int colNumber)
        {
            return isPartLegal(colNumber, a => a.Column);
        }

        public bool IsSectorLegal(int sectNumber)
        {
            return isPartLegal(sectNumber, a => a.Sector);
        }

        private string RowToString(int row)
        {
            return string.Format("<tr>{0}</tr>", cells.Where(r => r.Row == row).Select(c => c.ToString()).Aggregate((c, n) => c + n));
        }

        public override string ToString()
        {
            return cells.Select(s => s.Number.ToString()).Aggregate((c, n) => c + "," + n);
        }

        public string ToHTMLString()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceTemplateName = "Score.template.html";
            var resourceCssName = "Score.Sudoku.css";
            string templateString = String.Empty, cssString = String.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(resourceTemplateName))
            using (StreamReader reader = new StreamReader(stream))
            {
                templateString = reader.ReadToEnd();
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceCssName))
            using (StreamReader reader = new StreamReader(stream))
            {
                cssString = reader.ReadToEnd();
            }

            string cellsAsString = Enumerable.Range(0, boardSize).Select(i => RowToString(i)).Aggregate((c, n) => c + n);
            return string.Format(templateString, cssString, cellsAsString);
        }


    }
}
