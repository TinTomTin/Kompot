using System;

namespace Score
{
    public class Cell
    {
        public int Number;
        public int Column;
        public int Row;
        public int Square;
        public Boolean IsGuess;
        public int[] Possibilities;

        public Cell()
        {
            Number = -1;
            IsGuess = false;
        }

        public Cell(int row, int col, int number):this()
        {
            Row = row;
            Column = col;
            Number = number;
        }
    }


}
