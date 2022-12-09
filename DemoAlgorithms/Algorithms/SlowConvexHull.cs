using GeometryLib.Geometry;

namespace DemoAlgorithms.Algorithms
{
    public class SlowConvexHull : ISlowConvexHull
    {
        public List<Point2d> Points { get; }

        public List<Point2d> SelectPoints { get; } = new();

        public List<Line2d> UnselectLines { get; } = new();

        public List<Line2d> ConvexHull { get; } = new ();

        public int Index1 { get; private set; }

        public int Index2 { get; private set; }

        public bool? Sign { get; private set; }

        public Line2d? CurrentLine { get; private set; }

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

        public SlowConvexHull()
        {
            var points = GetRandomPoint2ds(30);
            points.Sort();
            Points = points;
        }

        public void Next()
        {
            if (Index1 == Index2) Index2++;
            var pCount = Points.Count;
            if (Index2 >= pCount)
            {
                SelectPoints.Add(Points[Index1]);
                Index1++;
                Index2 = 0;
                UnselectLines.Clear();
            }
            if (Index1 >= pCount) return;
            //
            var p1 = Points[Index1];
            var p2 = Points[Index2];
            CurrentLine = new Line2d(p1, p2);
            var bAdd = true;
            foreach (var distance in
                     from p3 in Points
                     where !Equals(p3, p1) && !Equals(p3, p2)
                     select CurrentLine.Distance(p3))
            {
                if (Sign == null)
                {
                    Sign = distance >= 0; //TODO ==0!!!
                }
                else
                {
                    if (distance < 0 && Sign == true ||
                        distance >= 0 && Sign == false)
                    {
                        bAdd = false;
                    }
                }
            }

            if (bAdd) ConvexHull.Add(CurrentLine);
            else UnselectLines.Add(CurrentLine);
            Index2++;
        }
    }
}