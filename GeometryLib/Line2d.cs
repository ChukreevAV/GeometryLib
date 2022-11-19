namespace GeometryLib
{
    /// <summary>Отрезок</summary>
    public class Line2d
    {
        /// <summary>Начальная точка</summary>
        public Point2d Start { get; set; }

        /// <summary>Конечная точка</summary>
        public Point2d End { get; set; }

        /// <summary>Конструктор</summary>
        /// <param name="start">Начальная точка</param>
        /// <param name="end">Конечная точка</param>
        public Line2d(Point2d start, Point2d end)
        {
            Start = start;
            End = end;
        }

        /// <summary></summary>
        public double Length => Start.Distance(End);

        /// <summary></summary>
        public double A => Start.Y - End.Y;

        /// <summary></summary>
        public double B => End.X - Start.X;

        /// <summary></summary>
        public double C => Start.X * End.Y - End.X * Start.Y;

        /// <summary></summary>
        private double Dx => Start.X - End.X;

        /// <summary></summary>
        private double Dy => Start.Y - End.Y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double Distance(Point2d p)
        {
            var a = A;
            var b = B;
            var desc = Math.Sqrt(a * a + b * b);
            return (a * p.X + b * p.Y + C) / desc;
        }

        /// <summary>Вектор направления отрезка</summary>
        /// <returns></returns>
        public Vector2d Direction() => (End - Start) / Length;

        /// <summary>Получаем упорядоченные координаты</summary>
        /// <param name="test">Отрезок</param>
        /// <returns>Координаты упорядоченные слева на право</returns>
        private static (double x1, double y1, double x2, double y2) Order(Line2d test)
        {
            return test.Start.X > test.End.X
                ? new(test.End.X, test.End.Y, test.Start.X, test.Start.Y)
                : new(test.Start.X, test.Start.Y, test.End.X, test.End.Y);
        }

        private static readonly double _epsilon = 0.01;

        /// <summary>Проверяем принадлежность точки отрезку</summary>
        /// <param name="test">Отрезок</param>
        /// <param name="p">Точка</param>
        /// <returns></returns>
        private static bool LinePointTest(Line2d test, Point2d p)
        {
            if (p.Distance(test.Start) < _epsilon) return true;
            if (p.Distance(test.End) < _epsilon) return true;

            var b1 = p.X >= test.Start.X - _epsilon 
                     && p.X <= test.End.X + _epsilon;
            var b2 = p.X <= test.Start.X + _epsilon 
                     && p.X >= test.End.X - _epsilon;
            var b3 = p.Y >= test.Start.Y - _epsilon 
                     && p.Y <= test.End.Y + _epsilon;
            var b4 = p.Y <= test.Start.Y + _epsilon 
                     && p.Y >= test.End.Y - _epsilon;
            return (b1 || b2) && (b3 || b4);
        }

        /// <summary>Поиск точки пересечения</summary>
        /// <param name="test">Отрезок ля проверки</param>
        /// <returns>Точка пересечения</returns>
        public Point2d? Intersect(Line2d test)
        {
            var desc = Dx * test.Dy -Dy * test.Dx;
            var x = (C * test.Dx - Dx * test.C) / desc;
            var y = (C * test.Dy - Dy * test.C) / desc;

            var res = new Point2d(x, y);
            var t1 = LinePointTest(this, res);
            var t2 = LinePointTest(test, res);
            return t1 && t2 ? res : null;
        }

        /// <inheritdoc/>
        public override string ToString() => $"sp:{Start}; ep:{End}";
    }
}