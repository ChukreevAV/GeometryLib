namespace GeometryLib
{
    public class SweepEvent
    {
        public Point2d Point { get; set; }

        public List<Line2d> Lines { get; } = new();
    }
}