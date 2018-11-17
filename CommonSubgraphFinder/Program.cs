using System;
using System.Diagnostics;
using CommonSubgraphFinder.MaxClique;
using CommonSubgraphFinder.Models;
using CommonSubgraphFinder.Services;
using CommonSubgraphFinder.Partitioning;

namespace CommonSubgraphFinder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string filePath1 = "./../../../Files/simpleGraph.csv";
            const string filePath2 = "./../../../Files/simpleGraph.csv";
            var g = GraphFactory.CreateFromCsvFile(filePath1);
            var h = GraphFactory.CreateFromCsvFile(filePath2);

            var modularProduct = ModularProductService.GetModularProductForVertexMaxGraph(g, h);
            //var weighted = new WeightedGraph(graph);
            //weighted.WeightMatrix[0, 0] = 10;

            var stopwatch = Stopwatch.StartNew();
            var maxClique = MaxCliqueFinder.FindMaxClique(modularProduct);

            // Aproximate 

            //var subgraphs = GraphPartitioner.PartitionGraph(weighted, 6);
            //foreach (var subgraph in subgraphs)
            //    MaxCliqueFinder.FindMaxClique(subgraph);

            stopwatch.Stop();
            Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
