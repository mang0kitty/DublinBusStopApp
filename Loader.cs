using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DijkstraTransportGraph
{
    public class Loader
    {
        public IEnumerable<Vertex> LoadVertices()
        {
            var rows = LoadRows("vertices.csv");
            var headers = rows.First();
            var data = rows.Skip(1);

            foreach (var row in data)
            {
                string stopId = "", name = "", latitude = "", longitude = "";
                for (var i = 0; i < row.Length; i++)
                {
                    switch (headers[i])
                    {
                        case "StopId":
                            stopId = row[i];
                            break;
                        case "Name":
                            name = row[i];
                            break;
                        case "Latitude":
                            latitude = row[i];
                            break;
                        case "Longitude":
                            longitude = row[i];
                            break;
                        default:
                            throw new Exception($"Unknown field found: {headers[i]}");
                    }
                }

                yield return new Vertex(
                    Convert.ToInt32(stopId),
                    name,
                    Convert.ToDouble(latitude),
                    Convert.ToDouble(longitude)
                );
            }
        }

        public IEnumerable<Edge> LoadEdges(IEnumerable<Vertex> vertices)
        {
            var vertexLookup = new Dictionary<int, Vertex>();
            foreach (var vertex in vertices)
            {
                vertexLookup[vertex.StopId] = vertex;
            }

            var rows = LoadRows("edges.csv");

            var headers = rows.First();
            var data = rows.Skip(1);

            foreach (var row in data)
            {
                string vertex1 = "", vertex2 = "", weight = "";
                for (var i = 0; i < row.Length; i++)
                {
                    switch (headers[i])
                    {
                        case "vertex1":
                            vertex1 = row[i];
                            break;
                        case "vertex2":
                            vertex2 = row[i];
                            break;
                        case "weight":
                            weight = row[i];
                            break;
                        default:
                            throw new Exception($"Unknown field found: {headers[i]}");
                    }
                }

                yield return new Edge(
                    vertexLookup[Convert.ToInt32(vertex1)],
                    vertexLookup[Convert.ToInt32(vertex2)],
                    Convert.ToInt32(weight)
                );
            }
        }

        private IEnumerable<string[]> LoadRows(string fileName)
        {
            using (var file = new StreamReader(fileName))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    var fields = ReadFields(line);
                    yield return fields;
                }
            }
        }

        private string[] ReadFields(string line)
        {
            var fields = new List<string>();
            var sb = new StringBuilder();
            var inQuotedField = false;
            foreach (var c in line)
            {
                if (c == ',' && !inQuotedField)
                {
                    fields.Add(sb.ToString());
                    sb.Clear();
                }
                else if (c == '"')
                {
                    inQuotedField = !inQuotedField;
                }
                else
                {
                    sb.Append(c);
                }
            }

            fields.Add(sb.ToString());

            return fields.ToArray();
        }
    }
}