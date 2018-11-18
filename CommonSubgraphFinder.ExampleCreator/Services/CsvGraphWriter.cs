using System.IO;
using CommonSubgraphFinder.Models;
using CsvHelper;

namespace CommonSubgraphFinder.ExampleCreator.Services
{
    public class CsvGraphWriter
    {
        public static void WriteGraphToCsv(string filePath, Graph graph)
        {
            using (var csv = new CsvWriter(new StreamWriter(File.OpenWrite(filePath))))
            {
                for (var u = 0; u < graph.VerticesCount; u++)
                {
                    for (var v = 0; v < graph.VerticesCount; v++)
                    {
                        csv.WriteField(graph.HasEdge(u, v)? 1 : 0);
                    }
                    csv.NextRecord();
                }
            }
        }
    }
}