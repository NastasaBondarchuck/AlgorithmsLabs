using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace lab4
{
    public class Algorithm
    {
        private Graph Graph;
        private List<int> AvailableVertices;
        private readonly Graph InitialGraph;
        private readonly int[] PaletteArray;
        private readonly List<int> UsedColorsList;

        public Algorithm(Graph initialGraph)
        {
            InitialGraph = initialGraph;
            Graph = new Graph(initialGraph);
            AvailableVertices = Helper.GetVertices();
            PaletteArray = new int[Constants.MaxVertexDegree + 1];
            for (int i = 0; i < Constants.MaxVertexDegree + 1; i++)
            {
                PaletteArray[i] = i;
            }

            UsedColorsList = new List<int>();
        }

        public Graph Run()
        {
            Graph resultGraph = new Graph(Graph);
            int bestChromaticNumber = GetBestChromaticNumber();

            Console.WriteLine("Initialize colored graph: ");
            Helper.PrintArray(Graph.GetColors());
            Console.WriteLine(
                "The optimal solution for the graph on the first iteration (previous maximum vertex " +
                $"degree: {Constants.MaxVertexDegree + 1}); new best chromatic number: {bestChromaticNumber}. " +
                "Time of process: 0 seconds.");
            Reset();
            
            for (int iterations = 0; iterations < Constants.IterationsMaxNumber;)
            {
                Stopwatch sw = Stopwatch.StartNew();
                for (int k = 1; k < Constants.IterationsPerStep + 1; k++, Reset())
                {
                    int newChromaticNumber = GetBestChromaticNumber();
                    if (newChromaticNumber >= bestChromaticNumber) continue;
                    Console.WriteLine($"After {iterations + k} iterations, a new optimal solution: " +
                                      $"previous best chromatic number: {bestChromaticNumber}, and the new one: " +
                                      $"{bestChromaticNumber = newChromaticNumber}. Time of process " +
                                      $"{sw.ElapsedMilliseconds / 1000} seconds.");
                    resultGraph = new Graph(Graph);
                }

                Console.WriteLine($"Iteration: {iterations += Constants.IterationsPerStep}, the best result: {bestChromaticNumber}. " +
                                  $"Time of process: {sw.ElapsedMilliseconds / 1000} seconds.");
            }

            Console.WriteLine("Initial colors of graph are (-1 - no color): ");
            Helper.PrintArray(Graph.GetColors());
            return resultGraph;
        }

        private void Reset()
        {
            UsedColorsList.Clear();
            AvailableVertices = Helper.GetVertices();
            Graph = new Graph(InitialGraph);
        }

        private int GetBestChromaticNumber()
        {
            while (!Graph.IsGraphProperlyColored())
            {
                ColorSelectedVertices(SelectExplorerVertices());
            }

            return UsedColorsList.Count;
        }

        private List<int> SelectExplorerVertices()
        {
            var selectedVertices = new List<int>();
            var random = new Random();
            int numberOfExplorers = Constants.ExplorerBeesNumber;
            while (numberOfExplorers > 0 && AvailableVertices.Count > 0)
            {
                int index = random.Next(AvailableVertices.Count);
                int randomSelectedVertex = AvailableVertices[index];
                AvailableVertices.RemoveAt(index);
                selectedVertices.Add(randomSelectedVertex);
                numberOfExplorers--;
            }

            return selectedVertices;
        }


        private void ColorSelectedVertices(IReadOnlyList<int> selectedVertices)
        {
            var degrees = new int[selectedVertices.Count];
            for (int i = 0; i < degrees.Length; i++)
            {
                degrees[i] = Graph.GetDegreeOfVertex(selectedVertices[i]);
            }

            var onlookerBeesCounts = Helper.GetOnlookersBeesSplit(degrees);
            for (int i = 0; i < selectedVertices.Count; i++)
            {
                var connectedVertices = Graph.GetAdjacentVertices(selectedVertices[i]);
                ColorConnectedVertex(connectedVertices, onlookerBeesCounts[i]);
                ColorVertex(selectedVertices[i]);
            }
        }

        private void ColorConnectedVertex(IReadOnlyList<int> connectedVertices, int onlookerBeesCount)
        {
            for (int i = 0; i < connectedVertices.Count; ++i)
            {
                if (i < onlookerBeesCount - 1) 
                    ColorVertex(connectedVertices[i]);
            }
        }

        private void ColorVertex(int vertex)
        {
            var availableColors = new HashSet<int>(UsedColorsList);
            var random = new Random();
            int color;
            while (true)
            {
                if (availableColors.Count == 0)
                {
                    color = PaletteArray[UsedColorsList.Count];
                    UsedColorsList.Add(color);
                    break;
                }

                color = availableColors.ElementAt(random.Next(availableColors.Count));
                availableColors.Remove(color);
                if (Graph.IsColorChangeValid(vertex, color))
                    break;
            }

            Graph.IsColorChangeValid(vertex, color);
        }
    }
}