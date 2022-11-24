namespace GeometryLib
{
    public class SweepEvent
    {
        public Point2d Point { get; set; }

        public List<Line2d> Lines { get; } = new();

        public SweepEvent(Point2d p)
        {
            Point = p;
        }

        public SweepEvent(Point2d p, Line2d line)
        {
            Point = p;
            Lines.Add(line);
        }
        
        public SweepEvent(Point2d p, Line2d line1, Line2d line2)
        {
            Point = p;
            Lines.Add(line1);
            Lines.Add(line2);
        }

        public SweepEvent(Point2d p, IEnumerable<Line2d> lines)
        {
            Point = p;
            Lines.AddRange(lines);
        }

        public void Add(Line2d line)
        {
              if (!Lines.Contains(line)) Lines.Add(line);
        }
    }
}