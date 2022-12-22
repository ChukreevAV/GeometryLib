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

        private HalfEdge(Vertex origin, Face? face)
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

            newHalfEdge.Next = Next;
            Next.Prev = newHalfEdge;
            Next = newHalfEdge;
            newHalfEdge.Twin = Twin;
            Twin.Twin = newHalfEdge;
            newHalfEdge.Prev = this;

            var newTwinHalfEdge = new HalfEdge(v, Twin.Face);
            newTwinHalfEdge.Prev = Twin;
            newTwinHalfEdge.Next = Twin.Next;
            Twin.Next.Prev = newTwinHalfEdge;
            Twin.Next = newTwinHalfEdge;

            newTwinHalfEdge.Twin = this;
            Twin = newTwinHalfEdge;
        }

        public List<HalfEdge> Divide(HalfEdge edge, Vertex v)
        {
            var rList = new List<HalfEdge>();
            var ccw = Point2d.Counterclockwise(Origin.Point, v.Point, edge.End.Point);
            Divide(v);
            rList.Add(Next);
            var nextEdge = Next;

            edge.Divide(v);
            rList.Add(edge.Next);

            if (ccw > 0)
            {
                var rightEdge = edge.Next;

                Next = rightEdge;
                rightEdge.Prev = this;

                Twin.Prev = edge;
                edge.Next = Twin;

                nextEdge.Prev = rightEdge.Twin;
                rightEdge.Twin.Next = nextEdge;

                nextEdge.Twin.Next = edge.Twin;
                edge.Twin.Prev = nextEdge.Twin;
            }
            else
            {
                var leftEdge = edge.Next;

                Next = edge.Twin;
                edge.Twin.Prev = this;

                Twin.Prev = leftEdge.Twin;
                leftEdge.Twin.Next = Twin;

                nextEdge.Prev = edge;
                edge.Next = nextEdge;

                nextEdge.Twin.Next = leftEdge;
                leftEdge.Prev = nextEdge.Twin;
            }

            return rList;
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

        public List<HalfEdge> GetLoop()
        {
            var loopList = new List<HalfEdge> { this };
            var nextEdge = Next;
            while (nextEdge != this)
            {
                if (nextEdge == null) return loopList;
                loopList.Add(nextEdge);
                nextEdge = nextEdge.Next;
            }

            return loopList;
        }

        public HalfEdge GetLeftEdge()
        {
            var minEdge = this;
            var nextEdge = Next;
            while (nextEdge != this)
            {
                if (nextEdge == null) return minEdge;
                if (nextEdge.Origin.Point > minEdge.Origin.Point) 
                    minEdge = nextEdge;

                nextEdge = nextEdge.Next;
            }

            return minEdge;
        }

        public bool IsCounterclockwise
             => Point2d.Counterclockwise(Origin.Point, End.Point, Next.End.Point) > 0;

        public double GetAngle() 
            => Point2d.GetAngle(Origin.Point, End.Point, Next.End.Point);

        public bool IsOuther() => Math.Abs(GetLeftEdge().GetAngle()) < Math.PI;

        public Vertex End => Twin.Origin;

        public bool IsEquals(Vertex sv, Vertex ev)
            => Origin == sv && End == ev;
    }
}