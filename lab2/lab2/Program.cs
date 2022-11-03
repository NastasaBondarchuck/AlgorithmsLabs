using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace lab2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Console.Write("Number of rows: ");
            // int Row = Convert.ToInt32(Console.ReadLine());
            // Console.Write("\nNumber of cols: ");
            // int Col = Convert.ToInt32(Console.ReadLine());
            // Random rand = new Random();
            // int[,] matrix = new int[Row,Col];
            // for (int i = 0; i < Row; i++)
            // {
            //     for (int j = 0; j < Col; j++)
            //     {
            //         int temp = rand.Next(-5, 20);
            //         matrix[i, j] = temp < 0 ? 0 : 1;
            //     }
            // }
            //
            // matrix[0, 0] = matrix[Row-1, Col-1] = 1;
            Maze maze = new Maze();
            Cell source = new Cell();
            source.Path = new List<Cell>{new Cell(0, 0)};
            // Cell destination = new Cell(Row-1, Col-1);
            Cell destination = new Cell(9, 9);
            // Maze maze = new Maze(matrix.GetLength(0), matrix.GetLength(1), matrix, source, destination);
           
            
            
            maze.ManhattanDistance(source);
            // List<Cell> result = FindWayAlgorithm.BFS(source, destination, maze);
            List<Cell> result = FindWayAlgorithm.AStar(source, destination, maze);

            maze.PrintMaze();
            Console.WriteLine();
            if (result.Count > 0)
            {
                Console.WriteLine($"The shortest path is: {result.Count}.");
                Console.Write("Path is: ");
                foreach (var cell in result)
                {
                    if (result.IndexOf(cell) == result.Count-1)
                    {
                        Console.Write($"({cell.X}.{cell.Y})");
                    }
                    else
                    {
                        Console.Write($"({cell.X}.{cell.Y}) -> ");
                    }
                   
                }
            }
            else
            {
                Console.WriteLine("Path doesn't exist.");
            }

            Console.ReadLine();
        }
        
    }
}