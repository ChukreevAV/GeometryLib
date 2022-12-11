using GeometryLib;
using GeometryLib.Geometry;

using Microsoft.AspNetCore.Mvc;

using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController, Route("[controller]")]
    public class ConvexHullController
    {
        [HttpGet]
        public async Task<ActionResult<ConvexHullState>> GetConvexHullStart()
        {
            var convexHull = new ConvexHullState(GetSampleData.GetRandomPoint2ds(20));
            return await new ValueTask<ActionResult<ConvexHullState>>(convexHull);
        }

        [HttpPost]
        public async Task<ActionResult<ConvexHullState>> PostConvexHullState(ConvexHullState chs)
        {
            Next(chs);
            return await new ValueTask<ActionResult<ConvexHullState>>(chs);
        }

        private static double GetAngle(Point2d p1, Point2d p2, Point2d p3)
        {
            var v1 = p1 - p2;
            var v2 = p3 - p2;
            var a1 = Math.Atan2(v1.X, v1.Y);
            var a2 = Math.Atan2(v2.X, v2.Y);
            return a2 - a1;
        }

        private static void MakeHalfHull(IList<Point2d> list, Func<double, bool> func)
        {
            var b1 = true;
            do
            {
                var c2 = list.Count;
                var ang = GetAngle(list[c2 - 3], list[c2 - 2], list[c2 - 1]);
                if (func(ang))
                {
                    list.Remove(list[c2 - 2]);
                    if (list.Count <= 2) b1 = false;
                }
                else b1 = false;
            } while (b1);
        }

        private static void Next(ConvexHullState state)
        {
            if (state.Points == null) return;
            state.UpConvexHull ??= new List<Point2d>();
            state.DownConvexHull ??= new List<Point2d>();

            if (state.Index1 == 0)
            {
                state.UpConvexHull.Add(state.Points[0]);
                state.UpConvexHull.Add(state.Points[1]);
                state.UpConvexHull.Add(state.Points[2]);
                MakeHalfHull(state.UpConvexHull, ang => ang < Math.PI);
                state.Index1 = 2;
            }
            else if (state.Index1 < state.Points.Count)
            {
                state.UpConvexHull.Add(state.Points[state.Index1]);
                MakeHalfHull(state.UpConvexHull, ang => ang < Math.PI);
                state.Index1++;
            }
            else if (state.Index2 == state.Points.Count)
            {
                var c3 = state.Points.Count;
                state.DownConvexHull.Add(state.Points[c3 - 1]);
                state.DownConvexHull.Add(state.Points[c3 - 2]);
                state.DownConvexHull.Add(state.Points[c3 - 3]);
                MakeHalfHull(state.DownConvexHull, ang => Math.Abs(ang) > Math.PI);
                state.Index2 = c3 - 4;
            }
            else if (state.Index2 >= 0)
            {
                state.DownConvexHull.Add(state.Points[state.Index2]);
                MakeHalfHull(state.DownConvexHull, ang => Math.Abs(ang) > Math.PI);
                state.Index2--;
            }
        }
    }
}