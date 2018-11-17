using System.Collections.Generic;
using CommonSubgraphFinder.Models;

namespace CommonSubgraphFinder.Services
{
    public static class ModularProductService
    {
        /// <summary>
        /// Returns modular product of graphs g and h. The returned graph is supposed to
        /// be used to find their maximum common subgraph, while maximal graph is defined
        /// as the one with maximal vertices count.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static WeightedGraph GetModularProductForVertexMaxGraph(Graph g, Graph h)
        {
            return GetModularProductGraph(g, h, 0, 0, 1);
        }

        /// <summary>
        /// Returns modular product of graphs g and h. The returned graph is supposed to
        /// be used to find their maximum common subgraph, while maximal graph is defined
        /// as the one with maximal sum of vertices and edges count.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static WeightedGraph GetModularProductForVertexPlusEdgesMaxGraph(Graph g, Graph h)
        {
            return GetModularProductGraph(g, h, 1, 0, 1);
        }

        public static WeightedGraph GetModularProductGraph(Graph g, Graph h, int edgeIsInBothGraphsWeight,
            int edgeIsInNeitherGraphWeight, int vertexWeight)
        {
            var mpVerticesCount = g.VerticesCount * h.VerticesCount;

            var pairIndex1 = 0;
            var modularProduct = new WeightedGraph(mpVerticesCount);

            for (var u = 0; u < g.VerticesCount; u++)
            {
                for (var v = 0; v < h.VerticesCount; v++, pairIndex1++)
                {
                    var pairIndex2 = 0;
                    for (var uPrim = 0; uPrim < g.VerticesCount; uPrim++)
                    {
                        for (var vPrim = 0; vPrim < h.VerticesCount; vPrim++, pairIndex2++)
                        {
                            if(u == uPrim || v == vPrim) continue;

                            if (g.HasEdge(u, uPrim) && h.HasEdge(v, vPrim))
                                modularProduct.AddEdge(pairIndex1, pairIndex2, edgeIsInBothGraphsWeight);

                            else if (!g.HasEdge(u, uPrim) && !h.HasEdge(v, vPrim))
                                modularProduct.AddEdge(pairIndex1, pairIndex2, edgeIsInNeitherGraphWeight);
                        }
                    }
                }
            }

            // set weight for every vertex
            for (var v = 0; v < modularProduct.VerticesCount; v++)
            {
                modularProduct.SetVertexWeight(v, vertexWeight);
            }

            return modularProduct;
        }

        public static CommonSubgraphMapping MapMpToBaseGraphs(Graph g, Graph h, IEnumerable<int> mpVertices)
        {
            var gVertices = new List<int>();
            var hVertices = new List<int>();
            foreach (var mpVertex in mpVertices)
            {
                gVertices.Add(mpVertex / g.VerticesCount);
                hVertices.Add(mpVertex % g.VerticesCount);
            }

            return new CommonSubgraphMapping()
            {
                FirstGraphVertices = gVertices,
                SecondGraphVertices = hVertices
            };
        }
    }
}
