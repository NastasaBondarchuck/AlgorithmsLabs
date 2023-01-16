using System;
using System.Collections.Generic;
using System.Linq;

namespace lab4
{
    public class Helper
    {
        public static void PrintArray(int[] arr)
        {
            const int maxRowLength = 10;
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + "\t");
                if ((i + 1) % maxRowLength == 0) 
                    Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public static List<int> GetVertices()
        {
            return Enumerable.Range(0, Constants.VertexCount).ToList();
        }

        public static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static double[] GetNectarValues(IReadOnlyList<int> selectedVerticesDegrees)
        {
            double[] res = new double[selectedVerticesDegrees.Count];
            for (int i = 0, totalDegrees = selectedVerticesDegrees.Sum(); i < selectedVerticesDegrees.Count; ++i)
            {
                res[i] = (double)selectedVerticesDegrees[i] / totalDegrees;
            }
            return res;
        }

        public static int[] GetOnlookersBeesSplit(int[] selectedVerticesDegrees)
        {
            double[] nectarValues = GetNectarValues(selectedVerticesDegrees);
            int onlookerBeesCount = Constants.TotalBeesNumber - Constants.ExplorerBeesNumber;
            int[] res = new int[nectarValues.Length];
            for (int i = 0; i < nectarValues.Length; ++i)
            {
                res[i] = (int)(onlookerBeesCount * nectarValues[i]);
                onlookerBeesCount -= res[i];
            }
            return res;
        }
    }
}