using GeometryLib;
using GeometryLib.Intersections;

using WebApplication2.Controllers;
using WebApplication2.Models;

namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            var convexHull = new LineIntersectionsState(GetSampleData.GetRandomLine2ds(7));

            var eventQueue = new EventQueue();

            foreach (var l in convexHull.Lines)
            {
                eventQueue.AddEvent(l);
            }

            var list1 = eventQueue.GetList();

            var ev = list1.First();
            var list2 = ev.Lines.Cast<IndexedLine2d>().ToList();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var cont = new LineIntersectionsController();
            var convexHull = new LineIntersectionsState(GetSampleData.GetRandomLine2ds(7));
            var convexHull2 = cont.Next(convexHull);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var cont = new LineIntersectionsController();
            var convexHull = new LineIntersectionsState(GetSampleData.GetRandomLine2ds(7));

            while (convexHull.SweepEvents.Any())
            {
                cont.Next(convexHull);
            }
            //var convexHull2 = cont.Next(convexHull);
        }
    }
}