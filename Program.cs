using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DijkstraTransportGraph
{
    class Program
    {
        /* public static IEnumerable<int> CountUpTo(int from)
        {
            Console.WriteLine($"Entered CountUpTo({from})");

            if (from == 0)
            {
                Console.WriteLine($"Breaking in CountUpTo({from})");
                yield break;
            }

            foreach (var count in CountUpTo(from - 1))
            {
                Console.WriteLine($"Returning {count} in CountUpTo({from})");
                yield return count;
            }

            Console.WriteLine($"Returning {from} in CountUpTo({from})");
            yield return from;
        }*/

        static void Main(string[] args)
        {
            var loader = new Loader();
            var vertices = loader.LoadVertices().ToArray();
            Console.WriteLine($"We read in {vertices.Length} vertices.");

            var edges = loader.LoadEdges(vertices).ToArray();

            Console.WriteLine($"We read in {edges.Length} edges.");

            Console.WriteLine($"Here's the first edge: {edges[0]}");

            if (edges.Any(edge => edges.Any(edge2 => edge.Vertex1 == edge2.Vertex2 && edge.Vertex2 == edge2.Vertex1)))
            {
                Console.WriteLine("This is a directed graph");
            }
            else
            {
                Console.WriteLine("This is an undirected graph");
            }

            var djk = new Dijkstra(vertices, edges);

            var start = vertices[0];
            var end = vertices[5];

            var path = djk.Search(start, end);
            Console.WriteLine($"Total Distance from {start.Name} to {end.Name}: {path.Aggregate(0, (dist, edge) => dist + edge.Weight)}m");

            var current = start;
            foreach (var edge in path)
            {
                Console.Write($"{current.Name} -> ");
                current = vertices.First(v => v.ID == (edge.Vertex1ID == current.ID ? edge.Vertex2ID : edge.Vertex1ID));
                // }
                Console.WriteLine(current.Name);

                // var countdown = Countdown(5);
                // Console.WriteLine(countdown.Take(2).Select(num => num.ToString()).Aggregate((x, y) => x + "," + y));
                // Console.WriteLine(countdown.Take(2).Select(num => num.ToString()).Aggregate((x, y) => x + "," + y));

                //Console.WriteLine(CountUpTo(3).Select(x => x.ToString()).Aggregate((x, y) => $"{x}, {y}"));
            }
        }
    }
}
