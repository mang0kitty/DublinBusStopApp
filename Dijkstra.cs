using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraTransportGraph
{
    public interface IVertex
    {
        int ID { get; }
    }

    public interface IEdge
    {
        int Weight { get; }
        int Vertex1ID { get; }
        int Vertex2ID { get; }
    }

    public class Dijkstra
    {
        public Dijkstra(IEnumerable<IVertex> vertices, IEnumerable<IEdge> edges)
        {
            Vertices = vertices;
            Edges = edges;
        }

        public IEnumerable<IVertex> Vertices { get; private set; }

        public IEnumerable<IEdge> Edges { get; private set; }

        public IEnumerable<IEdge> Search(IVertex start, IVertex end)
        {
            var searchVertices = BuildAdjacencyLists();

            searchVertices[start.ID].ShortestDistance = 0;

            UpdateDistances(searchVertices);

            return Backtrack(searchVertices, searchVertices[end.ID]);
        }

        private Dictionary<int, SearchVertex> BuildAdjacencyLists()
        {
            var searchVertices = new Dictionary<int, SearchVertex>();
            foreach (var vertex in this.Vertices)
            {
                searchVertices.Add(vertex.ID, new SearchVertex(vertex));
            }

            foreach (var edge in this.Edges)
            {
                var vertex1 = searchVertices[edge.Vertex1ID];
                var vertex2 = searchVertices[edge.Vertex2ID];

                vertex1.Connections.Add(edge);
                vertex2.Connections.Add(edge);
            }

            return searchVertices;
        }

        private void UpdateDistances(Dictionary<int, SearchVertex> vertices)
        {
            while (true)
            {
                var start = vertices.Values                     // Take all of our vertices
                    .Where(vertex => !vertex.Visited)           // Exclude everything that has been visited
                    .OrderBy(vertex => vertex.ShortestDistance) // Sort by the shortest distance
                    .FirstOrDefault();                          // Take the first one
                if (start == default(SearchVertex))
                {
                    return;
                }

                if (start.ShortestDistance == long.MaxValue)
                {
                    return;
                }

                start.Visited = true;

                var edges = start.Connections
                    .Select(edge => new
                    {
                        Other = vertices[edge.Vertex1ID == start.ID ? edge.Vertex2ID : edge.Vertex1ID],
                        Weight = edge.Weight
                    }).Where(edge => !edge.Other.Visited);

                foreach (var edge in edges)
                {
                    edge.Other.ShortestDistance = Math.Min(edge.Other.ShortestDistance, start.ShortestDistance + edge.Weight);
                }
            }
        }

        private IEnumerable<IEdge> Backtrack(Dictionary<int, SearchVertex> vertices, SearchVertex end)
        {
            if (end.ShortestDistance == 0)
            {
                yield break;
            }

            var lastLeg = end.Connections
                .Select(edge => new
                {
                    Other = vertices[edge.Vertex1ID == end.ID ? edge.Vertex2ID : edge.Vertex1ID],
                    Edge = edge
                })
                .Where(edge => edge.Other.ShortestDistance == end.ShortestDistance - edge.Edge.Weight)
                .First();

            foreach (var edge in Backtrack(vertices, lastLeg.Other))
            {
                yield return edge;
            }

            yield return lastLeg.Edge;
        }

        private class SearchVertex : IVertex
        {
            public SearchVertex(IVertex vertex)
            {
                Vertex = vertex;
            }

            public int ID { get { return Vertex.ID; } }

            public IVertex Vertex { get; private set; }

            public long ShortestDistance { get; set; } = long.MaxValue;

            public bool Visited { get; set; } = false;

            public List<IEdge> Connections { get; private set; } = new List<IEdge>();
        }


    }
}