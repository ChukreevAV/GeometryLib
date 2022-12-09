using DemoAlgorithms.Algorithms;

using GeometryLib.Geometry;

namespace WebApplication2.Models
{
    public class SlowConvexHullState
    {
        public List<Point2d>? points { get; set; }

        public List<Point2d>? selectPoints { get; set; }

        public List<Line2d>? unselectLines { get; set; }

        public List<Line2d>? convexHull { get; set; } 

        public int index1 { get; set; }

        public int index2 { get; set; }

        public bool? sign { get; set; }

        public Line2d? currentLine { get; set; }

        public SlowConvexHullState()
        {
            points ??= new List<Point2d>();
            selectPoints ??= new List<Point2d>();
            unselectLines ??= new List<Line2d>();
            convexHull ??= new List<Line2d>();
        }

        public SlowConvexHullState(IEnumerable<Point2d> points)
        {
            this.points = new List<Point2d>();
            this.points.AddRange(points);
        }

        public SlowConvexHullState(ISlowConvexHull convexHull)
        {
            points = new List<Point2d>();
            points.AddRange(convexHull.Points);
            selectPoints = new List<Point2d>();
            selectPoints.AddRange(convexHull.SelectPoints);
            unselectLines = new List<Line2d>();
            unselectLines.AddRange(convexHull.UnselectLines);
            this.convexHull = new List<Line2d>();
            this.convexHull.AddRange(convexHull.ConvexHull);

            index1 = convexHull.Index1;
            index2 = convexHull.Index2;
            sign = convexHull.Sign;
            currentLine = convexHull.CurrentLine;
        }
    }
}