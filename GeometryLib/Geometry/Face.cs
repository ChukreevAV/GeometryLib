namespace GeometryLib.Geometry
{
    public class Face
    {
        public HalfEdge? Outher { get; set; }

        public List<HalfEdge> Inner { get; set; } = new();

        public List<HalfEdge> GetEdges()
        {
            var fistEdge = Outher;
            var workEdge = Outher.Next;

            var edges = new List<HalfEdge> { fistEdge , workEdge };

            while (workEdge != fistEdge)
            {
                workEdge = workEdge.Next;
                edges.Add(workEdge);
            }
            return edges;
        }

        public void Reverse()
        {
            Outher?.Reverse();
            Outher = Outher?.Twin;
        }
    }
}