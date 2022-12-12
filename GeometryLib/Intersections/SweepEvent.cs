using GeometryLib.Geometry;

namespace GeometryLib.Intersections
{
    /// <summary>Событие на заметающей линии</summary>
    public class SweepEvent
    {
        /// <summary>Точка события</summary>
        public Point2d Point { get; set; }

        /// <summary>Список отрезков совпадающих в точке</summary>
        public List<IEventLine> Lines { get; } = new();

        /// <summary>Конструктор</summary>
        /// <param name="p">Точка события</param>
        public SweepEvent(Point2d p)
        {
            Point = p;
        }

        /// <summary>Конструктор</summary>
        /// <param name="p">Точка события</param>
        /// <param name="line">Отрезок попадающий в точку события</param>
        public SweepEvent(Point2d p, IEventLine line)
        {
            Point = p;
            Lines.Add(line);
        }

        /// <summary>Конструктор</summary>
        /// <param name="p">Точка события</param>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        public SweepEvent(Point2d p, IEventLine line1, IEventLine line2)
        {
            Point = p;
            Lines.Add(line1);
            Lines.Add(line2);
        }

        /// <summary>Конструктор</summary>
        /// <param name="p">Точка события</param>
        /// <param name="lines">Список отрезков в точке события</param>
        public SweepEvent(Point2d p, IEnumerable<IEventLine> lines)
        {
            Point = p;
            Lines.AddRange(lines);
        }

        /// <summary>Добавить отрезок</summary>
        /// <param name="line"></param>
        public void Add(IEventLine line)
        {
            if (!Lines.Contains(line)) Lines.Add(line);
        }
    }
}