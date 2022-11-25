using GeometryLib.Geometry;

namespace GeometryLib.Intersections
{
    /// <summary>Событие на заметающей линии</summary>
    public class SweepEvent
    {
        /// <summary>Точка события</summary>
        public Point2d Point { get; set; }

        /// <summary>Список отрезков совпадающих в точке</summary>
        public List<Line2d> Lines { get; } = new();

        /// <summary>Конструктор</summary>
        /// <param name="p">Точка события</param>
        public SweepEvent(Point2d p)
        {
            Point = p;
        }

        /// <summary>Конструктор</summary>
        /// <param name="p">Точка события</param>
        /// <param name="line">Отрезок попадающий в точку события</param>
        public SweepEvent(Point2d p, Line2d line)
        {
            Point = p;
            Lines.Add(line);
        }

        /// <summary>Конструктор</summary>
        /// <param name="p">Точка события</param>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        public SweepEvent(Point2d p, Line2d line1, Line2d line2)
        {
            Point = p;
            Lines.Add(line1);
            Lines.Add(line2);
        }

        /// <summary>Конструктор</summary>
        /// <param name="p">Точка события</param>
        /// <param name="lines">Список отрезков в точке события</param>
        public SweepEvent(Point2d p, IEnumerable<Line2d> lines)
        {
            Point = p;
            Lines.AddRange(lines);
        }

        /// <summary>Добавить отрезок</summary>
        /// <param name="line"></param>
        public void Add(Line2d line)
        {
            if (!Lines.Contains(line)) Lines.Add(line);
        }
    }
}