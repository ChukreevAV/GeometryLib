using static System.Net.Mime.MediaTypeNames;

namespace GeometryLib
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
            if (p.Distance(line.First()) < Point2d.Epsilon) return PointPosition.Up;
            if (p.Distance(line.Last()) < Point2d.Epsilon) return PointPosition.Down;
            if (line.IsCenter(p)) return PointPosition.Center;
            return PointPosition.None;
        }

        private StateNode _t = new();
        private List<SweepEvent> _result = new();

        private void HandleEventPoint(SweepEvent? ev)
        {
            if (ev == null) return;
            var lines = _t.Find(ev.Point);
            if (lines.Any()) _result.Add(new SweepEvent(ev.Point, lines));
            var del = lines.Where(l => GetPointPosition(l, ev.Point) != PointPosition.Up);

        }

        public List<SweepEvent> FindIntersections(List<Line2d> lines)
        {
            var q = new EventQueue();

            foreach (var l in lines)
            {
                q.AddUpEvent(l);
            }

            //var t = new StateNode();
            var rList = new List<SweepEvent>();

            while (!q.IsEmpty())
            {
                HandleEventPoint(q.GetNextEvent());
            }

            return rList;
        }
    }
}