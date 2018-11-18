using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommonSubgraphFinder.MaxClique;
using CommonSubgraphFinder.Models;
using CommonSubgraphFinder.Partitioning;
using CommonSubgraphFinder.Services;

namespace CommonSubgraphFinder
{
    internal class Program
    {
        const string FileName1 = "k30.csv";
        const string FileName2 = "kd4a.csv";
        const bool CountVerticesOnly = false;
        const bool UseExactAlgorithm = true;
        private static void Main(string[] args)
        {
            string filePath1 = PathService.GetInputFilePath(FileName1);
            string filePath2 = PathService.GetInputFilePath(FileName2);
            string resultPath = PathService.GetResultFilePath(FileName1, FileName2, CountVerticesOnly, UseExactAlgorithm);
            
            var g = CsvFilesService.CreateGraphFromCsv(filePath1);
            var h = CsvFilesService.CreateGraphFromCsv(filePath2);

            var modularProduct = CountVerticesOnly
                ? ModularProductService.GetModularProductForVertexMaxGraph(g, h)
                : ModularProductService.GetModularProductForVertexPlusEdgesMaxGraph(g, h);

            var stopwatch = Stopwatch.StartNew();
            HashSet<int> maxClique = new HashSet<int>();
            int maxCliqueWeight = 0;
            if (UseExactAlgorithm)
            {
                maxClique = MaxCliqueFinder.FindMaxClique(modularProduct, CountVerticesOnly);
                maxCliqueWeight = maxClique.Count;
            }
            else
            {
                var subgraphs = GraphPartitioner.PartitionGraph(modularProduct, modularProduct.VerticesCount / (int)Math.Log(modularProduct.VerticesCount));
                foreach (var subgraph in subgraphs)
                {
                    (HashSet<int> clique, int cliqueWeight) =
                        MaxCliqueFinder.FindMaxCliqueWithWeight(subgraph, CountVerticesOnly);
                    if (cliqueWeight > maxCliqueWeight)
                    {
                        maxCliqueWeight = cliqueWeight;
                        maxClique = clique;
                    }
                }
            }

            var mapping = ModularProductService.MapMpToBaseGraphs(g, h, maxClique.ToList());

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
