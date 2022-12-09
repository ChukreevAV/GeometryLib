using GeometryLib.Geometry;

using Microsoft.AspNetCore.Mvc;

using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlowConvexHullController : ControllerBase
    {
        private static List<Point2d> GetRandomPoint2ds(int count)
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

        public SlowConvexHullController()
        {
            _convexHull = new SlowConvexHullState(GetRandomPoint2ds(20));
        }

        [HttpGet]
        public async Task<ActionResult<SlowConvexHullState>> GetConvexHullStart()
        {
            return await new ValueTask<ActionResult<SlowConvexHullState>>(_convexHull);
        }

        [HttpPost]
        public async Task<ActionResult<SlowConvexHullState>> PostSlowConvexHullState(SlowConvexHullState chs)
        {
            chs.selectPoints ??= new List<Point2d>();
            chs.unselectLines ??= new List<Line2d>();
            chs.convexHull ??= new List<Line2d>();
            Next(chs);
            return await new ValueTask<ActionResult<SlowConvexHullState>>(chs);
        }

        private void Next(SlowConvexHullState chs)
        {
            if (chs.index1 == chs.index2) chs.index2++;
            var pCount = chs.points.Count;
            if (chs.index2 >= pCount)
            {
                chs.selectPoints.Add(chs.points[chs.index1]);
                chs.index1++;
                chs.index2 = 0;
                chs.unselectLines.Clear();
            }
            if (chs.index1 >= pCount) return;
            //
            var p1 = chs.points[chs.index1];
            var p2 = chs.points[chs.index2];
            chs.currentLine = new Line2d(p1, p2);
            var bAdd = true;
            foreach (var distance in
                     from p3 in chs.points
                     where !Equals(p3, p1) && !Equals(p3, p2)
                     select chs.currentLine.Distance(p3))
            {
                if (chs.sign == null)
                {
                    chs.sign = distance >= 0; //TODO ==0!!!
                }
                else
                {
                    if (distance < 0 && chs.sign == true ||
                        distance >= 0 && chs.sign == false)
                    {
                        bAdd = false;
                    }
                }
            }

            if (bAdd) chs.convexHull.Add(chs.currentLine);
            else chs.unselectLines.Add(chs.currentLine);
            chs.index2++;
        }
    }
}