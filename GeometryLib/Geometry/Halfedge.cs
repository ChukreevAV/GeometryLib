using static System.Net.Mime.MediaTypeNames;

namespace GeometryLib.Geometry
{
    public class HalfEdge
    {
        public Vertex Origin { get; set; }

        public HalfEdge? Twin { get; set; }

        public HalfEdge? Next { get; set; }

        public HalfEdge? Prev { get; set; }

        public Face? Face { get; set; }

        public HalfEdge(Vertex origin)
        {
            Origin = origin;
        }

        public HalfEdge(Vertex origin, Face face)
        {
            Origin = origin;
            Face = face;
        }

        public void AddPrev(HalfEdge? prev)
        {
            if (Twin == null) return;
            Prev = prev;
            if (prev == null) return;
            prev.Next = this;

            Twin.Next = prev.Twin;
            if (prev.Twin != null) prev.Twin.Prev = Twin;
        }

        public void AddNext(HalfEdge? next)
        {
            if (Twin == null) return;
            Next = next;
            if (next == null) return;
            next.Prev = this;

            Twin.Prev = next.Twin;
            if (next.Twin != null) next.Twin.Next = Twin;
        }

        private void SelfReverse()
        {
            if (Prev == null || Next == null || Twin == null) return;
            (Twin.Face, Face) = (Face, Twin.Face);
        }

        public void Reverse()
        {
            if (Prev == null || Next == null || Twin == null) return;
            var next = Next;
            Next = null;
            (Twin.Face, Face) = (Face, Twin.Face);
            next.SelfReverse();
            Next = next;
        }

        public Vertex? End => Twin?.Origin;
    }
}