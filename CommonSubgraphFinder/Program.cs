using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommonSubgraphFinder.MaxClique;
using CommonSubgraphFinder.Models;
using CommonSubgraphFinder.Services;
using CommonSubgraphFinder.Partitioning;

namespace CommonSubgraphFinder
{
    internal class Program
    {
        const string fileName1 = "simpleGraph.csv";
        const string fileName2 = "simpleGraph.csv";

        private static void Main(string[] args)
        {
            string filePath1 = PathService.GetInputFilePath(fileName1);
            string filePath2 = PathService.GetInputFilePath(fileName2);
            string resultPath = PathService.GetResultFilePath(fileName1, fileName2);
            
            var g = CsvFilesService.CreateGraphFromCsv(filePath1);
            var h = CsvFilesService.CreateGraphFromCsv(filePath2);

            var modularProduct = ModularProductService.GetModularProductForVertexMaxGraph(g, h);
            //var weighted = new WeightedGraph(graph);
            //weighted.WeightMatrix[0, 0] = 10;

            var stopwatch = Stopwatch.StartNew();
            var maxClique = MaxCliqueFinder.FindMaxClique(modularProduct, false);

            var mapping = ModularProductService.MapMpToBaseGraphs(g, h, maxClique.ToList());

            // Aproximate 

            //var subgraphs = GraphPartitioner.PartitionGraph(weighted, 6);
            //foreach (var subgraph in subgraphs)
            //    MaxCliqueFinder.FindMaxClique(subgraph);

            stopwatch.Stop();
            Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine();

            ShowResults(mapping, resultPath);

            Console.ReadKey();
        }

        private static void ShowResults(CommonSubgraphMapping mapping, string outputFilePath)
        {
            Console.WriteLine("Common subgraph found:");
            Console.WriteLine("G vertices:");

            foreach (var v in mapping.FirstGraphVertices)
                Console.Write(v + " ");

            Console.WriteLine();
            Console.WriteLine("H vertices:");

            foreach (var v in mapping.SecondGraphVertices)
                Console.Write(v + " ");

            Console.WriteLine();

            CsvFilesService.WriteCommonSubgraphToCsv(outputFilePath, mapping);
        }
    }
}
