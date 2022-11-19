using GeometryLib;

namespace TestProject1
{
    [TestClass] public class UnitTest1
    {
        public List<Point2d> GetRandomPoint2ds(int count)
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

        [TestMethod] public void TestSlowConvexHull()
        {
            var list1 = GetRandomPoint2ds(33);
            Assert.IsTrue(list1.Count == 33, "Get Points");
            var scr = new AutoCadScript();
            scr.AddPoints(list1);
            list1.Sort();

            var hull = ConvexHullMethods.SlowConvexHull(list1);
            Assert.IsTrue(hull.Count > 2, "Get hull");
            scr.AddLines(hull);
            scr.WriteFile(@"F:\work\TestSlowConvexHull.scr");
        }

        [TestMethod] public void TestConvexHull()
        {
            var list1 = GetRandomPoint2ds(33);
            Assert.IsTrue(list1.Count == 33, "Get Points");
            var scr = new AutoCadScript();
            scr.AddPoints(list1);
            list1.Sort();
         
            var hull = ConvexHullMethods.ConvexHull(list1);
            Assert.IsTrue(hull.Count > 2, "Get hull");
            scr.AddPolyline(hull);
            scr.WriteFile(@"F:\work\TestConvexHull.scr");
        }

        [TestMethod] public void TestMethod1()
        {
            var sp1 = new Point2d(-1, 0);
            var ep1 = new Point2d(1, 0);
            var line1 = new Line2d(sp1, ep1);
            var sp2 = new Point2d(0, -1);
            var ep2 = new Point2d(0, 1);
            var line2 = new Line2d(sp2, ep2);
            var res1 = line1.Intersect(line2);
            Assert.IsNotNull(res1, "res1");
            var sp3 = new Point2d(-1, 2);
            var ep3 = new Point2d(1, 2);
            var line3 = new Line2d(sp3, ep3);
            var res2 = line1.Intersect(line3);
            Assert.IsNull(res2, "res2");
        }

        public Line2d GetLine1()
        {
            var p1 = new Point2d(600, 100);
            var p2 = new Point2d(100, 100);
            return new Line2d(p2, p1);
        }

        public Line2d GetLine2()
        {
            var p1 = new Point2d(342.5974851452364, 61.41807012330702);
            var p2 = new Point2d(400, 200);
            return new Line2d(p2, p1);
        }

        [TestMethod] public void TestMethod2()
        {
            var line1 = GetLine1();
            var line2 = GetLine2();
            var ip1 = line1.Intersect(line2);
        }

        [TestMethod] public void TestMethod3()
        {
            Assert.IsFalse(double.IsFinite(double.PositiveInfinity));
            Assert.IsFalse(double.IsFinite(double.NegativeInfinity));
            Assert.IsFalse(double.IsFinite(double.NaN));
        }
    }
}