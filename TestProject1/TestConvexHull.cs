using GeometryLib;

namespace TestProject1
{
    [TestClass] public class TestConvexHull
    {
        [TestCategory("ConvexHull"), TestMethod]
        public void TestSlowConvexHull()
        {
            var list1 = GetSampleData.GetRandomPoint2ds(33);
            Assert.IsTrue(list1.Count == 33, "Get Points");
            var scr = new AutoCadScript();
            scr.AddPoints(list1);
            list1.Sort();

            var hull = ConvexHullMethods.SlowConvexHull(list1);
            Assert.IsTrue(hull.Count > 2, "Get hull");
            scr.AddLines(hull);
            scr.WriteFile(@"F:\work\TestSlowConvexHull.scr");
        }

        [TestCategory("ConvexHull"), TestMethod]
        public void TestConvexHull1()
        {
            var pointsCount = 33;
            var list1 = GetSampleData.GetRandomPoint2ds(pointsCount);
            Assert.IsTrue(list1.Count == pointsCount, "Get Points");
            var scr = new AutoCadScript();
            scr.AddPoints(list1);
            list1.Sort();

            var hull = ConvexHullMethods.ConvexHull(list1);
            Assert.IsTrue(hull.Count > 2, "Get hull");
            scr.AddPolyline(hull);
            scr.WriteFile(@"F:\work\TestConvexHull1.scr");
        }
    }
}