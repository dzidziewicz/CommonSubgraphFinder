using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace CommonSubgraphFinder.Services
{
    public static class CsvWriterService
    {
        public static void WriteToFile(string filePath, IEnumerable<int> gVertices, IEnumerable<int> hVertices)
        {
            using (var csv = new CsvWriter(new StreamWriter(File.OpenWrite(filePath))))
            {
                foreach (var v in gVertices)
                {
                    csv.WriteField(v);
                }
                csv.NextRecord();
                foreach (var v in hVertices)
                {
                    csv.WriteField(v);
                }
                csv.NextRecord();
            }
        }
    }
}
