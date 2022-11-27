using System.Globalization;

namespace GeometryLib.Geometry
{
    /// <summary>Точка на плоскости</summary>
    public class Point2d : IComparable
    {
        /// <summary>Допуск для сравнения</summary>
        public static readonly double Epsilon = 0.00000001;

        private static void TestDoubleValue(double d)
        {
            if (!double.IsFinite(d))
                throw new ArgumentException("Координата должна быть конечной");
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

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is Point2d tp) return Distance(tp) < Epsilon;
            return false;
        }

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
        public override string ToString() => X.ToString(CultureInfo.InvariantCulture) + " " +
                                             Y.ToString(CultureInfo.InvariantCulture);
        //X.ToString("F4") + " " + Y.ToString("F4");

        /// <summary>Строковое представления для хранения</summary>
        /// <returns></returns>
        public string ToInvariantCultureString() 
            => X.ToString(CultureInfo.InvariantCulture) + " " + 
               Y.ToString(CultureInfo.InvariantCulture);

        public static Vector2d operator +(Point2d a, Point2d b)
            => new(a.X + b.X, a.Y + b.Y);

        public static Vector2d operator -(Point2d a, Point2d b)
            => new(a.X - b.X, a.Y - b.Y);

        public static bool operator >(Point2d a, Point2d b)
            => a.Y == b.Y ? a.X < b.X : a.Y < b.Y;

        public static bool operator <(Point2d a, Point2d b)
            => a.Y == b.Y ? a.X > b.X : a.Y > b.Y;
    }
}