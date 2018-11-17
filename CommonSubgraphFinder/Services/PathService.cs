using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonSubgraphFinder.Services
{
    public static class PathService
    {
        public static string GetInputFilePath(string fileName) => $"./../../../Files/Inputs/{fileName}";

        public static string GetResultFilePath(string firstGraphFileName, string secondGraphFileName, bool countVerticesOnly, bool useExactAlgorithm)
        {
            firstGraphFileName = firstGraphFileName.Replace(".csv", "");
            secondGraphFileName = secondGraphFileName.Replace(".csv", "");
            var graphWeight = countVerticesOnly ? "V" : "VE";
            var algorithmType = useExactAlgorithm ? "exact" : "approx";
            
            return $"./../../../Files/Results/{firstGraphFileName}&{secondGraphFileName}_{algorithmType}_{graphWeight}.csv";
        }
    }
}
