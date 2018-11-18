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
        const string FileName1 = "7_10_A_Drejer.csv";
        const string FileName2 = "7_10_B_Drejer.csv";
        const bool CountVerticesOnly = true;
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
            int[] maxCliqueMapping = null;
            int maxCliqueWeight = 0;
            if (UseExactAlgorithm)
            {
                maxClique = MaxCliqueFinder.FindMaxClique(modularProduct, CountVerticesOnly);
                maxCliqueWeight = maxClique.Count;
            }
            else
            {
                Console.WriteLine(modularProduct.VerticesCount / (int)Math.Log(modularProduct.VerticesCount));
                var (subgraphs, subgraphsMappings) = GraphPartitioner.PartitionGraph(modularProduct, modularProduct.VerticesCount / (int)Math.Log(modularProduct.VerticesCount));
                for(int i = 0; i < subgraphs.Count; i++)
                {
                    (HashSet<int> clique, int cliqueWeight) =
                        MaxCliqueFinder.FindMaxCliqueWithWeight(subgraphs[i], CountVerticesOnly);
                    if (cliqueWeight > maxCliqueWeight)
                    {
                        maxCliqueWeight = cliqueWeight;
                        maxClique = clique;
                        maxCliqueMapping = subgraphsMappings[i];
                    }
                }
            }

            var parsedClique = UseExactAlgorithm ? maxClique.ToList() : maxClique.ToList().Select(subgraphIndex => maxCliqueMapping[subgraphIndex]);

            var mapping = ModularProductService.MapMpToBaseGraphs(g, h, parsedClique);

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
