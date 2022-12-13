using GeometryLib;
using GeometryLib.Geometry;
using GeometryLib.Intersections;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Viewer2d
{
    /// <summary>Логика взаимодействия для LinesView.xaml</summary>
    public partial class LinesView
    {
        private double _scale1 = 400;

        public void DrawLine(Line2d line2d, Brush brush)
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
        }

        public void DrawPoint(Point2d center, Brush brush)
        {
            var ellipse = new Ellipse();
            ellipse.Fill = brush;
            var size = 12;
            ellipse.Height = size;
            ellipse.Width = size;
            ellipse.Margin = new Thickness(center.X * _scale1 - size / 2, center.Y * _scale1 - size / 2, 0, 0);
            WorkCanvas.Children.Add(ellipse);
        }

        private void SetTimer()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += OnTimedEvent;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            dispatcherTimer.Start();
        }

        private int _position = 0;

        //private List<Line2d> _lines = LoadData.ReadLines(@"F:\work\lines2.txt");
        private List<Line2d> _lines = GetSampleData.GetRandomLine2ds(20);
        private StateNode _tree;

        private void OnTimedEvent(object? sender, EventArgs e)
        {
            WorkCanvas.Children.Clear();

            foreach (var line in _lines)
            {
                DrawLine(line, Brushes.Green);
            }
        }

        public LinesView()
        {
            InitializeComponent();

            //var lines = ReadLines(@"F:\work\lines1.txt");

            _tree = new StateNode();
            foreach (var line in _lines)
            {
                //_tree.Add(line);
            }

            //SetTimer();

            foreach (var line in _lines)
            {
                DrawLine(line, Brushes.Green);
            }

            var i = new IntersectionsMethods();
            var r = i.FindIntersections(_lines.Cast<IEventLine>().ToList());

            foreach (var ev in r)
            {
                var pp = ev.Point;
                DrawPoint(pp, Brushes.Red);
            }

            var sn = new StateNode();
            var line1 = _lines[0];
            sn.Add(line1.First(), line1);
            StackPanel1.DataContext = sn;
            //TextBlock1.Text = sn.Line?.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WorkCanvas.Children.Clear();

            foreach (var line in _lines)
            {
                DrawLine(line, Brushes.Green);
            }

            if (_position >= _lines.Count) _position = 0;
            var line1 = _lines[_position++];
            DrawLine(line1, Brushes.DarkBlue);
            //var leftLine = _tree.FindLeft(line1);
            //var leftLine = _tree.FindRight(line1);
            //if (leftLine != null) DrawLine(leftLine, Brushes.Red);
        }
    }
}