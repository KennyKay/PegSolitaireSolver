using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegSolitaireSolver
{
    public class Move
    {
        public Point From { get; private set; } // Peg
        public Point Jumped { get; private set; } // Peg
        public Point To { get; private set; } // Hole

        public Move(Point from, Point jumped, Point to)
        {
            From = from;
            Jumped = jumped;
            To = to;
        }
    }
}
