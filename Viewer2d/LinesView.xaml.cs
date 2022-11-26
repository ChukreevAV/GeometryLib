using GeometryLib.Geometry;
using GeometryLib.Intersections;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Viewer2d
{
    /// <summary>Логика взаимодействия для LinesView.xaml</summary>
    public partial class LinesView
    {
        private static readonly string Pattern = "(?<x1>\\d+\\.\\d+) (?<y1>\\d+\\.\\d+);(?<x2>\\d+\\.\\d+) (?<y2>\\d+\\.\\d+)";
        private static Regex regexLine = new(Pattern);

        public static Line2d? ParseLine(string text)
        {
            if (!regexLine.IsMatch(text)) return null;
            var m = regexLine.Match(text);
            var x1 = double.Parse(m.Groups["x1"].Value, CultureInfo.InvariantCulture);
            var y1 = double.Parse(m.Groups["y1"].Value, CultureInfo.InvariantCulture);
            var x2 = double.Parse(m.Groups["x2"].Value, CultureInfo.InvariantCulture);
            var y2 = double.Parse(m.Groups["y2"].Value, CultureInfo.InvariantCulture);
            return new Line2d(x1, y1, x2, y2);
        }

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

        private List<Line2d> _lines = ReadLines(@"F:\work\lines2.txt");
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
            var r = i.FindIntersections(_lines);

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