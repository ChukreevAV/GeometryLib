namespace GeometryLib.Geometry
{
    public class Subdivision
    {
        public List<Face> Faces { get; } = new ();

        public List<HalfEdge> Edges { get; } = new();

        public List<Vertex> Vertices { get; } = new();

        private Vertex AddVertex(Point2d point)
        {
            var v = Vertices.FirstOrDefault(v => v.IsEquals(point));
            if (v == null)
            {
                v = new Vertex(point);
                Vertices.Add(v);
            }
            return v;
        }

        private HalfEdge AddEdge(Vertex sv, Vertex ev, Face face)
        {
            var sEdge = new HalfEdge(sv, face);
            var eEdge = new HalfEdge(ev, face);

            sEdge.Twin = eEdge;
            eEdge.Twin = sEdge;

            Edges.Add(sEdge);
            Edges.Add(eEdge);

            return sEdge;
        }

        public Face? AddFace(List<Point2d> points)
        {
            var face = new Face();
            Vertex? sv = null;
            Vertex? prevVertex = null;
            HalfEdge? startHalfEdge = null;
            HalfEdge? prevHalfEdge = null;
            HalfEdge newHalfEdge;

            foreach (var p in points)
            {
                var v = AddVertex(p);
                if (sv == null) sv = v;
                else
                {
                    if (prevVertex == null) 
                        throw new NullReferenceException("prevVertex == null");

                    newHalfEdge = AddEdge(prevVertex, v, face);
                    newHalfEdge.AddPrev(prevHalfEdge);
                    if (startHalfEdge == null) startHalfEdge = newHalfEdge;
                    prevHalfEdge = newHalfEdge;
                }

                prevVertex = v;
            }

            if (prevVertex == null || sv == null)
                throw new NullReferenceException("prevVertex == null || sv == null");

            newHalfEdge = AddEdge(prevVertex, sv, face);
            newHalfEdge.AddPrev(prevHalfEdge);
            newHalfEdge.AddNext(startHalfEdge);

            face.Outher = newHalfEdge;

            return face;
        }
    }
}