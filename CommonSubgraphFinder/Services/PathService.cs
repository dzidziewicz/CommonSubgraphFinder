using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonSubgraphFinder.Services
{
    public static class PathService
    {
        public static string GetInputFilePath(string fileName) => $"./../../../Files/Inputs/{fileName}";

        public static string GetResultFilePath(string firstGraphFileName, string secondGraphFileName)
        {
            firstGraphFileName = firstGraphFileName.Replace(".csv", "");
            secondGraphFileName = secondGraphFileName.Replace(".csv", "");
            return $"./../../../Files/Results/{firstGraphFileName}&{secondGraphFileName}.csv";
        }
    }
}
