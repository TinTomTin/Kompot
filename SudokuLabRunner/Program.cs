using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Score;

namespace SudokuLabRunner
{
    class Program
    {
        private static SudokuBoard board;
        private static string defaultBoard;

        public static void Setup()
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

        static void Main(string[] args)
        {
            Setup();
            string input =  String.Empty;
            int steps = 0;

            while (board.EasyStep() && input != "q")
            {
                Console.WriteLine("Iteration: {0}", steps);
                string boardState = board.ToHTMLString();
                File.WriteAllText("board.html", boardState);
                steps++;
                input = Console.ReadLine();
            }

            
        }
    }
}
