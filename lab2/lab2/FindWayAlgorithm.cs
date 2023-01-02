using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.SqlServer.Server;

namespace lab2
{
    public class FindWayAlgorithm
    {
        public static List<(int, int)> BFS(Cell source, Cell destination, Maze maze)
        {
            int[] rowsMove = { -1, 0, 0, 1 };
            int[] colsMove = { 0, -1, 1, 0 };
            if (maze.Matrix[source.X,source.Y] != 1 || maze.Matrix[destination.X,destination.Y] != 1)
            {
                return new List<(int, int)>();
            }
            bool[,] visitedCells = new bool[maze.Matrix.GetLength(0), maze.Matrix.GetLength(1)];
            visitedCells[source.X, source.Y] = true;
            List<Cell> Queue = new List<Cell> {source};
            while (Queue.Count != 0)
            {
                Cell current = Queue[0];
                if (current.X == destination.X && current.Y == destination.Y)
                {
                    return destination.Path = current.Path;
                    // return current.Distance;
                }
                Queue.RemoveAt(0);
                for (int i = 0; i < 4; i++)
                {
                    int row = current.X + rowsMove[i];
                    int col = current.Y + colsMove[i];
                    if (maze.IsValid(row, col) && maze.Matrix[row, col] == 1 && !visitedCells[row, col])
                    {
                        visitedCells[row, col] = true;
                        Cell NeighborCell = new Cell(row, col)
                        {
                            Path = new List<(int, int)>(current.Path)
                        };
                        NeighborCell.Path.Add((NeighborCell.X, NeighborCell.Y));
                        
                        Queue.Add(NeighborCell);
                    }
                }
            }
            Queue.Clear();
            return new List<(int, int)>();
        }

        public static List<(int, int)> AStar(Cell source, Cell destination, Maze maze)
        {
            int[] rowsMove = { -1, 0, 0, 1 };
            int[] colsMove = { 0, -1, 1, 0 };
            
            if (maze.Matrix[source.X,source.Y] != 1 || maze.Matrix[destination.X,destination.Y] != 1)
            {
                return new List<(int, int)>();
            }
            
            bool[,] visitedCells = new bool[maze.Matrix.GetLength(0), maze.Matrix.GetLength(1)];
            visitedCells[source.X, source.Y] = true;
            List<Cell> PriorityQueue = new List<Cell>();
            PriorityQueue.Add(source);
            while (PriorityQueue.Count != 0)
            {
                
                Cell current = PriorityQueue[0];
                maze.ManhattanDistance(current);
                if (current.X == destination.X && current.Y == destination.Y)
                {
                    return destination.Path = current.Path;
                }
                PriorityQueue.RemoveAt(0);
                for (int i = 0; i < 4; i++)
                {
                    int row = current.X + rowsMove[i];
                    int col = current.Y + colsMove[i];
                    if (maze.IsValid(row, col) && maze.Matrix[row, col] == 1 && !visitedCells[row, col])
                    {
                        visitedCells[row, col] = true;
                        Cell NeighborCell = new Cell(row, col)
                        {
                            Path = new List<(int, int)>(current.Path)
                        };
                        NeighborCell.Path.Add((NeighborCell.X, NeighborCell.Y));
                        maze.ManhattanDistance(NeighborCell);
                        AddToQueue(PriorityQueue, NeighborCell);
                    }
                    
                }
            }
            PriorityQueue.Clear();
            return new List<(int, int)>();
        }

        public static void AddToQueue(List<Cell> Queue, Cell cell)
        {

            if (Queue.Count > 0)
            {
                int index = 0;
                while (index < Queue.Count && cell.MDistance + cell.Path.Count > Queue[index].MDistance + Queue[index].Path.Count)
                {
                    ++index;
                }
                if (index < Queue.Count - 1)
                {
                    Queue.Insert(index, cell);
                }
                else
                {
                    Queue.Add(cell);
                }
            }
            else
            {
                Queue.Add(cell);
            }
        }
        
    }
}