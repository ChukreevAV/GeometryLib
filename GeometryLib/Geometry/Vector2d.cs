namespace GeometryLib.Geometry
{
    /// <summary>Вектор на плоскости</summary>
    public class Vector2d
    {
        /// <summary>Ось вправо</summary>
        public double X { get; set; }

        /// <summary>Ось вверх</summary>
        public double Y { get; set; }

        /// <summary>Конструктор</summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Vector2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Скалярное произведение векторов</summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double DotProduct(Vector2d v) => X * v.X + Y * v.Y;

        /// <summary>Длина вектора</summary>
        public double Length => Math.Sqrt(X * X + Y * Y);

        /// <summary>Угол между векторам</summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Ang(Vector2d v)
        {
            var d = X * v.Y - v.X * Y;
            var sgn = d != 0 ? Math.Sign(d) : 1;
            return sgn * Math.Acos(DotProduct(v) / (Length * v.Length));
        }

        public static Vector2d operator *(double d, Vector2d v)
            => new(d * v.X, d * v.Y);

        public static Vector2d operator *(Vector2d v, double d)
            => new(d * v.X, d * v.Y);

        public static Vector2d operator /(Vector2d v, double d)
            => new(v.X / d, v.Y / d);
    }
}