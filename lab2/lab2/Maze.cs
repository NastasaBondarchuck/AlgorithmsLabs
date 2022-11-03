using System;

namespace lab2
{
    public class Maze
    {
        public int Row;
        public int Col;
        public int[,] Matrix;
        public Cell Source;
        public Cell Destination;

        public Maze(int row, int col, int[,] matrix, Cell sourse, Cell destination)
        {
            Row = row;
            Col = col;
            Matrix = matrix;
            Source = sourse;
            Destination = destination;
        }
        public Maze()
        {
            Row = 10;
            Col = 10;
            Matrix = new int[,]
            {
                {1, 0, 1, 1, 0, 0, 0, 1, 0, 0},
                {1, 1, 1, 0, 1, 1, 1, 0, 0, 0},
                {0, 1, 1, 1, 1, 1, 1, 1, 0, 1},
                {1, 1, 0, 1, 1, 1, 0, 1, 1, 1},
                {0, 1, 0, 1, 0, 1, 0, 0, 0, 1},
                {1, 1, 1, 1, 1, 0, 0, 0, 1, 1},
                {0, 1, 0, 0, 0, 1, 1, 1, 1, 0},
                {0, 1, 0, 0, 0, 1, 1, 0, 0, 0},
                {0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            Source =new Cell(0, 0);
            Destination = new Cell(9, 9);
        }
        public bool IsValid(int row, int col)
        {
            return ((row >= 0) && (row < Row) && (col >= 0) && (col < Col));
        }

        public void ManhattanDistance(Cell current)
        {
            int x = Math.Abs(Destination.X - current.X);
            int y = Math.Abs(Destination.Y - current.Y);
            current.MDistance = x + y;
        }

        public void PrintMaze()
        {
            Console.Write("  ");
            for (int num = 0; num < Matrix.GetLength(1); num++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                Console.Write("||");
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (Matrix[i, j] == 0)
                    {
                        Console.Write("x");
                    }
                    else if (Matrix[i, j] == 1)
                    {
                        if (i == Source.X && j == Source.Y)
                        {
                            Console.Write("S");
                        }
                        else if (i == Destination.X && j == Destination.Y)
                        {
                            Console.Write("D");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                }
                Console.Write("||");
                Console.WriteLine();
            }
            Console.Write("  ");
            for (int num = 0; num < Matrix.GetLength(1); num++)
            {
                Console.Write("=");
            }
        }
    }
}