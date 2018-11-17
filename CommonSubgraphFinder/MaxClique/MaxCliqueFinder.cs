using System.Collections.Generic;
using System.Linq;
using CommonSubgraphFinder.Models;

namespace CommonSubgraphFinder.MaxClique
{
    public class MaxCliqueFinder
    {
        private readonly WeightedGraph _graph;
        public HashSet<int> MaxClique { get; private set; }
        public int MaxCliqueWeight { get; private set; }
        private readonly bool _countVerticesOnly;

        public MaxCliqueFinder(WeightedGraph graph, bool countVerticesOnly)
        {
            _graph = graph;
            _countVerticesOnly = countVerticesOnly;
            MaxClique = new HashSet<int>();
        }

        public static HashSet<int> FindMaxClique(WeightedGraph graph, bool countVerticesOnly)
        {
            var finder = new MaxCliqueFinder(graph, countVerticesOnly);
            finder.BronKerbosch(new HashSet<int>(), new HashSet<int>(Enumerable.Range(0, graph.VerticesCount)), new HashSet<int>());

            return finder.MaxClique;
        }

        public void BronKerbosch(HashSet<int> R, HashSet<int> P, HashSet<int> X)
        {
            if (P.Count == 0 && X.Count == 0)
            {
                TryAddMaxClique(R);
            }

            foreach (var vertex in P.ToArray()) // Copying P set needed, because P is modified inside the loop.
            {
                var newR = new HashSet<int>(R);
                newR.UnionWith(new []{vertex});
                var newP = IntersectWithNeighboursOf(vertex, P);
                var newX = IntersectWithNeighboursOf(vertex, X);

                BronKerbosch(newR, newP, newX);

                P.Remove(vertex);
                newX.Add(vertex);
            }
        }


        public HashSet<int> IntersectWithNeighboursOf(int vertex, HashSet<int> set)
        {
            HashSet<int> newSet = new HashSet<int>();
            foreach (var u in set)
            {
                if (_graph.AdjacencyMatrix[u, vertex])
                    newSet.Add(u);
            }

            return newSet;
        }
        private void TryAddMaxClique(HashSet<int> R)
        {
            if (_countVerticesOnly)
            {
                if (R.Count > MaxClique.Count)
                {
                    MaxClique = R;
                    MaxCliqueWeight = MaxClique.Count;
                }
            }
            else
            {
                int maxWeightForNewClique = R.Count + (R.Count * (R.Count - 1) / 2);
                if (MaxCliqueWeight >= maxWeightForNewClique)
                    return; 

                var cliqueWeight = CalculateCliqueWeight(R);
                if (cliqueWeight > MaxCliqueWeight)
                {
                    MaxClique = R;
                    MaxCliqueWeight = cliqueWeight;
                }
            }

        }

        private int CalculateCliqueWeight(HashSet<int> hashSet)
        {
            int weight = hashSet.Count;
            var visited = new List<int>();
            foreach (var u in hashSet)
            {
                foreach (var v in visited)
                {
                    weight += _graph.WeightMatrix[u, v];
                }
                visited.Add(u);
            }

            return weight;
        }
    }
}
