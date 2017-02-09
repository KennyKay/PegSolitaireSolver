using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegSolitaireSolver
{
    public struct Point
    {
        public byte Row;
        public byte Column;

        public Point(byte row, byte column)
        {
            Row = row;
            Column = column;
        }

        public Point(int row, int column)
        {
            Row = (byte) row;
            Column = (byte) column;
        }

        public Point(Point point)
        {
            Row = point.Row;
            Column = point.Column;
        }

        public bool Equals(Point other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point && Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row.GetHashCode() * 397) ^ Column.GetHashCode();
            }
        }
    }
}
