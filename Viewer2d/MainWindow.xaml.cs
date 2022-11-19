using GeometryLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Viewer2d
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        public void DrawPoint(Point2d center, Brush brush)
        {
            var ellipse = new Ellipse();
            ellipse.Fill = brush;
            var size = 12;
            ellipse.Height = size;
            ellipse.Width = size;
            ellipse.Margin = new Thickness(center.X - size/2, center.Y - size / 2, 0, 0);
            WorkCanvas.Children.Add(ellipse);
        }

        public void DrawLine(Line2d line2d, Brush brush)
        {
            var line1 = new Line
            {
                X1 = line2d.Start.X,
                Y1 = line2d.Start.Y,
                X2 = line2d.End.X,
                Y2 = line2d.End.Y,
                Stroke = brush,
                StrokeThickness = 1
            };
            WorkCanvas.Children.Add(line1);
        }

        public void DrawLine(Point2d center, Vector2d v, Brush brush)
        {
            var line1 = new Line
            {
                X1 = center.X,
                Y1 = center.Y,
                X2 = 50 * v.X + center.X,
                Y2 = 50 * v.Y + center.Y,
                Stroke = brush,
                StrokeThickness = 1
            };
            WorkCanvas.Children.Add(line1);
        }

        public void Draw1()
        {
            var center = new Point2d(400, 200);

            var r = 150;
            var c1 = 33;
            var step = 2 * Math.PI / c1;

            for (var i = 0; i < c1; i++)
            {
                var val1 = step * i;
                var x = r * Math.Cos(val1);
                var y = r * Math.Sin(val1);
                var np = new Point2d(x + center.X, y + center.Y);
                var line2 = new Line2d(center, np);
                var d1 = line2.Direction();
                DrawLine(line2, Brushes.Black);
                DrawLine(center, d1, Brushes.Red);
                var n1 = new Vector2d(d1.Y, -d1.X);
                DrawLine(center, n1, Brushes.Aqua);
            }
        }

        private int nextStep = 1;

        public void Draw2()
        {
            var center = new Point2d(400, 200);
            var r = 150;
            var c1 = 32;
            var step = 2 * Math.PI / c1;
         
            var val1 = step * 17;
            var x = r * Math.Cos(val1);
            var y = r * Math.Sin(val1);
            var np = new Point2d(x + center.X, y + center.Y);
            var line2 = new Line2d(center, np);
            var d1 = line2.Direction();
            DrawLine(line2, Brushes.Black);
            DrawLine(center, d1, Brushes.Red);
            var n1 = new Vector2d(d1.Y, -d1.X);
            DrawLine(center, n1, Brushes.Aqua);
        }

        public Line2d GetLine1()
        {
            var p1 = new Point2d(600, 100);
            var p2 = new Point2d(100, 100);
            return new Line2d(p2, p1);
        }

       public void Draw3()
        {
            var p1 = new Point2d(600, 100);
            var p2 = new Point2d(100, 100);
            var line2 = new Line2d(p2, p1);
            DrawLine(line2, Brushes.SteelBlue);
        }

        //private static System.Timers.Timer aTimer;

        private void SetTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += OnTimedEvent;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            dispatcherTimer.Start();
        }

        private List<(Point2d?, Line2d, Line2d)>  tList = new ();

        private void OnTimedEvent(object? sender, EventArgs e)
        {
            try
            {
                WorkCanvas.Children.Clear();
                var line1 = GetLine1();
                Draw3();

                var center = new Point2d(400, 200);
                //DrawPoint(center, Brushes.Red);
                var r = 150;
                var c1 = 32;
                var step = 2 * Math.PI / c1;

                nextStep = nextStep >= c1 ? 0 : nextStep;
                var val1 = step * nextStep++;
                var x = r * Math.Cos(val1);
                var y = r * Math.Sin(val1);
                var np = new Point2d(x + center.X, y + center.Y);
                var line2 = new Line2d(center, np);
                var ip1 = line1.Intersect(line2);

                tList.Add((ip1, line1, line2));
                if (ip1 != null) DrawPoint(ip1, Brushes.Red);
                var d1 = line2.Direction();
                DrawLine(line2, Brushes.Black);
                DrawLine(center, d1, Brushes.Red);
                var n1 = new Vector2d(d1.Y, -d1.X);
                DrawLine(center, n1, Brushes.Aqua);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                //throw;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            //Draw3();
            SetTimer();

            //aTimer.Stop();
            //aTimer.Dispose();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            foreach (var d in tList)
            {
                sb.AppendLine($"{d.Item1}, {d.Item2}, {d.Item3}");
            }

            var path = @"F:\work\TestIntersect.txt";
            File.WriteAllText(path, sb.ToString());
        }
    }
}