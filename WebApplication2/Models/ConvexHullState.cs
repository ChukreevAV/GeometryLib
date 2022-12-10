using GeometryLib.Geometry;

using System;

namespace WebApplication2.Models
{
    public class ConvexHullState
    {
        public List<Point2d>? Points { get; set; }

        public List<Point2d>? UpConvexHull { get; set; }

        public List<Point2d>? DownConvexHull { get; set; }

        public int Index1 { get; set; }

        public int Index2 { get; set; }

        public ConvexHullState()
        {
            Points = new List<Point2d>();
            UpConvexHull = new List<Point2d>();
            DownConvexHull = new List<Point2d>();
        }

        public ConvexHullState(List<Point2d> points)
        {
            Points = new List<Point2d>();
            points.Sort();
            Points.AddRange(points);
            Index2 = Points.Count;
            UpConvexHull = new List<Point2d>();
            DownConvexHull = new List<Point2d>();
        }
    }
}