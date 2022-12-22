using System;
using GeometryLib;
using GeometryLib.Geometry;
using GeometryLib.Intersections;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Viewer2d
{
    /// <summary>
    /// Логика взаимодействия для SubdivisionView.xaml
    /// </summary>
    public partial class SubdivisionView
    {
        private double _scale1 = 100;

        private Dictionary<HalfEdge, Line> Lines = new();
        //private Dictionary<HalfEdge, Line> TwinLines = new();

        public Line DrawLine(Canvas canvas, IEventLine line2d, Brush brush)
        {
            var line1 = new Line
            {
                X1 = line2d.Start.X * _scale1,
                Y1 = 400 - line2d.Start.Y * _scale1,
                X2 = line2d.End.X * _scale1,
                Y2 = 400 - line2d.End.Y * _scale1,
                Stroke = brush,
                StrokeThickness = 1
            };
            canvas.Children.Add(line1);
            return line1;
        }

        private Subdivision GetData1()
        {
            var list1 = new List<Point2d>
            {
                new(1, 1),
                new(1, 2),
                new(2, 2),
                new(2, 1),
            };

            var list2 = new List<Point2d>
            {
                new(1.5, 0.5),
                new(2.5, 0.5),
                new(2.5, 1.5),
                new(1.5, 1.5),
            };

            var sub1 = new Subdivision();
            var sub2 = new Subdivision();
            sub1.AddFace("face1", list1);
            sub2.AddFace("face2", list2);

            sub1.Add(sub2);
            var ed1 = sub1.Edges[0];
            var ed2 = sub1.Edges[7];

            var ed3 = sub1.Edges[3];
            var ed4 = sub1.Edges[4];

            var v2 = new Vertex(new Point2d(1.5, 1));
            var v1 = new Vertex(new Point2d(2, 1.5));
            sub1.Vertices.Add(v1);
            sub1.Vertices.Add(v2);
            var eList = ed1.Divide(ed2, v1);
            sub1.Edges.AddRange(eList);
            eList = ed3.Divide(ed4, v2);
            sub1.Edges.AddRange(eList);

            return sub1;
        }

        private Subdivision GetData2()
        {
            var sub1 = new Subdivision();
            var list1 = GetSampleData.GetRandomConvexHull(9);
            var list2 = GetSampleData.GetRandomConvexHull(9);
            sub1.AddFace("face1", list1);
            sub1.AddFace("face2", list2);
            return sub1;
        }

        private void DrawSub(Subdivision sub)
        {
            foreach (var edge in sub.Edges)
            {
                var line = DrawLine(WorkCanvas, new EdgeLine(edge), Brushes.Green);
                Lines.Add(edge, line);
                Lines.Add(edge.Twin, line);
            }
        }

        private void TestFace(List<HalfEdge> edges)
        {
            var faces = new List<List<Face>>();
            var ccv = new List<bool>();
            var lefts = new List<HalfEdge>();
            var angs = new List<double>();
            var bList1 = new List<bool>();
            foreach (var edge in edges)
            {
                var fList = edge
                    .GetLoop()
                    .Select(e => e.Face)
                    .Distinct()
                    .ToList();

                var left = edge.GetLeftEdge();
                lefts.Add(left);
                var ang = left.GetAngle() *180 /Math.PI;

                angs.Add(ang);
                faces.Add(fList);
                ccv.Add(edge.IsCounterclockwise);
                bList1.Add(edge.IsOuther());
            }
        }

        public SubdivisionView()
        {
            InitializeComponent();
            var sub1 = GetData1();

            EdgesList.ItemsSource = sub1.Edges;
            var tList = sub1.Edges.Select(e => e.Twin);
            TwinsList.ItemsSource = tList;
            var loops = sub1.GetLoops();
            //var eList = sub1.Faces.SelectMany(f => f.GetEdges());
            var eList = loops.SelectMany(e => e.GetLoop());
            Edges1List.ItemsSource = eList;

            DrawSub(sub1);

            TestFace(loops);
            Faces1List.ItemsSource = loops;
        }

        private void SetColor(HalfEdge? edge, Brush brush)
        {
            if (edge == null || !Lines.ContainsKey(edge)) return;
            var line = Lines[edge];
            line.Stroke = brush;
        }

        private void SetTwinColor(HalfEdge? edge, Brush brush)
        {
            if (edge == null || !Lines.ContainsKey(edge)) return;
            var line = Lines[edge];
            line.Stroke = brush;
        }

        private void EdgesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var obj in e.RemovedItems)
            {
                if (obj is not HalfEdge edge1) continue;
                SetColor(edge1, Brushes.Green);
                SetColor(edge1.Prev, Brushes.Green);
                SetColor(edge1.Next, Brushes.Green);
            }

            foreach (var obj in e.AddedItems)
            {
                if (obj is not HalfEdge edge) continue;

                TwinStackPanel.DataContext = edge.Twin;
                NextStackPanel.DataContext = edge.Next;
                PrevStackPanel.DataContext = edge.Prev;

                SetColor(edge, Brushes.Red);
                SetColor(edge.Prev, Brushes.DarkBlue);
                SetColor(edge.Next, Brushes.Aqua);
            }
        }

        private void TwinsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var obj in e.RemovedItems)
            {
                if (obj is not HalfEdge edge1) continue;
                SetTwinColor(edge1, Brushes.Green);
                SetTwinColor(edge1.Prev, Brushes.Green);
                SetTwinColor(edge1.Next, Brushes.Green);
            }

            foreach (var obj in e.AddedItems)
            {
                if (obj is not HalfEdge edge) continue;

                TwinStackPanel.DataContext = edge.Twin;
                NextStackPanel.DataContext = edge.Next;
                PrevStackPanel.DataContext = edge.Prev;

                SetTwinColor(edge, Brushes.Red);
                SetTwinColor(edge.Prev, Brushes.DarkBlue);
                SetTwinColor(edge.Next, Brushes.Aqua);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (var line in Lines.Values)
            {
                line.Stroke = Brushes.Green;
            }
        }

        private void Faces1List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var obj in e.AddedItems)
            {
                if (obj is not HalfEdge edge) continue;
                WorkCanvas2.Children.Clear();
                var color = edge.IsCounterclockwise ? Brushes.Green : Brushes.Red;
                DrawLine(WorkCanvas2, new EdgeLine(edge), color);
                var nextEdge = edge.Next;
                while (nextEdge != edge)
                {
                    if (nextEdge == null) return;
                    color = nextEdge.IsCounterclockwise ? Brushes.Green : Brushes.Red;
                    DrawLine(WorkCanvas2, new EdgeLine(nextEdge), color);
                    nextEdge = nextEdge.Next;
                }
            }
        }

        private void WorkCanvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(WorkCanvas);
            Coords.Text = $"{p.X/_scale1}; {(p.Y - 400) * -1 / _scale1}";
        }
    }
}