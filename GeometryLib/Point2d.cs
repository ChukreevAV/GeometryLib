using System.Globalization;

namespace GeometryLib
{
    /// <summary>Точка на плоскости</summary>
    public class Point2d : IComparable
    {
        private bool TestDoubleValue(double d)
        {
            if (!double.IsFinite(d))
                throw new ArgumentException("Координата должна быть конечной");

            return true;
        }

        private double _x;
        private double _y;

        /// <summary>Ось вправо</summary>
        public double X 
        { 
            get => _x;
            set
            {
                TestDoubleValue(value);
                _x = value;
            }
        }

        /// <summary>Ось вверх</summary>
        public double Y
        {
            get => _y;
            set
            {
                TestDoubleValue(value);
                _y = value;
            }
        }

        /// <summary>Конструктор</summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Point2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Расстояние между точками</summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double Distance(Point2d p)
            => Math.Sqrt(Math.Pow(X - p.X, 2) + Math.Pow(Y - p.Y, 2));

        public int CompareTo(object? obj)
        {
            if (obj is not Point2d tp)
                throw new ArgumentException("Объект не Point2d");
            if (tp.X == X)
            {
                return tp.Y > Y ? -1 : 1;
            }

            return tp.X > X ? -1 : 1;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return X.ToString(CultureInfo.InvariantCulture) + " " +
                   Y.ToString(CultureInfo.InvariantCulture);
        }

        public static Vector2d operator +(Point2d a, Point2d b)
            => new(a.X + b.X, a.Y + b.Y);

        public static Vector2d operator -(Point2d a, Point2d b)
            => new(a.X - b.X, a.Y - b.Y);
    }
}