using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonSubgraphFinder.Models
{
    public class VisualizerInput
    {
        public int[] Graph1 { get; set; }
        public int[] Graph2 { get; set; }
        public int Graph1Count { get; set; }
        public int Graph2Count { get; set; }
        public int[] Mapping1 { get; set; }
        public int[] Mapping2 { get; set; }

        public VisualizerInput(Graph graph1, Graph graph2, CommonSubgraphMapping mapping)
        {
            Graph1 = EnumerateMatrix(graph1.AdjacencyMatrix).ToArray();
            Graph2 = EnumerateMatrix(graph2.AdjacencyMatrix).ToArray();
            Graph1Count = graph1.VerticesCount;
            Graph2Count = graph2.VerticesCount;
            Mapping1 = mapping.FirstGraphVertices.ToArray();
            Mapping2 = mapping.SecondGraphVertices.ToArray();
        }



        private IEnumerable<int> EnumerateMatrix(bool[,] array)
        {
            foreach (var i in array)
            {
                yield return i ? 1 : 0;
            }
        }
    }
}
