using GeometryLib.Geometry;

namespace TestProject1
{
    public static class GetSampleData
    {
        /// <summary>Создаём список случайных точек в квадрате 0-1</summary>
        /// <param name="count">Количество точек</param>
        /// <returns>Список точек в 0-1</returns>
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
    }
}