using System;
using System.Collections.Generic;
using System.Linq;

namespace PegSolitaireSolver
{
    public enum Box : byte
    {
        Wall = 0,
        Token = 1,
        Empty = 2
    }

    public enum Side : byte
    {
        Left,
        Right,
        Up,
        Down
    }

    public class Board
    {
        private readonly Box[][] grid;
        private readonly int playableTokens;
        private readonly List<Point> holes;

        public int Length => grid.Length;

        public int Tokens => playableTokens - holes.Count;

        public Box[] this[int index] => grid[index];

        public Board(Box[][] grid)
        {
            this.grid = grid;
            PlayableTokens(out playableTokens, out holes);
        }


        private Board(Box[][] grid, int playableTokens, List<Point> holes)
        {
            this.grid = grid;
            this.playableTokens = playableTokens;
            this.holes = holes.ToList();
        }

        private Board(Board board) : this(board.grid, board.playableTokens, board.holes)
        {

        }

        private void PlayableTokens(out int count, out List<Point> holes)
        {
            count = 0;
            holes = new List<Point>();
            for (int i = 0; i < grid.Length; i++)
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j].Equals(Box.Empty))
                    holes.Add(new Point(i, j));

                if (!grid[i][j].Equals(Box.Wall))
                    count++;
            }
        }

        public Box GetBox(Point startPoint, Side side, int steps, out Point endPoint)
        {
            int row = startPoint.Row;
            int column = startPoint.Column;

            switch (side)
            {
                case Side.Left:
                    column -= steps;
                    break;
                case Side.Right:
                    column += steps;
                    break;
                case Side.Up:
                    row -= steps;
                    break;
                case Side.Down:
                    row += steps;
                    break;
            }

            endPoint = new Point(row, column);

            if (row >= 0 && row < grid[0].Length && column >= 0 && column < grid.Length)
                return grid[row][column];
            return Box.Wall;
        }

        public bool IsValid()
        {
            if (Tokens < 1)
                return false;
            if (holes.Count == 0 && Tokens != 1)
                return false;

            // TODO: Check if all tokens have at least one token or an empty Box next to it

            var validBoard = new bool[grid.Length];

            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                }
            }
            return true;
        }

        public Tuple<bool, int> SolveRec(int node)
        {
            Print(node);

            if (IsSolved())
                return new Tuple<bool, int>(true, node);

            foreach (var hole in holes)
            {
                foreach (Side side in Enum.GetValues(typeof(Side)))
                {
                    Point adjPoint;
                    Point nextPoint;
                    if (GetBox(hole, side, 1, out adjPoint) == Box.Token && GetBox(hole, side, 2, out nextPoint) == Box.Token)
                    {
                        var newBoard = new Board(this);

                        newBoard[adjPoint.Row][adjPoint.Column] = Box.Empty;
                        newBoard[nextPoint.Row][nextPoint.Column] = Box.Empty;
                        newBoard[hole.Row][hole.Column] = Box.Token;

                        newBoard.holes.Remove(hole);
                        newBoard.holes.Add(adjPoint);
                        newBoard.holes.Add(nextPoint);

                        node++;

                        var result = newBoard.SolveRec(node);
                        if (result.Item1)
                            return result;

                        node = result.Item2;
                    }
                }
            }

            return new Tuple<bool, int>(false, node);
        }

        private bool IsSolved()
        {
            return playableTokens - holes.Count == 1;
        }

        public void Print(int node)
        {
            Console.Out.WriteLine("Node : " + node);
            for (var i = 0; i < Length; i++)
            {
                for (int j = 0; j < this[i].Length; j++)
                    Console.Out.Write((byte) this[i][j]);
                Console.Out.Write(Environment.NewLine);
            }
            Console.Out.WriteLine();
        }
    }
}
