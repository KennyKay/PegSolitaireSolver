using System;
using System.IO;

namespace PegSolitaireSolver
{
    public static class BoardReader
    {
        public static Board ReadBoard(string path)
        {
            const int rowLength = 7;
            const int columnLength = 7;

            var rows = new Box[rowLength][];

            using (var sr = new StreamReader(path))
                for (var i = 0; i < rowLength; i++)
                {
                    rows[i] = new Box[columnLength];

                    var line = sr.ReadLine();

                    for (int j = 0; j < columnLength; j++)
                        rows[i][j] = (Box) (line[j] - '0');
                }
            return new Board(rows);
        }
    }
}
