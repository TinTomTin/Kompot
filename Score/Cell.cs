using System;

namespace Score
{
    public class Cell
    {
        public int Number;
        public int Column;
        public int Row;
        public int Sector;
        public Boolean IsGuess;
        public int[] Possibilities;

        public Cell()
        {
            Number = 0;
            IsGuess = false;
        }

        public Cell(int row, int col, int sector, int number):this()
        {
            Row = row;
            Column = col;
            Number = number;
            Sector = sector;
        }
    }


}
