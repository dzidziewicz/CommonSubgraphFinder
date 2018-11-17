using System;
using System.IO;
using CommonSubgraphFinder.Models;
using CsvHelper;

namespace CommonSubgraphFinder.Services
{
    public static class GraphFactory
    {
        public static Graph CreateFromCsvFile(string filePath)
        {
            var rowsCount = CountRows(filePath);
            var graph = new Graph(rowsCount);

            FillAdjacencyMatrix(filePath, graph);

            return graph;
        }


        private static int CountRows(string filePath)
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

        private static void FillAdjacencyMatrix(string filePath, Graph graph)
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
                            graph.AddEdge(row, col);
                    }

                    if (col < graph.VerticesCount)
                        throw new Exception($"Too few columns in row number {row}");

                    row++;
                }
            }
        }
    }
}
