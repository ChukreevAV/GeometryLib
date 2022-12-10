using GeometryLib.Geometry;

namespace WebApplication2.Models
{
    public class SlowConvexHullState
    {
        public List<Point2d>? Points { get; set; }

        public List<Point2d>? SelectPoints { get; set; }

        public List<Line2d>? UnselectLines { get; set; }

        public List<Line2d>? ConvexHull { get; set; }

        public int Index1 { get; set; }

        public int Index2 { get; set; }

        public bool? Sign { get; set; }

        public Line2d? CurrentLine { get; set; }

        public SlowConvexHullState()
        {
            Points ??= new List<Point2d>();
            SelectPoints ??= new List<Point2d>();
            UnselectLines ??= new List<Line2d>();
            ConvexHull ??= new List<Line2d>();
        }

        public SlowConvexHullState(IEnumerable<Point2d> points)
        {
            Points = new List<Point2d>();
            Points.AddRange(points);
        }
    }
}