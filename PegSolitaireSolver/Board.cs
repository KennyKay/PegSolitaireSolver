using System;
using System.Collections.Generic;
using System.Linq;

namespace PegSolitaireSolver
{
    public enum Box : byte
    {
        Wall = 0,
        Peg = 1,
        Hole = 2
    }

    public enum Side : byte
    {
        Left,
        Up,
        Right,
        Down
    }

    public class Board
    {
        private readonly Box[][] grid;
        private readonly int boxCount;
        private readonly List<Point> holes;

        public int Length => grid.Length;

        public int PegCount => boxCount - holes.Count;

        public Box[] this[int index] => grid[index];

        public Board(Box[][] grid)
        {
            this.grid = grid;
            PlayableBoxes(out boxCount, out holes);
        }

        private Board(Box[][] grid, int boxCount, List<Point> holes)
        {
            this.grid = grid;
            this.boxCount = boxCount;
            this.holes = holes;
        }

        private void PlayableBoxes(out int count, out List<Point> holes)
        {
            count = 0;
            holes = new List<Point>();
            for (int i = 0; i < grid.Length; i++)
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j].Equals(Box.Hole))
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
            if (PegCount < 1)
                return false;
            if (holes.Count == 0 && PegCount != 1)
                return false;
            return true;
        }

        public Tuple<bool, int> SolveRec(int node = 1)
        {
            Print(node);

            if (IsSolved())
                return new Tuple<bool, int>(true, node);

            var moves = GetPossibleMoves();

            foreach (var move in moves)
            {
                var newBoard = this.Clone();

                var to = move.To;
                var jumped = move.Jumped;
                var from = move.From;

                newBoard[jumped.Row][jumped.Column] = Box.Hole;
                newBoard[from.Row][from.Column] = Box.Hole;
                newBoard[to.Row][to.Column] = Box.Peg;

                newBoard.holes.Remove(to);
                newBoard.holes.Add(jumped);
                newBoard.holes.Add(from);

                node++;

                var result = newBoard.SolveRec(node);
                node = result.Item2;

                if (result.Item1)
                    return result;
            }

            return new Tuple<bool, int>(false, node);
        }

        private IEnumerable<Move> GetPossibleMoves()
        {
            foreach (var hole in holes)
            foreach (Side side in Enum.GetValues(typeof(Side)))
            {
                Point jumped;
                Point from;
                if (GetBox(hole, side, 2, out from) == Box.Peg && GetBox(hole, side, 1, out jumped) == Box.Peg)
                    yield return new Move(from, jumped, hole);
            }
        }

        private Board Clone()
        {
            var newgrid = new Box[Length][];
            Enumerable.Range(0, Length).AsParallel().ForAll(i =>
            {
                newgrid[i] = new Box[grid[i].Length];
                Array.Copy(grid[i], newgrid[i], grid[i].Length);
            });
            return new Board(newgrid, boxCount, holes.ToList());
        }

        private bool IsSolved()
        {
            return boxCount - holes.Count == 1;
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
