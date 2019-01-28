namespace DijkstraTransportGraph
{
    public class Vertex : IVertex
    {
        public Vertex(int stopId, string name, double lat, double lng)
        {
            StopId = stopId;
            Name = name;
            Latitude = lat;
            Longitude = lng;
        }

        public int ID { get { return StopId; } }

        public int StopId { get; private set; }

        public string Name { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }
    }
}