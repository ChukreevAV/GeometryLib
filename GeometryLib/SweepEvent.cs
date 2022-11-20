namespace GeometryLib
{
    public class SweepEvent
    {
        public Point2d Point { get; set; }

        public List<Line2d> Lines { get; } = new();

        public SweepEvent(Point2d p, Line2d line)
        {
            Point = p;
            Lines.Add(line);
        }
    }
}