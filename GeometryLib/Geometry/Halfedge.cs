namespace GeometryLib.Geometry
{
    public class HalfEdge
    {
        public Vertex Origin { get; set; }

        public HalfEdge Twin { get; set; }

        public HalfEdge? Next { get; set; }

        public HalfEdge? Prev { get; set; }

        public Face? Face { get; set; }

        private HalfEdge(Vertex origin)
        {
            Origin = origin;
            Twin = this;
        }

        public HalfEdge(Vertex origin, Vertex end)
        {
            Origin = origin;
            Twin = new HalfEdge(end) { Twin = this };
        }

        private HalfEdge(Vertex origin,  Face? face)
        {
            Origin = origin;
            Face = face;
            Twin = this;
        }

        public HalfEdge(Vertex origin, Vertex end, Face? face)
        {
            Origin = origin;
            Twin = new HalfEdge(end) { Twin = this };
            Face = face;
        }

        public void AddPrev(HalfEdge? prev)
        {
            Prev = prev;
            if (prev == null) return;
            prev.Next = this;

            Twin.Next = prev.Twin;
            prev.Twin.Prev = Twin;
        }

        public void AddNext(HalfEdge? next)
        {
            Next = next;
            if (next == null) return;
            next.Prev = this;

            Twin.Prev = next.Twin;
            next.Twin.Next = Twin;
        }

        public void SetTwin(HalfEdge edge)
        {
            Twin = edge;
            edge.Twin = this;
        }

        public void Divide(Vertex v)
        {
            if (Twin == null) throw new ArgumentException("Twin is null");

            var newHalfEdge = new HalfEdge(v, Face);
            newHalfEdge.SetTwin(Twin);
            newHalfEdge.AddNext(Next);
            AddNext(newHalfEdge);

            var newTwinHalfEdge = new HalfEdge(v, Twin.Face);
            newTwinHalfEdge.SetTwin(this);
            newTwinHalfEdge.AddNext(Prev?.Twin);
        }

        /// <summary>
        /// Counterclockwise > 0
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="v"></param>
        public void Divide(HalfEdge edge, Vertex v)
        {
            var ccw = Point2d.Counterclockwise(Origin.Point, v.Point, edge.End.Point);

            var nextEdge = new HalfEdge(v, Face);
            var nextTwinEdge = new HalfEdge(v, Twin.Face);
            nextEdge.SetTwin(Twin);
            nextEdge.AddNext(Next);
            SetTwin(nextTwinEdge);
            AddPrev(Prev);

            if (ccw < 0)
            {
                var rightEdge = new HalfEdge(v, edge.Face);
                var rightTwinEdge = new HalfEdge(v, edge.Twin.Face);
                rightEdge.SetTwin(edge.Twin);
                edge.SetTwin(rightTwinEdge);
                edge.AddPrev(edge.Prev);

                nextEdge.AddPrev(edge);
                rightEdge.AddNext(edge.Next);
                rightEdge.AddPrev(this);
            }
            else
            {
                var leftEdge = new HalfEdge(v, edge.Face);
                var leftTwinEdge = new HalfEdge(v, edge.Twin.Face);
                leftEdge.SetTwin(edge.Twin);
                edge.SetTwin(leftTwinEdge);
                edge.AddPrev(edge.Prev);

                nextEdge.AddPrev(edge);
                leftEdge.AddNext(edge.Next);
                leftEdge.AddPrev(this);
            }
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

        public Vertex End => Twin.Origin;

        public bool IsEquals(Vertex sv, Vertex ev) 
            => Origin == sv && End == ev;
    }
}