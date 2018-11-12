using System;
using System.IO;
using CsvHelper;

namespace CommonSubgraphFinder.Models
{
    public class Graph
    {
        public bool[,] AdjacencyMatrix { get; }

        public int VerticesCount { get; }

        public Graph(int verticesCount)
        {
            VerticesCount = verticesCount;
            AdjacencyMatrix = new bool[verticesCount, verticesCount];
        }

        protected Graph(bool[,] adjacencyMatrix)
        {
            if(adjacencyMatrix.GetLength(0) != adjacencyMatrix.GetLength(1))
                throw new Exception("Invalid adjacency matrix");
            AdjacencyMatrix = adjacencyMatrix;
            VerticesCount = adjacencyMatrix.GetLength(0);
        }

        public virtual void AddEdge(int vertex1, int vertex2)
        {
            if (!IsVertexIndexValid(vertex1) || !IsVertexIndexValid(vertex2))
                throw new Exception("Invalid vertex index");

            AdjacencyMatrix[vertex1, vertex2] = true;
        }

        protected bool IsVertexIndexValid(int v)
        {
            return v >= 0 && v < VerticesCount;
        }

    }
}
