using System;
using CommonSubgraphFinder.Models;
using CommonSubgraphFinder.Services;

namespace CommonSubgraphFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = "./../../../simpleGraph.csv";
            var graph = GraphFactory.CreateFromCsvFile(filePath);
            var weighted = new WeightedGraph(graph);
            weighted.WeightMatrix[0, 0] = 10;
            Console.ReadKey();
        }
    }
}
