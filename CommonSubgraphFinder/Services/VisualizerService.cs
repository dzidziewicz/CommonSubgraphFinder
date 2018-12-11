using System.IO;
using CommonSubgraphFinder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CommonSubgraphFinder.Services
{
    public class VisualizerService
    {
        private const string OutputPath = "./Visualizer/data.js";
        public static void ExportToFile(VisualizerInput input)
        {
            var json = JsonConvert.SerializeObject(input,
                new JsonSerializerSettings() {ContractResolver = new CamelCasePropertyNamesContractResolver()});

            var result = $"var DATA = {json}";

            File.WriteAllText(OutputPath, result);
        }
    }
}
