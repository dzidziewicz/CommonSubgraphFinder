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
        private int[] _colors;
        public List<WeightedGraph> ResultSubgraphs { get; private set; }

        public GraphPartitioner(WeightedGraph graph, int maxVerticesCount)
        {
            _graph = graph;
            _maxVerticesCount = maxVerticesCount;
            _currentColor = 0;
            _colors = GetArrayOfValues(-1, graph.VerticesCount);
            ResultSubgraphs = new List<WeightedGraph>();
        }

        public static List<WeightedGraph> PartitionGraph(WeightedGraph graph, int maxVerticesCount)
        {
            var partitioner = new GraphPartitioner(graph, maxVerticesCount);
            for(int vertice = 0; vertice < graph.VerticesCount; vertice++)
                if(partitioner._colors[vertice] == -1)
                    partitioner.PaintingBFS(vertice);

            int maxColor = partitioner._currentColor - 1;
            for(int color =0; color <= maxColor; color++)
            {
                var subgraph = partitioner.GetSubgraphForColor(color);
                partitioner.ResultSubgraphs.Add(subgraph);
            }
            return partitioner.ResultSubgraphs;
        }

        private void PaintingBFS(int initialVertice)
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
                    if (!visited[neighbour] && !marked[neighbour] && this._colors[neighbour] == -1)
                    {
                        Q.Enqueue(neighbour);
                        marked[neighbour] = true;
                    }
                }
                this._colors[currentVertice] = this._currentColor;
                visited[currentVertice] = true;
                count++;
            }

            this._currentColor++;
        }

        private WeightedGraph GetSubgraphForColor(int color)
        {
            int[] vertices = GetVerticesForColor(color);
            bool[,] subgraphAdjecencyMatrix = GetCuttedMatrix(this._graph.AdjacencyMatrix, vertices);
            int[,] subgraphWeightMatrix = GetCuttedMatrix(this._graph.WeightMatrix, vertices);
            var graph = new WeightedGraph(new Graph(subgraphAdjecencyMatrix));
            graph.WeightMatrix = subgraphWeightMatrix;
            return graph;
        }

        private T[,] GetCuttedMatrix<T>(T[,] matrix, int[] indexes)
        {
            int size = matrix.GetLength(0);
            var rows = new List<List<T>>();
            for(int i =0; i < size; i++)
            {
                if (indexes.Contains(i))
                {
                    var row = new List<T>();
                    for (int j = 0; j < size; j++)
                    {
                        if (indexes.Contains(j))
                            row.Add(matrix[i, j]);
                    }
                    rows.Add(row);
                }
            }
            return LLTo2DArray(rows);
        }

        private int[] GetVerticesForColor(int subgraphColor)
        {
            var vertices = new List<int>();
            for (int vertice = 0; vertice < this._colors.Length; vertice++)
                if (this._colors[vertice] == subgraphColor)
                    vertices.Add(vertice);
            return vertices.ToArray();
        }

        private T[] GetArrayOfValues<T>(T value, int count)
        {
            return Enumerable.Repeat(value, count).ToArray();
        }

        private T[,] LLTo2DArray<T>(List<List<T>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            int max = source.Select(l => l).Max(l => l.Count());

            var result = new T[source.Count, max];

            for (int i = 0; i < source.Count; i++)
            {
                for (int j = 0; j < source[i].Count(); j++)
                {
                    result[i, j] = source[i][j];
                }
            }

            return result;
        }
    }
}


