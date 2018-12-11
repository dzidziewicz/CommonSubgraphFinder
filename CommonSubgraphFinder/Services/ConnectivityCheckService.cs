using System.Collections.Generic;
using System.Linq;
using CommonSubgraphFinder.Models;

namespace CommonSubgraphFinder.Services
{
    public class ConnectivityCheckService
    {
        private readonly WeightedGraph _modularProduct;
        private bool[] _visited;
        private List<int> _subgraphVertices;

        public ConnectivityCheckService(WeightedGraph modularProduct)
        {
            _modularProduct = modularProduct;
        }

        public bool IsConnected(HashSet<int> subgraphVertices)
        {
            _subgraphVertices = subgraphVertices.ToList();
            _visited = new bool[_modularProduct.VerticesCount];

            Dfs(_subgraphVertices[0]);
            foreach (var vertex in _subgraphVertices)
            {
                if (!_visited[vertex]) return false;
            }
            return true; 
        }

        private void Dfs(int v)
        {
            if(_visited[v]) return;

            _visited[v] = true;
            foreach (var u in _modularProduct.NeighboursOf(v))
            {
                if (!_subgraphVertices.Contains(u)) continue;           // u is not in found subgraph so we cannot visit it
                if(_modularProduct.WeightMatrix[v, u] == 0) continue;   // (v,u) corrensponds to a non-existent edge in input graph

                Dfs(u);
            }
        }
    }
}
