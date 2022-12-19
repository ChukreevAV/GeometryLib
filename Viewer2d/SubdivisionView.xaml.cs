using GeometryLib.Geometry;
using GeometryLib.Intersections;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
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

        public Line DrawLine(IEventLine line2d, Brush brush)
        {
            var line1 = new Line
            {
                X1 = line2d.Start.X * _scale1,
                Y1 = _scale1 - line2d.Start.Y * _scale1 * -1,
                X2 = line2d.End.X * _scale1,
                Y2 = _scale1 - line2d.End.Y * _scale1 * -1,
                Stroke = brush,
                StrokeThickness = 1
            };
            WorkCanvas.Children.Add(line1);
            return line1;
        }

        private Subdivision GetData()
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
            sub1.AddFace(list1);
            sub2.AddFace(list2);

            sub1.Add(sub2);
            var ed1 = sub1.Edges[2];
            var ed2 = sub1.Edges[5];

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

        private void DrawSub(Subdivision sub)
        {
            foreach (var edge in sub.Edges)
            {
                var line = DrawLine(new EdgeLine(edge), Brushes.Green);
                Lines.Add(edge, line);
                Lines.Add(edge.Twin, line);
            }
        }

        public SubdivisionView()
        {
            InitializeComponent();
            var sub1 = GetData();

            EdgesList.ItemsSource = sub1.Edges;
            var tList = sub1.Edges.Select(e => e.Twin);
            TwinsList.ItemsSource = tList;

            var eList = sub1.Faces.SelectMany(f => f.GetEdges());
            Edges1List.ItemsSource = eList;

            DrawSub(sub1);
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
    }
}