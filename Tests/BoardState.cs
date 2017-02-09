using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PegSolitaireSolver;

namespace Tests
{
    [TestClass]
    public class BoardState
    {
        [TestMethod]
        public void IsValid1()
        {
            var grid = new[]
            {
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, Box.Token, 0, 0, 0},
                new Box[] {0, 0, 0, Box.Token, 0, 0, 0},
                new Box[] {0, 0, 0, Box.Empty, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
            };
            var board = new Board(grid);

            Assert.IsTrue(board.IsValid());
        }

        [TestMethod]
        public void IsValid2()
        {
            var grid = new[]
            {
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, Box.Token, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
            };
            var board = new Board(grid);

            Assert.IsTrue(board.IsValid());
        }

        [TestMethod]
        public void IsInvalid1()
        {
            var grid = new[]
            {
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, Box.Empty, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
            };
            var board = new Board(grid);

            Assert.IsFalse(board.IsValid());
        }

        [TestMethod]
        public void IsInvalid2()
        {
            var grid = new[]
            {
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
                new Box[] {0, 0, 0, 0, 0, 0, 0},
            };
            var board = new Board(grid);

            Assert.IsFalse(board.IsValid());
        }
    }
}
