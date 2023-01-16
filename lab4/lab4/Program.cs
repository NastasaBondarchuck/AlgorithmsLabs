using System;
using System.Diagnostics;
namespace lab4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var adjMatrix = new int[Constants.VerticesNumber, Constants.VerticesNumber];
            var graph = new Graph(adjMatrix);

            Console.WriteLine("Matrix is valid: " + graph.IsMatrixValid());
            Console.WriteLine("Graph degrees: ");
            Helper.PrintArray(graph.GetVertexDegrees());

            Console.WriteLine("Running...");
            var sw = Stopwatch.StartNew();

            graph = new Algorithm(graph).Run();
            sw.Stop();
            Console.WriteLine($"Time to run: {sw.ElapsedMilliseconds / 1000}s");
        
            Console.WriteLine("Graph coloring: ");
            Helper.PrintArray(graph.GetColors());

            Console.WriteLine("Graph colored properly: " + graph.IsGraphProperlyColored());
        }
    }
}