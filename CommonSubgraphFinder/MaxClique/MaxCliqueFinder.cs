using System.Collections.Generic;
using System.Linq;
using CommonSubgraphFinder.Models;

namespace CommonSubgraphFinder.MaxClique
{
    public class MaxCliqueFinder
    {
        private readonly WeightedGraph _graph;
        public HashSet<int> MaxClique { get; private set; }

        public MaxCliqueFinder(WeightedGraph graph)
        {
            _graph = graph;
            MaxClique = new HashSet<int>();
        }

        public static HashSet<int> FindMaxClique(WeightedGraph graph)
        {
            var finder = new MaxCliqueFinder(graph);
            finder.BronKerbosch(new HashSet<int>(), new HashSet<int>(Enumerable.Range(0, graph.VerticesCount)), new HashSet<int>());

            return finder.MaxClique;
        }

        public void BronKerbosch(HashSet<int> R, HashSet<int> P, HashSet<int> X)
        {
            if (P.Count == 0 && X.Count == 0)
            {
                if (R.Count > MaxClique.Count)
                {
                    MaxClique = R;
                }
            }

            foreach (var vertex in P.ToList())
            {
                var newR = new HashSet<int>(R);
                newR.UnionWith(new []{vertex});
                var newP = new HashSet<int>(P);
                var neighbours = _graph.NeighboursOf(vertex).ToList();
                newP.IntersectWith(neighbours);
                var newX = new HashSet<int>(X);
                newX.IntersectWith(neighbours);

                BronKerbosch(newR, newP, newX);

                P.Remove(vertex);
                newX.UnionWith(new []{vertex});
            }
        }
    }
}
