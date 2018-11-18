namespace CommonSubgraphFinder.ExampleCreator.Services
{
    public static class PathService
    {
        public static string GetInputFilePath(string fileName) => $"./../../../../CommonSubgraphFinder/Files/Inputs/{fileName}";

        public static string GetResultFilePath(string firstGraphFileName, string secondGraphFileName)
        {
            firstGraphFileName = firstGraphFileName.Replace(".csv", "");
            secondGraphFileName = secondGraphFileName.Replace(".csv", "");
            return $"./../../../Files/Results/{firstGraphFileName}&{secondGraphFileName}.csv";
        }

        public static string GetFileName(int firstGraphVerticesCount, int secondGraphVerticesCount,
            bool isThisNameForFirstGraph)
        {
            var filename = firstGraphVerticesCount <= secondGraphVerticesCount
                ? $"{firstGraphVerticesCount}_{secondGraphVerticesCount}" + (isThisNameForFirstGraph ? "_A" : "_B")
                : $"{secondGraphVerticesCount}_{firstGraphVerticesCount}" + (isThisNameForFirstGraph ? "_B" : "_A");

            filename += "_Drejer.csv";

            return filename;
        }
    }
}