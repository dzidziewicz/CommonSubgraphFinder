using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace CommonSubgraphFinder.Models
{
    public class WeightedGraph : Graph
    {
        public int[,] WeightMatrix { get; set; }

        public WeightedGraph(int verticesCount) : base(verticesCount)
        {
            WeightMatrix = new int[verticesCount, verticesCount];
        }

        public WeightedGraph(Graph baseGraph, int vertexWeight = 1, int edgeWeight = 1) : base(baseGraph.AdjacencyMatrix)
        {
            WeightMatrix = new int[VerticesCount, VerticesCount];
            FillWeightMatrix(vertexWeight, edgeWeight);
        }

        public void AddEdge(int vertex1, int vertex2, int weight)
        {
            base.AddEdge(vertex1, vertex2);
            WeightMatrix[vertex1, vertex2] = weight;
        }

        public void SetVertexWeight(int vertex, int weight)
        {
            if (!IsVertexIndexValid(vertex))
                throw new Exception("Invalid vertex index");
            WeightMatrix[vertex, vertex] = weight;
        }

        private void FillWeightMatrix(int vertexWeight, int edgeWeight)
        {
            for (int v = 0; v < VerticesCount; v++)
            {
                for (int u = 0; u < VerticesCount; u++)
                {
                    if (u == v)
                    {
                        WeightMatrix[u, v] = vertexWeight;
                    }
                    else if (AdjacencyMatrix[u, v])
                    {
                        WeightMatrix[u, v] = edgeWeight;
                    }
                }
            }
        }

        public IEnumerable<int> NeighboursOf(int vertex)
        {
            for (int u = 0; u < VerticesCount; u++)
            {
                if (AdjacencyMatrix[vertex, u])
                    yield return u;
            }
        }
    }
}
