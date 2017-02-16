using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegSolitaireSolver
{
    public static class Heuristics
    {
        public static int MaxMoves(Board board)
        {
            var pegs = board.PegCount;
            var moves = board.GetPossibleMoves();
            var moveCount = moves.Count();
            if (moveCount == 0)
                return int.MaxValue;

            var pegsWithMove = board.GetGroupedPegMoves().Count;
            return pegs - 1 - (pegsWithMove / pegs);
        }

        public static int MinMoves(Board board)
        {
            var pegs = board.PegCount;
            var moves = board.GetPossibleMoves();
            var moveCount = moves.Count();
            if (moveCount == 0)
                return int.MaxValue;

            return pegs - 1 - moveCount / (moveCount + 1);
        }

        public static int MaxMovablePegs(Board board)
        {
            var pegs = board.PegCount;
            var moves = board.GetPossibleMoves();
            var moveCount = moves.Count();
            if (moveCount == 0)
                return int.MaxValue;

            return pegs - 1 - 1 / (moveCount + 1);
        }

        public static int ManhattanDistance(Board board)
        {
            // gets the distance sum of each peg relative to another

            var manhattanScore = 0;
            var numPegs = 0;

            for (int i = 0; i < board.Length; i++)
            for (int j = 0; j < board[i].Length; j++)
                if (board[i][j] == Box.Peg)
                {
                    numPegs++;
                    for (int ii = 0; ii < board.Length; ii++)
                    for (int jj = 0; jj < board[ii].Length; jj++)
                        if (board[ii][jj] == Box.Peg)
                            manhattanScore += ManhattanDistance(i, ii, j, jj);
                }

            return manhattanScore / (2 * numPegs);
        }

        public static int ManhattanDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static int ManhattanDistance(Point p1, Point p2)
        {
            return ManhattanDistance(p1.Row, p2.Row, p1.Column, p2.Column);
        }
    }
}
