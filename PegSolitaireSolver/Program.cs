using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegSolitaireSolver
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var path = args[0];
            var path = "puzzle1.board";

            var board = BoardReader.ReadBoard(path);

            Solve(board);
            
            Console.ReadLine();
        }

        private static void Solve(Board board)
        {

            if (!board.IsValid())
            {
                Console.WriteLine("Board is unsolvable");
                return;
            }

            var result = board.SolveRec(0);

            if (!result.Item1)
            {
                Console.WriteLine("Board could not be solved");
                return;
            }
            else
            {
                Console.WriteLine("Board solved");
                Console.WriteLine("Nodes visited : " + result.Item2);
            }
        }
    }
}
