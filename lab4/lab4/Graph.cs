using System;

namespace lab4
{
    public class Graph
    {
         private readonly int[,] AdjacencyMatrix;
        private readonly int[] Colors;

        public Graph(Graph g)
        {
            AdjacencyMatrix = new int[g.AdjacencyMatrix.GetLength(0), g.AdjacencyMatrix.GetLength(1)];
            Colors = new int[g.Colors.Length];

            Array.Copy(g.AdjacencyMatrix, AdjacencyMatrix, g.AdjacencyMatrix.Length);
            Array.Copy(g.Colors, Colors, g.Colors.Length);
        }

        public Graph(int[,] adjacencyMatrix)
        { 
            Random rand = new Random();
            AdjacencyMatrix = adjacencyMatrix;
            Colors = new int[adjacencyMatrix.GetLength(0)];
            for (int i = 0; i < Colors.Length; i++)
            {
                Colors[i] = Constants.NoColor;
            }

            for (int currentVertex = 0; currentVertex < Constants.VertexCount; ++currentVertex)
            {
                int finalVertexDegree = Math.Min(rand.Next(Constants.MinVertexDegree, Constants.MaxVertexDegree + 1)
                                                 - GetDegreeOfVertex(currentVertex), Constants.VertexCount - currentVertex - 1);
                for (int newConnections = 0; newConnections < finalVertexDegree; ++newConnections)
                {
                    bool isConnectedAlready = true;
                    for (int tryCount = 0, newVertex = rand.Next(currentVertex + 1, Constants.VertexCount);
                         isConnectedAlready && tryCount < Constants.VertexCount;
                         ++tryCount, newVertex = rand.Next(currentVertex + 1, Constants.VertexCount))
                    {
                        if (AdjacencyMatrix[currentVertex, newVertex] == 0 &&
                            GetDegreeOfVertex(newVertex) < Constants.MaxVertexDegree)
                        {
                            isConnectedAlready = false;
                            AdjacencyMatrix[currentVertex, newVertex] = 1;
                            AdjacencyMatrix[newVertex, currentVertex] = 1;
                        }
                    }
                }
            }
        }
        public bool IsMatrixValid()
        {
            for (int vertex = 0; vertex < AdjacencyMatrix.GetLength(0); vertex++)
            {
                if (GetDegreeOfVertex(vertex) > Constants.MaxVertexDegree) 
                    return false;
            }

            return true;
        }
        public bool IsGraphProperlyColored()
        {
            for (int i = 0; i < Colors.Length; i++)
            {
                if (Colors[i] == Constants.NoColor) 
                    return false;
            }

            return IsColoringValid();
        }
        public int[] GetColors()
        {
            return Colors;
        }
        public int[] GetVertexDegrees()
        {
            int[] vertexDegrees = new int[AdjacencyMatrix.GetLength(0)];
            for (int i = 0; i < vertexDegrees.Length; ++i)
            {
                vertexDegrees[i] = GetDegreeOfVertex(i);
            }

            return vertexDegrees;
        }
        public int GetDegreeOfVertex(int vertex)
        {
            int degree = 0;
            for (int i = 0; i < AdjacencyMatrix.GetLength(0); i++)
            {
                degree += AdjacencyMatrix[vertex, i];
            }

            return degree;
        }
        public int[] GetAdjacentVertices(int vertex)
        {
            int[] adjacentVertices = new int[GetDegreeOfVertex(vertex)];
            int index = 0;
            for (int i = 0; i < AdjacencyMatrix.GetLength(0); ++i)
            {
                if (AdjacencyMatrix[vertex, i] == 1) 
                    adjacentVertices[index++] = i;
            }

            return adjacentVertices;
        }
        public bool IsColorChangeValid(int vertex, int newColor)
        {
            int previousColor = Colors[vertex];
            Colors[vertex] = newColor;
            bool isValid = IsColoringValid();
            if (!isValid) 
                Colors[vertex] = previousColor;

            return isValid;
        }
        private bool IsColoringValid()
        {
            for (int i = 0; i < AdjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < AdjacencyMatrix.GetLength(1); j++)
                {
                    if (AdjacencyMatrix[i, j] == 1 && Colors[i] != Constants.NoColor && Colors[i] == Colors[j]) 
                        return false;
                }
            }

            return true;
        }
    }
    
}