using System;
using CommonSubgraphFinder.ExampleCreator.Services;

namespace CommonSubgraphFinder.ExampleCreator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("A single example requires two graphs");
            Console.WriteLine("Define the first graph");

            var g = GraphBuilder.Build();

            Console.WriteLine();
            Console.WriteLine("Define the second graph");

            var h = GraphBuilder.Build();

            Console.WriteLine();

            var fileNameForG = PathService.GetFileName(g.VerticesCount, h.VerticesCount, true);
            var fileNameForH = PathService.GetFileName(g.VerticesCount, h.VerticesCount, false);

            var pathForG = PathService.GetInputFilePath(fileNameForG);
            var pathForH = PathService.GetInputFilePath(fileNameForH);

            CsvGraphWriter.WriteGraphToCsv(pathForG, g);
            CsvGraphWriter.WriteGraphToCsv(pathForH, h);

            Console.WriteLine("Writing to files finished!");
        }
    }
}
