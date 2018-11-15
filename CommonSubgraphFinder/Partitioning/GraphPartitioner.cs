using System;
using System.Collections.Generic;
using System.Linq;
using CommonSubgraphFinder.Models;

namespace CommonSubgraphFinder.Partitioning
{
    class GraphPartitioner
    {
        private readonly WeightedGraph _graph;
        private readonly int _maxVerticesCount;
        private int _currentColor;
        private int[] _subgraphs;
        public List<WeightedGraph> ResultGraphs { get; private set; }

        public GraphPartitioner(WeightedGraph graph, int maxVerticesCount)
        {
            _graph = graph;
            _maxVerticesCount = maxVerticesCount;
            _currentColor = 1;
            _subgraphs = GetArrayOfValues(-1, graph.VerticesCount);
            ResultGraphs = new List<WeightedGraph>();
        }

        public static List<WeightedGraph> PartitionGraph(WeightedGraph graph, int maxVerticesCount)
        {
            var partitioner = new GraphPartitioner(graph, maxVerticesCount);
            for(int vertice = 0; vertice < graph.VerticesCount; vertice++)
                if(partitioner._subgraphs[vertice] == -1)
                    partitioner.PaintingBFS(vertice);

            return partitioner.ResultGraphs;
        }

        public void PaintingBFS(int initialVertice)
        {
            int verticesCount = this._graph.VerticesCount;
            bool[] visited = GetArrayOfValues(false, verticesCount);
            bool[] marked = GetArrayOfValues(false, verticesCount); 

            var Q = new Queue<int>();
            Q.Enqueue(initialVertice);
            int count = 0;

            while (Q.Count != 0 && count < this._maxVerticesCount)
            {
                int currentVertice = Q.Dequeue();
                var neighbours = this._graph.NeighboursOf(currentVertice);
                foreach (int neighbour in neighbours)
                {
                    if (!visited[neighbour] && !marked[neighbour])
                    {
                        Q.Enqueue(neighbour);
                        marked[neighbour] = true;
                    }
                }
                this._subgraphs[currentVertice] = this._currentColor;
                visited[currentVertice] = true;
                count++;
            }

            this._currentColor++;
        }

        private T[] GetArrayOfValues<T>(T value, int count)
        {
            return Enumerable.Repeat(value, count).ToArray();
        }
    }
}


