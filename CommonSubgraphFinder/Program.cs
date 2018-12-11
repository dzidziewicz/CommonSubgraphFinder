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
        private static void Main(string[] args)
        {
            Console.WriteLine("Provide name of file with first graph (with extension) : ");
            string f1 = Console.ReadLine();
            Console.WriteLine("Provide name of file with second graph (with extension) :  ");
            string f2= Console.ReadLine();
            string FileName1 = f1;
            string FileName2 = f2;
            string OutputFileName = f1.Split('.')[0] + "_AND_" + f2.Split('.')[0];
            bool CountVerticesOnly = true;
            Console.WriteLine("Count vertices only (otherwise vertices and edges)? [y/N]");
            string answer1 = Console.ReadLine();
            if (answer1 == "n" || answer1 == "N")
                CountVerticesOnly = false;

            bool UseExactAlgorithm = true;
            Console.WriteLine("Use exact algorithm (otherwise aproximate)? [y/N]");
            string answer2 = Console.ReadLine();
            if (answer2 == "n" || answer2 == "N")
                UseExactAlgorithm = false;

            string filePath1 = PathService.GetInputFilePath(FileName1);
            string filePath2 = PathService.GetInputFilePath(FileName2);
            string resultPath = PathService.GetResultFilePath(OutputFileName, CountVerticesOnly, UseExactAlgorithm);
            
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
                // Console.WriteLine(modularProduct.VerticesCount / (int)Math.Log(modularProduct.VerticesCount));
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
            ExportResults(g, h, mapping);
            Console.ReadKey();
        }

        private static void ExportResults(Graph g, Graph h, CommonSubgraphMapping mapping)
        {
            var input = new VisualizerInput(g, h, mapping);
            VisualizerService.ExportToFile(input);
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
