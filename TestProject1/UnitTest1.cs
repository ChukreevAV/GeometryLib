using GeometryLib;
using GeometryLib.Geometry;
using GeometryLib.Intersections;
using GeometryLib.Trees;

using System.Text;

namespace TestProject1
{
    [TestClass] public class UnitTest1
    {
        [TestCategory("Geometry"), TestMethod] public void TestIntersect1()
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

        [TestCategory("Geometry"), TestMethod] public void TestIntersect2()
        {
            var line1 = GetLine1();
            var line2 = GetLine2();
            var ip1 = line1.Intersect(line2);
            Assert.IsNotNull(ip1);
        }

        [TestMethod] public void TestDoubleIsFinite()
        {
            Assert.IsFalse(double.IsFinite(double.PositiveInfinity));
            Assert.IsFalse(double.IsFinite(double.NegativeInfinity));
            Assert.IsFalse(double.IsFinite(double.NaN));
        }

        [TestCategory("Geometry"), TestMethod] public void TestCreateLines()
        {
            var list1 = GetSampleData.GetRandomPoint2ds(30);
            var lines = new List<Line2d>();
            for (var i = 0; i < 15; i+=2)
            {
                lines.Add(new Line2d(list1[i], list1[i + 1]));
            }

            var sb = new StringBuilder();

            foreach (var l in lines)
            {
                sb.AppendLine(l.ToString());
            }
            File.WriteAllText(@"F:\work\lines3.txt", sb.ToString());

            var scr = new AutoCadScript();
            scr.AddLines(lines);
            scr.WriteFile(@"F:\work\lines3.scr");
        }

        [TestCategory("StateNode"), TestMethod] public void TestStateNodeAdd()
        {
            var strings = File.ReadAllLines(@"F:\work\lines1.txt");
            var lines = new List<Line2d>();
            var root = new StateNode();
            foreach (var str in strings)
            {
                var line = LoadData.ParseLine(str);
                if (line == null) continue;
                lines.Add(line);
                root.Add(line.First(), line);
            }
            Assert.IsTrue(lines.Count == strings.Length);
        }

        [TestMethod] public void TestPoint2dOperators()
        {
            var p1 = new Point2d(10, 10);
            var p2 = new Point2d(10, 100);
            var p3 = new Point2d(11, 10);

            Assert.IsFalse(p1 < p3);
            Assert.IsFalse(p1 < p2);
            Assert.IsTrue(p1 > p2);
            Assert.IsTrue(p1 > p3);
        }

        [TestCategory("Bst"), TestMethod] public void TestBstNodeFind()
        {
            var list1 = GetSampleData.GetRandomPoint2ds(9);
            BstNode<Point2d>? root = null;

            foreach (var p in list1)
            {
                if (root == null) root = new BstNode<Point2d>(p, p);
                else root.Add(p, p);
            }

            var p1 = list1[5];

            var t1 = root?.Find(p1);
            Assert.IsNotNull(t1);
            root?.Remove(p1);
        }

        [TestCategory("Bst"), TestMethod] public void TestBinarySearchTreeRemove()
        {
            var list1 = GetSampleData.GetRandomPoint2ds(19);
            var bst = new BinarySearchTree<Point2d>();

            foreach (var p in list1)
            {
                bst.Add(p, p);
            }

            var left = bst.GetLeft();
            var right = bst.GetRight();

            Assert.IsNotNull(left);
            Assert.IsNotNull(right);

            foreach (var p in list1)
            {
                bst.Remove(p);
            }
        }

        private static bool StateNodeIsNull(StateNode node)
        {
            if (node.Line != null) return false;
            if (node.LeftLine != null) return false;
            if (node.RightLine != null) return false;
            if (node.LeftNode != null) return false;
            if (node.RightNode != null) return false;
            return true;
        }

        [TestCategory("StateNode"), TestMethod] public void TestStateNodeRemove()
        {
            var lines = LoadData.ReadLines(@"F:\work\lines2.txt");
            var tree = new StateNode();
            foreach (var line in lines)
            {
                tree.Add(line.First(), line);
            }

            foreach (var line in lines)
            {
                tree.Remove(line);
            }

            Assert.IsTrue(StateNodeIsNull(tree));
        }

        [TestCategory("StateNode"), TestMethod] public void TestStateNodeFind()
        {
            var lines = LoadData.ReadLines(@"F:\work\lines2.txt");
            var tree = new StateNode();
            foreach (var line in lines)
            {
                tree.Add(line.First(), line);
            }

            var p1 = new Point2d(0.48703490807666006, 0.34483213219585906);
            var left = tree.FindLeft(p1);
            var right = tree.FindRight(p1);

            Assert.IsNotNull(left, "left");
            Assert.IsNotNull(right, "right");
        }

        [TestCategory("StateNode"), TestMethod]
        public void TestStateNodeFind2()
        {
            var lines = LoadData.ReadLines(@"F:\work\lines1.txt");
            var tree = new StateNode();
            foreach (var line in lines)
            {
                tree.Add(line.First(), line);
            }

            var line1 = lines[1];
            //var leftLine = tree.FindLeft(line1);
            //Assert.IsNotNull(leftLine);
        }

        [TestCategory("IntersectionsMethods"), TestMethod]
        public void TestFindIntersections1()
        {
            var lines = LoadData.ReadLines(@"F:\work\lines1.txt");
            var i = new IntersectionsMethods();
            var r = i.FindIntersections(lines);
            Assert.IsTrue(r.Any());
        }

        [TestCategory("EventQueue"), TestMethod] public void TestEventQueue1()
        {
            var lines = LoadData.ReadLines(@"F:\work\lines1.txt");
            var eventQueue = new EventQueue();
            var eList = new List<SweepEvent?>();

            foreach (var l in lines)
            {
                eventQueue.AddEvent(l);
            }

            while (!eventQueue.IsEmpty())
            {
                eList.Add(eventQueue.GetNextEvent());
            }

            Assert.IsTrue(eList.Any());
        }

        [TestMethod] public void TestMethod1()
        {
            var lines = LoadData.ReadLines(@"F:\work\lines1.txt");
            var l3 = lines[3];
            var l4 = lines[4];
            var p1 = new Point2d(0.36988774764577725, 0.5687411345076795);
            var b1 = l3.Contain(p1);
            var res1 = IntersectionsMethods.GetPointPosition(l4, p1);
        }

        [TestMethod] public void TestMethod2()
        {
            var p1 = new Point2d(0, 0);
            var p2 = new Point2d(10, 10);
            var l1 = new Line2d(p1, p2);
            var x1 = l1.GetPointByY(0.5); 
            Assert.IsTrue(x1.X == 0.5);
            var x2 = l1.GetPointByY(5);
            Assert.IsTrue(x2.X == 5);
        }

        [TestMethod] public void TestMethod3()
        {

        }
    }
}