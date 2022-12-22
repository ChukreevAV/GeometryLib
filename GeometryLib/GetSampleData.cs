using GeometryLib.ConvexHull;
using GeometryLib.Geometry;

namespace GeometryLib
{
    public static class GetSampleData
    {
        public static List<Point2d> GetRandomPoint2ds(int count)
        {
            var random = new Random();
            var list1 = new List<Point2d>();
            for (var i = 0; i < count; i++)
            {
                list1.Add(new Point2d(
                    random.NextDouble(),
                    random.NextDouble()
                ));
            }

            return list1;
        }

        public static List<Point2d> GetRandomConvexHull(int count)
        {
            var list1 = GetRandomPoint2ds(count);
            list1.Sort();
            return ConvexHullMethods.ConvexHull(list1);
        }

        public static List<Line2d> GetRandomLine2ds(int count)
        {
            var dCount = count * 2;
            var list1 = GetRandomPoint2ds(dCount);
            var lines = new List<Line2d>();
            for (var i = 0; i < dCount - 1; i += 2)
            {
                lines.Add(new Line2d(list1[i], list1[i + 1]));
            }

            return lines;
        }
    }
}