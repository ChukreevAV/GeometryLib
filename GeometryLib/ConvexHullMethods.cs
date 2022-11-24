using GeometryLib.Geometry;

namespace GeometryLib
{
    public static class ConvexHullMethods
    {
        /// <summary>
        /// Очень медленный алгоритм поиска выпуклой оболочки 
        /// </summary>
        /// <param name="ps">Отсортированный список точек</param>
        /// <returns>Список отрезков выпуклой оболочки</returns>
        public static List<Line2d> SlowConvexHull(List<Point2d> ps)
        {
            var hull = new List<Line2d>();
            bool? sign = null;
            foreach (var p1 in ps)
            {
                foreach (var p2 in ps)
                {
                    if (p1 == p2) continue;
                    var line1 = new Line2d(p1, p2);
                    var bAdd = true;
                    foreach (var d1 in 
                             from p3 in ps 
                             where p3 != p1 && p3 != p2 
                             select line1.Distance(p3))
                    {
                        if (sign == null)
                        {
                            sign = d1 >= 0; //TODO ==0!!!
                        }
                        else
                        {
                            if ((d1 < 0 && sign == true) ||
                                (d1 >= 0 && sign == false))
                            {
                                bAdd = false;
                            }
                        }
                    }
                    if (bAdd) hull.Add(line1);
                }
            }

            return hull;
        }

        public static double GetAngle(Point2d p1, Point2d p2, Point2d p3)
        {
            var v1 = p1 - p2;
            var v2 = p3 - p2;
            var a1 = Math.Atan2(v1.X, v1.Y);
            var a2 = Math.Atan2(v2.X, v2.Y);
            return a2 - a1;
        }

        public static List<Point2d> ConvexHull(List<Point2d> ps)
        {
            var upHull = new List<Point2d> { ps[0], ps[1] };
            for (var i = 2; i < ps.Count; i++)
            {
                upHull.Add(ps[i]);
                var b1 = true;
                do
                {
                    var c2 = upHull.Count;
                    var ang = GetAngle(upHull[c2 - 3], upHull[c2 - 2], upHull[c2 - 1]);
                    if (ang < Math.PI)
                    {
                        upHull.Remove(upHull[c2 - 2]);
                        if (upHull.Count <= 2) b1 = false;
                    }
                    else b1 = false;
                } while (b1);
            }

            var c3 = ps.Count;
            var lowHull = new List<Point2d> { ps[c3 - 1], ps[c3 - 2] };

            for (var i = c3 - 3; i >= 0; i--)
            {
                lowHull.Add(ps[i]);
                var b1 = true;
                do
                {
                    var c2 = lowHull.Count;
                    var ang = Math.Abs(GetAngle(lowHull[c2 - 3], lowHull[c2 - 2], lowHull[c2 - 1]));
                    if (ang > Math.PI)
                    {
                        lowHull.Remove(lowHull[c2 - 2]);
                        if (lowHull.Count <= 2) b1 = false;
                    }
                    else b1 = false;
                } while (b1);
            }

            var sp = lowHull[0];
            lowHull.Remove(sp);
            upHull.AddRange(lowHull);
            return upHull;
        }
    }
}