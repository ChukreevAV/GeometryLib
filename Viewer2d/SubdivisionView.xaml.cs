using GeometryLib.Geometry;
using GeometryLib.Intersections;

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using static System.Net.Mime.MediaTypeNames;

namespace Viewer2d
{
    /// <summary>
    /// Логика взаимодействия для SubdivisionView.xaml
    /// </summary>
    public partial class SubdivisionView
    {
        private double _scale1 = 100;

        private Dictionary<HalfEdge, Line> Lines = new();

        public Line DrawLine(IEventLine line2d, Brush brush)
        {
            var line1 = new Line
            {
                X1 = line2d.Start.X * _scale1,
                Y1 = line2d.Start.Y * _scale1,
                X2 = line2d.End.X * _scale1,
                Y2 = line2d.End.Y * _scale1,
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
            return sub1;
        }

        private void DrawSub(Subdivision sub)
        {
            foreach (var edge in sub.Edges)
            {
                var line = DrawLine(new EdgeLine(edge), Brushes.Green);
                Lines.Add(edge, line);
            }
        }

        public SubdivisionView()
        {
            InitializeComponent();
            var sub1 = GetData();
            EdgesList.ItemsSource = sub1.Edges;
            DrawSub(sub1);
        }

        private void EdgesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var obj in e.RemovedItems)
            {
                if (obj is not HalfEdge edge1) continue;
                var line1 = Lines[edge1];
                line1.Stroke = Brushes.Green;
                //TwinStackPanel.DataContext = null;
                var linePrev1 = Lines[edge1.Prev];
                linePrev1.Stroke = Brushes.Green;

                var lineNext1 = Lines[edge1.Next];
                lineNext1.Stroke = Brushes.Green;
            }

            foreach (var obj in e.AddedItems)
            {
                if (obj is not HalfEdge edge) continue;
                var line = Lines[edge];
                line.Stroke = Brushes.Red;
                TwinStackPanel.DataContext = edge.Twin;
                NextStackPanel.DataContext = edge.Next;
                PrevStackPanel.DataContext = edge.Prev;

                var linePrev = Lines[edge.Prev];
                linePrev.Stroke = Brushes.DarkBlue;

                var lineNext = Lines[edge.Next];
                lineNext.Stroke = Brushes.Aqua;
            }
        }
    }
}