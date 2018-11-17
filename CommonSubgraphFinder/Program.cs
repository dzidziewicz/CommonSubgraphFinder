using System;
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
            var maxClique = MaxCliqueFinder.FindMaxClique(modularProduct, false);

            ModularProductService.MapMpToBaseGraphs(g, h, maxClique.ToList(), out var gVertices, out var hVertices);

            // Aproximate 

            //var subgraphs = GraphPartitioner.PartitionGraph(weighted, 6);
            //foreach (var subgraph in subgraphs)
            //    MaxCliqueFinder.FindMaxClique(subgraph);

            stopwatch.Stop();
            Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine();

            Console.WriteLine("Common subgraph found:");
            Console.WriteLine("G vertices:");
            foreach (var v in gVertices)
            {
                Console.Write(v + " ");
            }

            Console.WriteLine();

            Console.WriteLine("H vertices:");
            foreach (var v in hVertices)
            {
                Console.Write(v + " ");
            }

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
