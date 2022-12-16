using GeometryLib.Geometry;

namespace GeometryLib.Intersections
{
    public interface IEventLine
    {
        /// <summary>Начальная точка</summary>
        Point2d Start { get; }

        /// <summary>Конечная точка</summary>
        Point2d End { get; }

        /// <summary>"Первая" точка</summary>
        /// <returns></returns>
        Point2d First();


        /// <summary>"Последняя" точка</summary>
        /// <returns></returns>
        Point2d Last();

        /// <summary>Точка "внутри" отрезка</summary>
        /// <param name="p"></param>
        /// <returns></returns>
        bool IsCenter(Point2d p);

        /// <summary>Поиск точки пересечения</summary>
        /// <param name="test">Отрезок ля проверки</param>
        /// <returns>Точка пересечения</returns>
        Point2d? Intersect(IEventLine test);

        /// <summary>Параметр A в уравнении Ax+By+C=0</summary>
        double A { get; }

        /// <summary>Параметр B в уравнении Ax+By+C=0</summary>
        double B { get; }

        /// <summary>Параметр C в уравнении Ax+By+C=0</summary>
        double C { get; }

        /// <summary>Разница X-координаты начала и конца отрезка</summary>
        double Dx { get; }

        /// <summary>Разница Y-координаты начала и конца отрезка</summary>
        double Dy { get; }

        Point2d GetPointByY(double y);

        bool Contain(Point2d p);
    }
}