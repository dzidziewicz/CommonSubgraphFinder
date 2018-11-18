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
        public List<int[]> SubgraphsVerticesMaps { get; private set; }
        public List<WeightedGraph> ResultSubgraphs { get; private set; }

        public GraphPartitioner(WeightedGraph graph, int maxVerticesCount)
        {
            _graph = graph;
            _maxVerticesCount = maxVerticesCount;
            _currentColor = 0;
            _colors = GetArrayOfValues(-1, graph.VerticesCount);
            ResultSubgraphs = new List<WeightedGraph>();
            SubgraphsVerticesMaps = new List<int[]>();
        }


        /// <summary>
        ///  Main method that partitions given graph into subgraphs of given max number of vertices
        /// </summary>
        /// <param name="graph">Graph that will be partitioned</param>
        /// <param name="maxVerticesCount">Maximum amount of vertices subgraph can have</param>
        /// <returns>list of subgraphs</returns>
        public static (List<WeightedGraph>, List<int[]>) PartitionGraph(WeightedGraph graph, int maxVerticesCount)
        {
            var partitioner = new GraphPartitioner(graph, maxVerticesCount);
            for(int vertice = 0; vertice < graph.VerticesCount; vertice++)
                if(partitioner._colors[vertice] == -1)
                    partitioner.PaintingBFS(vertice);

            int maxColor = partitioner._currentColor - 1;
            for(int color =0; color <= maxColor; color++)
            {
                var (subgraph, mapping) = partitioner.GetSubgraphForColor(color);
                partitioner.ResultSubgraphs.Add(subgraph);
                partitioner.SubgraphsVerticesMaps.Add(mapping);
            }
            return (partitioner.ResultSubgraphs ,partitioner.SubgraphsVerticesMaps);
        }


        /// <summary>
        /// Modified version of BFS, that marks one subgraph in whole graph
        /// </summary>
        /// <param name="initialVertice">vertice where BFS starts</param>
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

        /// <summary>
        /// After graph vertices are marked with colors, this method creates one new graph for given color
        /// </summary>
        /// <param name="color">color of subgraph</param>
        /// <returns>Subgraph of given color and mapping to orginal vertices numbers (for further results)</returns>
        private (WeightedGraph, int[]) GetSubgraphForColor(int color)
        {
            int[] vertices = GetVerticesForColor(color);

            int[] verticesMap = new int[vertices.Length];
            for (int v = 0; v < vertices.Length; v++)
                verticesMap[v] = vertices[v];

            bool[,] subgraphAdjecencyMatrix = GetCuttedMatrix(this._graph.AdjacencyMatrix, vertices);
            int[,] subgraphWeightMatrix = GetCuttedMatrix(this._graph.WeightMatrix, vertices);
            var graph = new WeightedGraph(new Graph(subgraphAdjecencyMatrix));
            graph.WeightMatrix = subgraphWeightMatrix;
            return (graph, verticesMap);
        }

        /// <summary>
        /// Method return all vertices of given color
        /// </summary>
        /// <param name="subgraphColor">color</param>
        /// <returns>all vertices of given color</returns>
        private int[] GetVerticesForColor(int subgraphColor)
        {
            var vertices = new List<int>();
            for (int vertice = 0; vertice < this._colors.Length; vertice++)
                if (this._colors[vertice] == subgraphColor)
                    vertices.Add(vertice);
            return vertices.ToArray();
        }

        /// <summary>
        /// Helper method, that gives "cutted matrix" (common part of rows and columns described by indexes)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix">matrix to be cutted</param>
        /// <param name="indexes"> indexes of columns and rows that we want to keep</param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method, that produces array filled with same values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">values to fill</param>
        /// <param name="count">length of array</param>
        /// <returns></returns>
        private T[] GetArrayOfValues<T>(T value, int count)
        {
            return Enumerable.Repeat(value, count).ToArray();
        }

        /// <summary>
        /// Helper method, that convers list of list to two dimensional array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
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


