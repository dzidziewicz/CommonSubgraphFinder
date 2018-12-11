using System;
using System.Collections.Generic;

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

        public Graph(bool[,] adjacencyMatrix)
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

        public bool HasEdge(int vertex1, int vertex2)
        {
            if (!IsVertexIndexValid(vertex1) || !IsVertexIndexValid(vertex2))
                throw new Exception("Invalid vertex index");

            return AdjacencyMatrix[vertex1, vertex2];
        }

        public IEnumerable<int> NeighboursOf(int vertex)
        {
            for (int u = 0; u < VerticesCount; u++)
            {
                if (AdjacencyMatrix[vertex, u])
                    yield return u;
            }
        }
        protected bool IsVertexIndexValid(int v)
        {
            return v >= 0 && v < VerticesCount;
        }

    }
}
