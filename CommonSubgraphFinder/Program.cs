using System;
using System.Diagnostics;
using CommonSubgraphFinder.MaxClique;
using CommonSubgraphFinder.Models;
using CommonSubgraphFinder.Services;
using CommonSubgraphFinder.Partitioning;

namespace CommonSubgraphFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = "./../../../Files/k30.csv";
            var graph = GraphFactory.CreateFromCsvFile(filePath);
            var weighted = new WeightedGraph(graph);
            weighted.WeightMatrix[0, 0] = 10;

            var stopwatch = Stopwatch.StartNew();
            var maxClique = MaxCliqueFinder.FindMaxClique(weighted);

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
