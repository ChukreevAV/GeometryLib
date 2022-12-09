using GeometryLib.Geometry;

using Microsoft.AspNetCore.Mvc;

using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConvexHullController : ControllerBase
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

        private readonly SlowConvexHullState _convexHull;

        public ConvexHullController()
        {
            _convexHull = new SlowConvexHullState(GetRandomPoint2ds(30));
        }

        [HttpGet]
        public async Task<ActionResult<SlowConvexHullState>> GetConvexHullStart()
        {
            return await new ValueTask<ActionResult<SlowConvexHullState>>(_convexHull);
        }

        [HttpPost]
        public async Task<ActionResult<SlowConvexHullState>> PostSlowConvexHullState(SlowConvexHullState chs)
        {
            Next(chs);
            return await new ValueTask<ActionResult<SlowConvexHullState>>(chs);
        }

        private void Next(SlowConvexHullState chs)
        {
            if (chs.Index1 == chs.Index2) chs.Index2++;
            var pCount = chs.Points.Count;
            if (chs.Index2 >= pCount)
            {
                chs.SelectPoints.Add(chs.Points[chs.Index1]);
                chs.Index1++;
                chs.Index2 = 0;
                chs.UnselectLines.Clear();
            }
            if (chs.Index1 >= pCount) return;
            //
            var p1 = chs.Points[chs.Index1];
            var p2 = chs.Points[chs.Index2];
            chs.CurrentLine = new Line2d(p1, p2);
            var bAdd = true;
            foreach (var distance in
                     from p3 in chs.Points
                     where !Equals(p3, p1) && !Equals(p3, p2)
                     select chs.CurrentLine.Distance(p3))
            {
                if (chs.Sign == null)
                {
                    chs.Sign = distance >= 0; //TODO ==0!!!
                }
                else
                {
                    if (distance < 0 && chs.Sign == true ||
                        distance >= 0 && chs.Sign == false)
                    {
                        bAdd = false;
                    }
                }
            }

            if (bAdd) chs.ConvexHull.Add(chs.CurrentLine);
            else chs.UnselectLines.Add(chs.CurrentLine);
            chs.Index2++;
        }
    }
}