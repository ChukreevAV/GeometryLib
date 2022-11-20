namespace GeometryLib
{
    /// <summary>Отрезок</summary>
    public class Line2d
    {
        /// <summary>Начальная точка</summary>
        public Point2d Start { get; set; }

        /// <summary>Конечная точка</summary>
        public Point2d End { get; set; }

        public Line2d(double x1, double y1, double x2, double y2)
        {
            Start = new Point2d(x1, y1);
            End = new Point2d(x2, y2);
        }

        /// <summary>Конструктор</summary>
        /// <param name="start">Начальная точка</param>
        /// <param name="end">Конечная точка</param>
        public Line2d(Point2d start, Point2d end)
        {
            Start = start;
            End = end;
        }

        /// <summary>Длина отрезка</summary>
        public double Length => Start.Distance(End);

        public Point2d First()
        {
            if (Start.Y == End.Y)
            {
                return Start.X < End.X ? Start : End;
            }

            return Start.Y < End.Y ? Start : End;
        }

        public Point2d Last()
        {
            if (Start.Y == End.Y)
            {
                return Start.X > End.X ? Start : End;
            }

            return Start.Y > End.Y ? Start : End;
        }

       /// <summary>Параметр A в уравнении Ax+By+C=0</summary>
       public double A => Start.Y - End.Y;

        /// <summary>Параметр B в уравнении Ax+By+C=0</summary>
        public double B => End.X - Start.X;

        /// <summary>Параметр C в уравнении Ax+By+C=0</summary>
        public double C => Start.X * End.Y - End.X * Start.Y;

        /// <summary>Разница X-координаты начала и конца отрезка</summary>
        private double Dx => Start.X - End.X;

        /// <summary>Разница Y-координаты начала и конца отрезка</summary>
        private double Dy => Start.Y - End.Y;

        /// <summary>Расстояние от прямой до точки</summary>
        /// <param name="p">точка до которой определяется расстояние</param>
        /// <returns>Длина нормали от точки до прямой</returns>
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

        private static readonly double Epsilon = 0.01;

        /// <summary>Проверяем принадлежность точки отрезку</summary>
        /// <param name="test">Отрезок</param>
        /// <param name="p">Точка</param>
        /// <returns></returns>
        private static bool LinePointTest(Line2d test, Point2d p)
        {
            if (p.Distance(test.Start) < Epsilon) return true;
            if (p.Distance(test.End) < Epsilon) return true;

            var b1 = p.X >= test.Start.X - Epsilon 
                     && p.X <= test.End.X + Epsilon;
            var b2 = p.X <= test.Start.X + Epsilon 
                     && p.X >= test.End.X - Epsilon;
            var b3 = p.Y >= test.Start.Y - Epsilon 
                     && p.Y <= test.End.Y + Epsilon;
            var b4 = p.Y <= test.Start.Y + Epsilon 
                     && p.Y >= test.End.Y - Epsilon;
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
        public override string ToString() => $"{Start};{End}";
    }
}