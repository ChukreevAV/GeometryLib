using GeometryLib.Geometry;

namespace GeometryLib
{
    public class EventQueue
    {
        private BinarySearchTree<SweepEvent> Tree { get; } = new();

        public void AddEvent(Point2d p)
        {
            var even = Tree.Find(p);
            if (even != null) return;
            even = new SweepEvent(p);
            Tree.Add(p, even);
        }

        public void AddEvent(Point2d p, Line2d line)
        {
            var even = Tree.Find(p);
            if (even == null)
            {
                even = new SweepEvent(p, line);
                Tree.Add(p, even);
            }
            else
            {
                even.Add(line);
            }
        }

        public void AddEvent(Point2d p, Line2d line1, Line2d line2)
        {
            var even = Tree.Find(p);
            if (even == null)
            {
                even = new SweepEvent(p, line1, line2);
                Tree.Add(p, even);
            }
            else
            {
                even.Add(line1);
                even.Add(line2);
            }
        }

        public void AddEvent(Line2d line)
        {
            AddEvent(line.First(), line);
            AddEvent(line.Last());
        }

        public void AddUpEvent(Line2d line)
        {
            AddEvent(line.First(), line);
        }

        public void AddDownEvent(Line2d line)
        {
            AddEvent(line.Last(), line);
        }

        public SweepEvent? GetNextEvent()
        {
            var nn = Tree.GetRight();
            if (nn == null) return null;
            var ev = nn.Value;
            Tree.Remove(nn.Key);
            return ev;
        }

        public bool IsEmpty() => Tree.IsEmpty();
    }
}