using System;
using CommonSubgraphFinder.Models;

namespace CommonSubgraphFinder.ExampleCreator.Services
{
    public static class GraphBuilder
    {
        public static Graph Build()
        {
            Console.WriteLine("Type vertices count");
            Console.WriteLine();
            var verticesCount = int.Parse(Console.ReadLine());
            var graph = new Graph(verticesCount);
            Console.WriteLine();
            Console.WriteLine("Type edges using format 'u v' with a single space between vertex indices, one edge per line");
            Console.WriteLine("Finish adding edges by pressing ^Z");
            Console.WriteLine("Graph is undirected, no need to type 'u v' and 'v u'");
            Console.WriteLine();

            string input;
            while (!string.IsNullOrEmpty(input = Console.ReadLine()))
            {
                var vertices = input.Split(' ');
                var v1 = int.Parse(vertices[0]);
                var v2 = int.Parse(vertices[1]);
                graph.AddEdge(v1, v2);
                graph.AddEdge(v2, v1);
            }

            return graph;
        }
    }
}