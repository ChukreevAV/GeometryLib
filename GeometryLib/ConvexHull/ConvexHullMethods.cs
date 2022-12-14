using GeometryLib.Geometry;

namespace GeometryLib.ConvexHull
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
                    if (Equals(p1, p2)) continue;
                    var line1 = new Line2d(p1, p2);
                    var bAdd = true;
                    foreach (var distance in
                             from p3 in ps
                             where !Equals(p3, p1) && !Equals(p3, p2)
                             select line1.Distance(p3))
                    {
                        if (sign == null)
                        {
                            sign = distance >= 0; //TODO ==0!!!
                        }
                        else
                        {
                            if (distance < 0 && sign == true ||
                                distance >= 0 && sign == false)
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



        private static void MakeHalfHull(IList<Point2d> list, Func<double, bool> func)
        {
            var b1 = true;
            do
            {
                var c2 = list.Count;
                var ang = Point2d.GetAngle(list[c2 - 3], list[c2 - 2], list[c2 - 1]);
                if (func(ang))
                {
                    list.Remove(list[c2 - 2]);
                    if (list.Count <= 2) b1 = false;
                }
                else b1 = false;
            } while (b1);
        }

        /// <summary>Поиск выпуклой оболочки</summary>
        /// <param name="ps">Список точек</param>
        /// <returns>Точки выпуклой оболочки</returns>
        public static List<Point2d> ConvexHull(List<Point2d> ps)
        {
            var upHull = new List<Point2d> { ps[0], ps[1] };
            for (var i = 2; i < ps.Count; i++)
            {
                upHull.Add(ps[i]);
                MakeHalfHull(upHull, ang => ang < Math.PI);
            }

            var c3 = ps.Count;
            var lowHull = new List<Point2d> { ps[c3 - 1], ps[c3 - 2] };

            for (var i = c3 - 3; i >= 0; i--)
            {
                lowHull.Add(ps[i]);
                MakeHalfHull(lowHull, ang => Math.Abs(ang) > Math.PI);
            }

            var sp = lowHull[0];
            lowHull.Remove(sp);
            upHull.AddRange(lowHull);
            return upHull;
        }
    }
}