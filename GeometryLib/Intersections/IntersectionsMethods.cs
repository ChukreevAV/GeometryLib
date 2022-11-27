﻿using GeometryLib.Geometry;

namespace GeometryLib.Intersections
{
    public class IntersectionsMethods
    {
        public enum PointPosition
        {
            Up,
            Center,
            Down,
            None
        }

        public static PointPosition GetPointPosition(Line2d line, Point2d p)
        {
            if (p.Distance(line.First()) < Point2d.Epsilon)
                return PointPosition.Up;
            if (p.Distance(line.Last()) < Point2d.Epsilon)
                return PointPosition.Down;
            if (line.IsCenter(p)) return PointPosition.Center;
            return PointPosition.None;
        }

        private StateNode _t = new();
        private EventQueue _eventQueue = new();
        private List<SweepEvent> _result = new();

        private void FindNewEvent(Line2d? sl, Line2d? sr, Point2d p)
        {
            if (sl == null || sr == null) return;
            var ip = sl.Intersect(sr);
            if (ip != null && ip.Y > p.Y) 
                _eventQueue.AddEvent(ip, sl, sr);
        }

        private void HandleEventPoint(SweepEvent? ev)
        {
            if (ev == null) return;
            var dp = new Point2d(ev.Point.X, ev.Point.Y + Point2d.Epsilon);
            var lines = _t.Find(ev.Point); //!!?

            var cList = lines
                .Where(l
                    => GetPointPosition(l, ev.Point) == PointPosition.Center)
                .ToList();

            var upList = ev.Lines.Where(l
                => GetPointPosition(l, ev.Point) == PointPosition.Up)
                .ToList();

            var downList = lines.Where(l
                => GetPointPosition(l, ev.Point) == PointPosition.Down)
                .ToList();

            var rList = new List<Line2d>();
            rList.AddRange(upList);
            rList.AddRange(cList);
            rList.AddRange(downList);
            rList = rList.Distinct().ToList();

            if ( rList.Count > 1 )
            {
                _result.Add(new SweepEvent(ev.Point, rList));
            }

            _t.Remove(downList);
            _t.Remove(cList);

            //var aList = new List<Line2d>();
            //aList.AddRange(upList);
            //aList.AddRange(cList);
            //aList = aList.OrderBy(l => l.First()).ToList();
            //_t.Add(aList);

            _t.Add(dp, upList);
            _t.Add(dp, cList);

            if (!upList.Any() && !cList.Any())
            {
                var left = _t.FindLeft(ev.Point);
                var right = _t.FindRight(ev.Point);

                FindNewEvent(left, right, ev.Point);
            }
            else
            {
                var list1 = new List<Line2d>();
                list1.AddRange(upList);
                list1.AddRange(cList);
                list1 = list1.OrderBy(l => l.GetPointByY(dp.Y)).ToList();

                var left = list1.First();
                var right = _t.FindLeft(dp, left);
                if (right != null) FindNewEvent(left, right, ev.Point);

                right = list1.Last();
                left = _t.FindRight(dp, right);
                if (left != null) FindNewEvent(left, right, ev.Point);
            }
        }

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