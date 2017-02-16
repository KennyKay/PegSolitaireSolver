using System;
using System.Collections;
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
    public enum Symmetry : byte
    {
        None,
        Horizontal,
        Vertical,
        HorizontalAndVertical,
        Complete
    }

    public class Board
    {
        private readonly Box[][] grid;
        private readonly int boxCount;
        private readonly List<Point> holes;
        private readonly Symmetry symmetry;

        public int Length => grid.Length;

        public int PegCount => boxCount - holes.Count;

        public Box[] this[int index] => grid[index];

        public Board(Box[][] grid)
        {
            this.grid = grid;
            BoardConfig(out boxCount, out holes, out symmetry);
        }

        private Board(Box[][] grid, int boxCount, List<Point> holes, Symmetry symmetry)
        {
            this.grid = grid;
            this.boxCount = boxCount;
            this.holes = holes;
            this.symmetry = symmetry;
        }

        private void BoardConfig(out int count, out List<Point> holes, out Symmetry symmetry)
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

            // Check Symmetry

            if (IsCompleteSymmetry())
            {
                symmetry = Symmetry.Complete;
            }
            else
            {
                var horSym = IsHorizontalSymmetry();
                var verSym = IsVerticalSymmetry();
                if (horSym && verSym)
                {
                    symmetry = Symmetry.HorizontalAndVertical;
                }
                else if (horSym)
                {
                    symmetry = Symmetry.Horizontal;
                }
                else if (verSym)
                {
                    symmetry = Symmetry.Vertical;
                }
                else
                {
                    symmetry = Symmetry.None;
                }
            }
        }

        private bool IsVerticalSymmetry()
        {
            var maxLength = grid.Length - 1;
            for (int i = 0; i < maxLength; i++)
                for (int j = 0; j < maxLength; j++)
                {
                    if (this[i][j] == Box.Wall && this[i][j] != this[i][maxLength - j])
                    {
                        return false;
                    }
                }

            return true;
        }

        private bool IsHorizontalSymmetry()
        {
            var maxLength = grid.Length - 1;
            for (int i = 0; i < maxLength; i++)
                for (int j = 0; j < maxLength; j++)
                {
                    if (this[i][j] == Box.Wall && this[i][j] != this[maxLength - i][j])
                    {
                        return false;
                    }
                }

            return true;
        }

        private bool IsCompleteSymmetry()
        {
            var maxLength = grid.Length - 1;
            for (int i = 0; i < maxLength; i++)
            for (int j = 0; j < maxLength; j++)
            {
                var notVert = this[i][j] == Box.Wall && this[i][j] != this[i][maxLength - j];
                var notHor = this[i][j] == Box.Wall && this[i][j] != this[maxLength - i][j];
                var notDiag1 = this[i][j] == Box.Wall && this[i][j] != this[maxLength - i][maxLength - j];
                var notDiag2 = this[i][maxLength - j] == Box.Wall && this[i][maxLength - j] != this[maxLength - i][j];

                if (notVert || notHor || notDiag1 || notDiag2)
                {
                    return false;
                }
            }

            return true;
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

        public Tuple<bool, int> SolveRec(LinkedList<Board> visitedBoards, int node = 0)
        {
            Print(node);

            if (IsSolved())
                return new Tuple<bool, int>(true, node);


            // TODO check if board is symmetrical and all equivalent rotated / mirrored boards in the visitedBoard list

            var moves = GetPossibleMoves();

            var successors = GetSuccessors(visitedBoards, moves);

            var sortedSuccessors = successors.OrderBy(Heuristics.ManhattanDistance);

            foreach (var successor in sortedSuccessors)
            {
                node++;

                var result = successor.SolveRec(visitedBoards, node);
                node = result.Item2;

                if (result.Item1)
                    return result;
            }

            return new Tuple<bool, int>(false, node);
        }

        public IEnumerable<Board> GetSuccessors(LinkedList<Board> visitedBoards, IEnumerable<Move> moves)
        {
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

                if (visitedBoards.Contains(newBoard))
                    continue;

                AddEquivalentBoards(visitedBoards, newBoard);


                yield return newBoard;
            }
        }

        private void AddEquivalentBoards(LinkedList<Board> visitedBoards, Board board)
        {
            visitedBoards.AddLast(this);
            // todo: get all equivalent boards
            switch (symmetry)
            {
                case Symmetry.None:
                    break;
                case Symmetry.Horizontal:
                    break;
                case Symmetry.Vertical:
                    break;
                case Symmetry.HorizontalAndVertical:
                    break;
                case Symmetry.Complete:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static readonly Side[] Sides = {Side.Left, Side.Up, Side.Right, Side.Down};

        public IEnumerable<Move> GetPossibleMoves()
        {

            var rnd = new Random();
            var randomlyOrderedSides = Sides.OrderBy(i => rnd.Next());

            foreach (var hole in holes)
            foreach (var side in randomlyOrderedSides)
            {
                Point jumped;
                Point from;
                if (GetBox(hole, side, 2, out from) == Box.Peg && GetBox(hole, side, 1, out jumped) == Box.Peg)
                    yield return new Move(from, jumped, hole);
            }
        }

        public IDictionary<Point, IEnumerable<Move>> GetGroupedPegMoves()
        {
            var dictPegMoves = new Dictionary<Point, IEnumerable<Move>>();

            var rnd = new Random();
            var randomlyOrderedSides = Sides.OrderBy(i => rnd.Next());

            for (int i = 0; i < this.Length; i++)
            for (int j = 0; j < this[i].Length; j++)
                if (this[i][j] == Box.Peg)
                {
                    var pegPoint = new Point(i, j);
                    var pegMoves = new List<Move>();
                    foreach (var side in randomlyOrderedSides)
                    {
                        Point jumped;
                        Point to;
                        if (GetBox(pegPoint, side, 2, out to) == Box.Hole && GetBox(pegPoint, side, 1, out jumped) == Box.Peg)
                            pegMoves.Add(new Move(pegPoint, jumped, to));
                    }
                    dictPegMoves.Add(pegPoint, pegMoves);
                }

            return dictPegMoves;
        }

        private Board Clone()
        {
            var newgrid = new Box[Length][];
            Enumerable.Range(0, Length).AsParallel().ForAll(i =>
            {
                newgrid[i] = new Box[grid[i].Length];
                Array.Copy(grid[i], newgrid[i], grid[i].Length);
            });
            return new Board(newgrid, boxCount, holes.ToList(), symmetry);
        }

        private bool IsSolved()
        {
            return PegCount == 1;
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
        
        protected bool Equals(Board other)
        {
            for (int i = 0; i < this.Length; i++)
                for (int j = 0; j < this[i].Length; j++)
                    if (this[i][j] != other[i][j])
                        return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Board)obj);
        }
    }
}
