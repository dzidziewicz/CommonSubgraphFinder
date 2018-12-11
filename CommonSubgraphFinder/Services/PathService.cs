﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonSubgraphFinder.Services
{
    public static class PathService
    {
        public static string GetInputFilePath(string fileName) => $"./../data/{fileName}";

        public static string GetResultFilePath(string outPutFileName, bool countVerticesOnly, bool useExactAlgorithm)
        {
            var graphWeight = countVerticesOnly ? "V" : "VE";
            var algorithmType = useExactAlgorithm ? "exact" : "approx";
            
            return $"./../data/Results/{outPutFileName}_{algorithmType}_{graphWeight}.csv";
        }
    }
}
