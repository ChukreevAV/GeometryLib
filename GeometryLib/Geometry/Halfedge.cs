using static System.Net.Mime.MediaTypeNames;

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
            newHalfEdge.Prev = this;

            var newTwinHalfEdge = new HalfEdge(v, Twin.Face);
            newTwinHalfEdge.Prev = Twin;
            newTwinHalfEdge.Next = Twin.Next;
            Twin.Next.Prev = newTwinHalfEdge;
            Twin.Next = newTwinHalfEdge;

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

            if (ccw < 0)
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

        /// <summary>
        /// Counterclockwise > 0
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="v"></param>
        public List<HalfEdge> OldDivide(HalfEdge edge, Vertex v)
        {
            var rList = new List<HalfEdge>();
            var ccw = Point2d.Counterclockwise(Origin.Point, v.Point, edge.End.Point);

            var nextEdge = new HalfEdge(v, Face);
            var nextTwinEdge = new HalfEdge(v, Twin.Face);
            rList.Add(nextEdge);
            //rList.Add(nextTwinEdge);
            nextEdge.SetTwin(Twin);
            nextEdge.AddNext(Next);
            SetTwin(nextTwinEdge);
            AddPrev(Prev);

            if (ccw < 0)
            {
                var rightEdge = new HalfEdge(v, edge.Face);
                var rightTwinEdge = new HalfEdge(v, edge.Twin.Face);
                rList.Add(rightEdge);
                //rList.Add(rightTwinEdge);
                rightEdge.SetTwin(edge.Twin);
                edge.SetTwin(rightTwinEdge);
                edge.AddPrev(edge.Prev);
                rightEdge.AddNext(edge.Next);
                nextEdge.AddPrev(edge);
                rightEdge.AddPrev(this);
            }
            else
            {
                var leftEdge = new HalfEdge(v, edge.Face);
                var leftTwinEdge = new HalfEdge(v, edge.Twin.Face);
                rList.Add(leftEdge);

                //rList.Add(leftTwinEdge);
                leftEdge.SetTwin(edge.Twin);
                edge.SetTwin(leftTwinEdge);
                edge.AddPrev(edge.Prev);
                leftEdge.AddNext(edge.Next);
                nextEdge.AddPrev(edge);
                leftEdge.AddPrev(this);
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

        public Vertex End => Twin.Origin;

        public bool IsEquals(Vertex sv, Vertex ev)
            => Origin == sv && End == ev;
    }
}