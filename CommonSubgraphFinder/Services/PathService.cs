using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonSubgraphFinder.Services
{
    public static class PathService
    {
        public static string GetInputFilePath(string fileName) => $"./../data/Inputs/{fileName}";

        public static string GetResultFilePath(string sizes, bool countVerticesOnly, bool useExactAlgorithm)
        {
            var graphWeight = countVerticesOnly ? "V" : "VE";
            var algorithmType = useExactAlgorithm ? "exact" : "approx";
            
            return $"./../data/Results/{sizes}_{algorithmType}_{graphWeight}.csv";
        }
    }
}
