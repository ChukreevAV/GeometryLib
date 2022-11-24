using GeometryLib;

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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
            Assert.IsNotNull(ip1);
        }

        [TestMethod] public void TestMethod3()
        {
            Assert.IsFalse(double.IsFinite(double.PositiveInfinity));
            Assert.IsFalse(double.IsFinite(double.NegativeInfinity));
            Assert.IsFalse(double.IsFinite(double.NaN));
        }

        [TestMethod] public void TestCreateLines()
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

        private static readonly string Pattern = "(?<x1>\\d+\\.\\d+) (?<y1>\\d+\\.\\d+);(?<x2>\\d+\\.\\d+) (?<y2>\\d+\\.\\d+)";
        private static Regex regexLine = new (Pattern);

        public Line2d? ParseLine(string text)
        {
            if (!regexLine.IsMatch(text)) return null;
            var m = regexLine.Match(text);
            var x1 = double.Parse(m.Groups["x1"].Value, CultureInfo.InvariantCulture);
            var y1 = double.Parse(m.Groups["y1"].Value, CultureInfo.InvariantCulture);
            var x2 = double.Parse(m.Groups["x2"].Value, CultureInfo.InvariantCulture);
            var y2 = double.Parse(m.Groups["y2"].Value, CultureInfo.InvariantCulture);
            return new Line2d(x1, y1, x2, y2);
        }

        [TestMethod] public void TestMethod5()
        {
            var test1 = "0.6012846366974559 0.9414754366938098;0.8479698078839378 0.3919478387540417";
            var pattern = "(?<x1>\\d+\\.\\d+) (?<y1>\\d+\\.\\d+);(?<x2>\\d+\\.\\d+) (?<y2>\\d+\\.\\d+)";
            var reg1 = new Regex(pattern);

            if (reg1.IsMatch(test1)) 
            {
                var m = reg1.Match(test1);
                var x1 = double.Parse(m.Groups["x1"].Value, CultureInfo.InvariantCulture);
                var y1 = double.Parse(m.Groups["y1"].Value, CultureInfo.InvariantCulture);
                var x2 = double.Parse(m.Groups["x2"].Value, CultureInfo.InvariantCulture);
                var y2 = double.Parse(m.Groups["y2"].Value, CultureInfo.InvariantCulture);
            }
        }

        public List<Line2d> ReadLines(string path)
        {
            var strings = File.ReadAllLines(path);
            var list = new List<Line2d>();
            foreach (var line in strings.Select(ParseLine))
            {
                if (line != null) list.Add(line);
            }

            return list;
        }

        [TestMethod] public void TestMethod6()
        {
            var strings = File.ReadAllLines(@"F:\work\lines1.txt");
            var lines = new List<Line2d>();
            var root = new StateNode();
            foreach (var str in strings)
            {
                var line = ParseLine(str);
                if (line != null)
                {
                    lines.Add(line);
                    root.Add(line);
                }
            }
        }

        [TestMethod] public void TestMethod7()
        {
            var p1 = new Point2d(10, 10);
            var p2 = new Point2d(10, 100);
            var p3 = new Point2d(11, 10);

            var b1 = p1 < p3;
            var b2 = p1 < p2;
            var b3 = p1 > p3;
            var b4 = p1 > p2;
        }

        [TestMethod] public void TestMethod8()
        {
            var list1 = GetRandomPoint2ds(9);
            BstNode<Point2d>? root = null;

            foreach (var p in list1)
            {
                if (root == null) root = new BstNode<Point2d>(p, p);
                else root.Add(p, p);
            }

            var p1 = list1[5];

            var t1 = root.Find(p1);

            root.Remove(p1);
        }

        [TestMethod] public void TestMethod9()
        {
            var list1 = GetRandomPoint2ds(19);
            var bst = new BinarySearchTree<Point2d>();

            foreach (var p in list1)
            {
                bst.Add(p, p);
            }

            var left = bst.GetLeft();
            var right = bst.GetRight();

            foreach (var p in list1)
            {
                bst.Remove(p);
            }
        }

        private bool StateNodeIsNull(StateNode node)
        {
            if (node.Line != null) return false;
            if (node.LeftLine != null) return false;
            if (node.RightLine != null) return false;
            if (node.LeftNode != null) return false;
            if (node.RightNode != null) return false;
            return true;
        }

        [TestMethod] public void TestStateNodeRemove()
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

        [TestMethod] public void TestStateNodeFind()
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

            var line1 = lines[0];
            var leftLine = tree.FindLeft(line1);
        }

        [TestMethod] public void TestFindIntersections1()
        {
            var lines = ReadLines(@"F:\work\lines1.txt");
            var i = new IntersectionsMethods();
            var r = i.FindIntersections(lines);
        }

        [TestMethod] public void TestEventQueue1()
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
        }
    }
}