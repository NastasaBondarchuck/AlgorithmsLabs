using System;
using System.Diagnostics;


namespace lab1
{
    
    internal class Program
    {
        static void Main(string[] args) 
        {
            const int linesCount = 100_000;
            const int megabytes = 10;
            Console.WriteLine("Not modified");
            Normal(megabytes);
            Console.WriteLine("\nModified");
            Modified(linesCount);
            Console.ReadLine();
        }
        static void Normal(int megabytes)
        {
            string path = "A.bin";
            
            RandomGenerator.GenerateBySize(path, megabytes);
            Console.WriteLine("Not modified file is generated");
            
            var sw = Stopwatch.StartNew();
            KwaySort.MergeSort(path, out string sortedFileName, false);
            sw.Stop();
            Console.WriteLine($"Sorted, file: {sortedFileName}, seconds: {sw.Elapsed.TotalSeconds}");
        }

        static void Modified(int linesNumber)
        {
            string path = "Amod.bin";
            
            RandomGenerator.GenerateByLinesCount(path, linesNumber);
            Console.WriteLine("Modified file is generated"); ;
            var sw = Stopwatch.StartNew();
            KwaySort.SortParts(path, "Result.bin", linesNumber, linesNumber / 8);
            KwaySort.MergeSort("Result.bin", out string sortedFileName, true);
            sw.Stop();
            Console.WriteLine($"Sorted, file: {sortedFileName}, seconds: {sw.Elapsed.TotalSeconds}");
            Helper.ShowFile(sortedFileName, 20);
        }
    }
}