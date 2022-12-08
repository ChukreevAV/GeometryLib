using GeometryLib.Geometry;

namespace WebApplication1.Services
{
    public interface ISlowConvexHullService
    {
        List<Point2d> Points { get; }

        List<Point2d> SelectPoints { get; }

        List<Line2d> UnselectLines { get; }

        List<Line2d> ConvexHull { get; }

        int Index1 { get; }

        int Index2 { get; }
        bool? Sign { get; }

        Line2d? CurrentLine { get; }

        void Next();
    }
}