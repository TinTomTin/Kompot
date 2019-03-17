using System;
using System.Linq;
using System.Collections.Generic;

namespace Score
{
    public class Cell
    {
        private int p_number;

        public int Number {
            get { return p_number; }
            set { p_number = value; Possibilities.Remove(value); }
        } 

        public int Column;
        public int Row;
        public int Sector;
        public Boolean IsGuess;
        public List<int> Possibilities;

        public Cell()
        {
            Possibilities = new List<int>();
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

        public void SetNumber(int i)
        {
            p_number = i;
        }

        public void SetPossibilities(List<int> possibilities)
        {
            Possibilities = possibilities;
        }

        
        public override string ToString()
        {
            string annotation = String.Empty;
            string cssClass = (Sector % 2) == 1 ? "oddSector" : "evenSector";
            if (Possibilities.Count > 0)
            {
                annotation = Possibilities.Select(p => p.ToString()).Aggregate((c, n) => c + "," + n);
            }
            return string.Format("<td class='{0}' title='{1}'>{2}</td>", cssClass, annotation, Number == 0 ? " " : Number.ToString());
        }
    }


}
