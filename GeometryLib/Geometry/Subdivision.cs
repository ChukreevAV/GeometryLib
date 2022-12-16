using GeometryLib.Intersections;

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
            var oEdge = Edges.FirstOrDefault(e => e.IsEquals(sv, ev));

            if (oEdge != null)
            {
                oEdge.Face = face;
                return oEdge;
            }

            var sEdge = new HalfEdge(sv, ev, face);
            //var eEdge = new HalfEdge(ev, face);

            //sEdge.Twin = eEdge;
            //eEdge.Twin = sEdge;

            Edges.Add(sEdge);
            //Edges.Add(eEdge);

            return sEdge;
        }

        public Face AddFace(List<Point2d> points)
        {
            var face = new Face();
            Faces.Add(face);

            Vertex? sv = null;
            Vertex? prevVertex = null;
            HalfEdge? startHalfEdge = null;
            HalfEdge? prevHalfEdge = null;
            HalfEdge newHalfEdge;

            foreach (var v in points.Select(AddVertex))
            {
                if (sv == null) sv = v;
                else
                {
                    if (prevVertex == null) 
                        throw new NullReferenceException("prevVertex == null");

                    newHalfEdge = AddEdge(prevVertex, v, face);
                    newHalfEdge.AddPrev(prevHalfEdge);
                    startHalfEdge ??= newHalfEdge;
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

        private HalfEdge Copy(HalfEdge old, Face newFace)
        {
            var origin = Vertices.Find(v => v.IsEquals(old.Origin.Point));

            if (old.End == null)
                throw new Exception("old.End == null");

            var end = Vertices.Find(v => v.IsEquals(old.End.Point));

            if (origin == null || end == null) 
                throw new Exception("origin == null || end == null");

            return AddEdge(origin, end, newFace);
        }

        private void Copy(Face oldFace, Subdivision sb)
        {
            var nFace = new Face();
            sb.Faces.Add(nFace);

            var fistEdge = oldFace.Outher; //TODO Add Inner
            var workEdge = fistEdge?.Next;

            if (fistEdge == null || workEdge == null)
                throw new Exception("fistEdge == null || nextEdge == null");

            var fEdge = sb.Copy(fistEdge, nFace);
            nFace.Outher = fEdge;
            var wEdge = sb.Copy(workEdge, nFace);
            fEdge.AddNext(wEdge);

            while (workEdge != fistEdge)
            {
                var nextEdge = workEdge.Next;

                if (nextEdge == null) 
                    throw new Exception("nextEdge == null");

                var nEdge = sb.Copy(nextEdge, nFace);

                wEdge.AddNext(nEdge);
                wEdge = nEdge;
                workEdge = nextEdge;
            }

            wEdge.AddNext(fEdge);

            //return nFace;
        }

        public Subdivision Copy()
        {
            var sb = new Subdivision();

            foreach (var ver in Vertices)
            {
                sb.AddVertex(ver.Point);
            }

            foreach (var face in Faces)
            {
                Copy(face, sb);
            }

            return sb;
        }

        public void Add(Subdivision sd)
        {
            foreach (var ver in sd.Vertices)
            {
                AddVertex(ver.Point);
            }

            foreach (var face in sd.Faces)
            {
                Copy(face, this);
            }
        }

        public List<IEventLine> GetLines()
        {
            return Edges.Select(e => new EdgeLine(e)).ToList<IEventLine>();
        }
    }
}