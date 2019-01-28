namespace DijkstraTransportGraph
{
    public class Edge : IEdge
    {
        public Edge(Vertex v1, Vertex v2, int weight)
        {
            Weight = weight;
            Vertex1 = v1;
            Vertex2 = v2;
        }

        public int Weight { get; private set; }

        public int Vertex1ID { get { return Vertex1.ID; } }

        public int Vertex2ID { get { return Vertex2.ID; } }

        public Vertex Vertex1 { get; private set; }

        public Vertex Vertex2 { get; private set; }

        public bool Connects(Vertex v1, Vertex v2)
        {
            return (this.Vertex1 == v1 && this.Vertex2 == v2)
                || (this.Vertex1 == v2 && this.Vertex2 == v1);
        }

        public override string ToString()
        {
            return $"{Vertex1.Name} -> {Vertex2.Name} [{Weight}]";
        }
    }
}
