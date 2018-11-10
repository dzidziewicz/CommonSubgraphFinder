using System;

namespace CommonSubgraphFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = "./../../../simpleGraph.csv";
            var graph = new Graph.Graph(filePath);

            Console.ReadKey();
        }
    }
}
