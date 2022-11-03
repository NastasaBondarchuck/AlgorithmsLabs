using System;
using System.Collections.Generic;

namespace lab2
{
    public class Cell
    {
        public int X;
        public int Y;
        public int MDistance;
        public List<Cell> Path;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            MDistance = Int32.MaxValue;
            Path = new List<Cell>();
        }

        public Cell()
        {
            X = 0;
            Y = 0;
        }
    }
}