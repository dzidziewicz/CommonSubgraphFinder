using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommonSubgraphFinder.MaxClique;
using CommonSubgraphFinder.Models;
using CommonSubgraphFinder.Services;

namespace CommonSubgraphFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = "./../../../Files/k20.csv";
            var graph = GraphFactory.CreateFromCsvFile(filePath);
            var weighted = new WeightedGraph(graph);
            weighted.WeightMatrix[0, 0] = 10;

            var stopwatch = Stopwatch.StartNew();
            var maxClique = MaxCliqueFinder.FindMaxClique(weighted);
            stopwatch.Stop();
            Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}
