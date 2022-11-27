using GeometryLib.Geometry;

using System.ComponentModel;

namespace GeometryLib.Intersections
{
    public class IntersectionsMethods
    {
        /// <summary>Положение точки относительно отрезка</summary>
        public enum PointPosition
        {
            [Description("Начало отрезка")] Up,
            [Description("Внутри отрезка")] Center,
            [Description("Конец отрезка")] Down,
            [Description("Нигде")] None
        }

        /// <summary>Получаем положение точки относительно отрезка</summary>
        /// <param name="line">Отрезок</param>
        /// <param name="p">Точка</param>
        /// <returns>Положение точки</returns>
        public static PointPosition GetPointPosition(Line2d line, Point2d p)
        {
            if (p.Distance(line.First()) < Point2d.Epsilon) return PointPosition.Up;
            if (p.Distance(line.Last()) < Point2d.Epsilon) return PointPosition.Down;
            return line.IsCenter(p) ? PointPosition.Center : PointPosition.None;
        }

        private readonly StateNode _statusTree = new();
        private EventQueue _eventQueue = new();
        private readonly List<SweepEvent> _result = new();

        /// <summary>Поиск точки нового события - пересечения</summary>
        /// <param name="sl">Левый отрезок</param>
        /// <param name="sr">Правый отрезок</param>
        /// <param name="p">Точка события</param>
        private void FindNewEvent(Line2d? sl, Line2d? sr, Point2d p)
        {
            if (sl == null || sr == null) return;
            var ip = sl.Intersect(sr);
            if (ip != null && ip.Y > p.Y) 
                _eventQueue.AddEvent(ip, sl, sr);
        }

        /// <summary>Обработка события</summary>
        /// <param name="ev">Событие</param>
        private void HandleEventPoint(SweepEvent? ev)
        {
            if (ev == null) return;

            var dp = new Point2d(ev.Point.X, ev.Point.Y + Point2d.Epsilon);
            var lines = _statusTree.Find(ev.Point); //!!?

            //Пересекающие отрезки
            var cList = lines
                .Where(l => GetPointPosition(l, ev.Point) == PointPosition.Center)
                .ToList();

            //Отрезки на добавление
            var upList = ev.Lines
                .Where(l => GetPointPosition(l, ev.Point) == PointPosition.Up)
                .ToList();

            //Отрезки на удаление
            var downList = lines
                .Where(l => GetPointPosition(l, ev.Point) == PointPosition.Down)
                .ToList();

            var rList = new List<Line2d>();
            rList.AddRange(upList);
            rList.AddRange(cList);
            rList.AddRange(downList);
            rList = rList.Distinct().ToList();

            if ( rList.Count > 1) _result.Add(new SweepEvent(ev.Point, rList));

            _statusTree.Remove(downList);
            _statusTree.Remove(cList);

            _statusTree.Add(dp, upList);
            _statusTree.Add(dp, cList);

            if (!upList.Any() && !cList.Any())
            {
                //Поиск новых пересечений после удаления отрезка
                var left = _statusTree.FindLeft(ev.Point);
                var right = _statusTree.FindRight(ev.Point);

                FindNewEvent(left, right, ev.Point);
            }
            else
            {
                var list1 = new List<Line2d>();
                list1.AddRange(upList);
                list1.AddRange(cList);
                list1 = list1.OrderBy(l => l.GetPointByY(dp.Y)).ToList();

                var left = list1.First();
                var right = _statusTree.FindLeft(dp, left);
                if (right != null) FindNewEvent(left, right, ev.Point);

                right = list1.Last();
                left = _statusTree.FindRight(dp, right);
                if (left != null) FindNewEvent(left, right, ev.Point);
            }
        }

        /// <summary>Поиск пересечений отрезков</summary>
        /// <param name="lines">Список отрезков</param>
        /// <returns>Список пересечений</returns>
        public List<SweepEvent> FindIntersections(List<Line2d> lines)
        {
            _eventQueue = new EventQueue();

            foreach (var l in lines)
            {
                _eventQueue.AddEvent(l);
            }

            while (!_eventQueue.IsEmpty())
            {
                HandleEventPoint(_eventQueue.GetNextEvent());
            }

            return _result;
        }
    }
}