namespace GeometryLib
{
    public class EventQueue
    {
        private BinarySearchTree<SweepEvent> Tree { get; } = new();

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
                even.Lines.Add(line);
            }
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
            Tree.Remove(nn.Key);
            return nn.Value;
        }
    }
}