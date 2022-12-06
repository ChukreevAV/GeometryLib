using GeometryLib.Geometry;

namespace WebApplication1.Services
{
    public class DataService : IDataService
    {
        public List<Point2d> Points { get; set; } = new();

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

        public List<Point2d> GetPoints()
        {
            if (!Points.Any())
                Points.AddRange(GetRandomPoint2ds(20));

            return Points;
        }

        public List<Point2d> GetNewPoints()
        {
            Points.Clear();
            Points.AddRange(GetRandomPoint2ds(20));
            return Points;
        }
    }
}