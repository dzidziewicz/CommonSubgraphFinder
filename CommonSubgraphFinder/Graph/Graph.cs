using CsvHelper;
using System;
using System.IO;

namespace CommonSubgraphFinder.Graph
{
    public class Graph
    {
        private readonly bool[,] _adjacencyMatrix;

        public int VerticesCount { get; set; }

        public Graph(int verticesCount)
        {
            VerticesCount = verticesCount;
            _adjacencyMatrix = new bool[verticesCount, verticesCount];
        }

        public Graph(string filePath)
        {
            var rowsCount = CountRows(filePath);

            VerticesCount = rowsCount;
            _adjacencyMatrix = new bool[VerticesCount, VerticesCount];

            FillAdjacencyMatrix(filePath);
        }

        public void AddEdge(int vertex1, int vertex2)
        {
            if (!IsVertexIndexValid(vertex1) || !IsVertexIndexValid(vertex2))
                throw new Exception("Invalid vertex index");

            _adjacencyMatrix[vertex1, vertex2] = true;
        }

        private bool IsVertexIndexValid(int v)
        {
            return v >= 0 && v < VerticesCount;
        }

        private int CountRows(string filePath)
        {
            using (var csv = new CsvReader(File.OpenText(filePath)))
            {
                var rowsCount = 0;
                while (csv.Read())
                {
                    rowsCount++;
                }
                return rowsCount;
            }
        }

        private void FillAdjacencyMatrix(string filePath)
        {
            using (var csv = new CsvReader(File.OpenText(filePath)))
            {
                var row = 0;
                while (csv.Read())
                {
                    var col = 0;
                    for (; csv.TryGetField(col, out int edge); col++)
                    {
                        if (edge == 1)
                            AddEdge(row, col);
                    }

                    if (col < VerticesCount)
                        throw new Exception($"Too few columns in row number {row}");

                    row++;
                }
            }
        }
    }
}
