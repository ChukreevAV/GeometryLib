using GeometryLib;
using GeometryLib.Geometry;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TestProject1
{
    [TestClass] public class UnitTest1
    {
        /// <summary>������ ������ ��������� ����� � �������� 0-1</summary>
        /// <param name="count">���������� �����</param>
        /// <returns>������ ����� � 0-1</returns>
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

        [TestCategory("ConvexHull"), TestMethod] public void TestSlowConvexHull()
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

        [TestCategory("ConvexHull"), TestMethod] public void TestConvexHull()
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
            var list1 = GetRandomPoint2ds(200);
            var lines = new List<Line2d>();
            for (var i = 0; i < 100; i+=2)
            {
                lines.Add(new Line2d(list1[i], list1[i + 1]));
            }

            var sb = new StringBuilder();

            foreach (var l in lines)
            {
                sb.AppendLine(l.ToString());
            }
            File.WriteAllText(@"F:\work\lines2.txt", sb.ToString());

            var scr = new AutoCadScript();
            scr.AddLines(lines);
            scr.WriteFile(@"F:\work\lines2.scr");
        }

        private const string Pattern = "(?<x1>\\d+\\.\\d+) (?<y1>\\d+\\.\\d+);(?<x2>\\d+\\.\\d+) (?<y2>\\d+\\.\\d+)";
        private static readonly Regex RegexLine = new (Pattern);

        public static Line2d? ParseLine(string text)
        {
            if (!RegexLine.IsMatch(text)) return null;
            var m = RegexLine.Match(text);
            var x1 = double.Parse(m.Groups["x1"].Value, CultureInfo.InvariantCulture);
            var y1 = double.Parse(m.Groups["y1"].Value, CultureInfo.InvariantCulture);
            var x2 = double.Parse(m.Groups["x2"].Value, CultureInfo.InvariantCulture);
            var y2 = double.Parse(m.Groups["y2"].Value, CultureInfo.InvariantCulture);
            return new Line2d(x1, y1, x2, y2);
        }

        /// <summary>������ ������ ����� �� �����</summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Line2d> ReadLines(string path)
        {
            var strings = File.ReadAllLines(path);
            var list = new List<Line2d>();
            foreach (var line in strings.Select(ParseLine))
            {
                if (line != null) list.Add(line);
            }

            return list;
        }

        [TestCategory("StateNode"), TestMethod] public void TestStateNodeAdd()
        {
            var strings = File.ReadAllLines(@"F:\work\lines1.txt");
            var lines = new List<Line2d>();
            var root = new StateNode();
            foreach (var str in strings)
            {
                var line = ParseLine(str);
                if (line == null) continue;
                lines.Add(line);
                root.Add(line);
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
            var list1 = GetRandomPoint2ds(9);
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
            var list1 = GetRandomPoint2ds(19);
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
            var lines = ReadLines(@"F:\work\lines2.txt");
            var tree = new StateNode();
            foreach (var line in lines)
            {
                tree.Add(line);
            }

            foreach (var line in lines)
            {
                tree.Remove(line);
            }

            Assert.IsTrue(StateNodeIsNull(tree));
        }

        [TestCategory("StateNode"), TestMethod] public void TestStateNodeFind()
        {
            var lines = ReadLines(@"F:\work\lines2.txt");
            var tree = new StateNode();
            foreach (var line in lines)
            {
                tree.Add(line);
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
            var lines = ReadLines(@"F:\work\lines1.txt");
            var tree = new StateNode();
            foreach (var line in lines)
            {
                tree.Add(line);
            }

            var line1 = lines[1];
            var leftLine = tree.FindLeft(line1);
            Assert.IsNotNull(leftLine);
        }

        [TestCategory("IntersectionsMethods"), TestMethod]
        public void TestFindIntersections1()
        {
            var lines = ReadLines(@"F:\work\lines1.txt");
            var i = new IntersectionsMethods();
            var r = i.FindIntersections(lines);
            Assert.IsTrue(r.Any());
        }

        [TestCategory("EventQueue"), TestMethod] public void TestEventQueue1()
        {
            var lines = ReadLines(@"F:\work\lines1.txt");
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
    }
}