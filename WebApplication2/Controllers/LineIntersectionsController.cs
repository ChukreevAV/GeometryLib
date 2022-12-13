using GeometryLib;
using GeometryLib.Geometry;
using GeometryLib.Intersections;

using Microsoft.AspNetCore.Mvc;

using WebApplication2.Models;

using static GeometryLib.Intersections.IntersectionsMethods;

namespace WebApplication2.Controllers
{
    [ApiController, Route("[controller]")]
    public class LineIntersectionsController
    {

        [HttpGet]
        public async Task<ActionResult<LineIntersectionsState>> GetLineIntersections()
        {
            var convexHull = new LineIntersectionsState(GetSampleData.GetRandomLine2ds(7));
            return await new ValueTask<ActionResult<LineIntersectionsState>>(convexHull);
        }

        private StateNode? _statusTree;
        private EventQueue? _eventQueue;
        private List<SweepEvent>? _result;

        private static List<SweepEvent> GetEventList(List<SweepEventDto>? result)
        {
            if (result == null) return new List<SweepEvent>();
            var rList = new List<SweepEvent>();

            foreach (var ev in result)
            {
                var p = ev.Point;
                var lines = ev.Lines.Cast<IEventLine>().ToList();
                rList.Add(new SweepEvent(p, lines));
            }
            return rList;
        }

        private static EventQueue GetEventQueue(LineIntersectionsState state)
        {
            var eventQueue = new EventQueue();

            if (state.SweepEvents == null) return eventQueue;
            foreach (var l in state.SweepEvents)
            {
                if (l.Point == null || l.Lines == null)
                    throw new ArgumentException("SweepEvent null");

                var p = l.Point;
                var lines = l.Lines.Cast<IEventLine>().ToList();
                eventQueue.AddEvent(p, lines);
            }

            return eventQueue;
        }

        /// <summary>Поиск точки нового события - пересечения</summary>
        /// <param name="sl">Левый отрезок</param>
        /// <param name="sr">Правый отрезок</param>
        /// <param name="p">Точка события</param>
        private void FindNewEvent(IEventLine? sl, IEventLine? sr, Point2d p)
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

            var rList = new List<IEventLine>();
            rList.AddRange(upList);
            rList.AddRange(cList);
            rList.AddRange(downList);
            rList = rList.Distinct().ToList();

            if (rList.Count > 1) _result.Add(new SweepEvent(ev.Point, rList));

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
                var list1 = new List<IEventLine>();
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

        [HttpPost]
        public async Task<ActionResult<LineIntersectionsState>> Next(LineIntersectionsState state)
        {
            _eventQueue = GetEventQueue(state);
            _statusTree = StateNodeDto.Load(state.Tree) ?? new StateNode();
            _result = GetEventList(state.Result);

            HandleEventPoint(_eventQueue.GetNextEvent());

            state.SetEvents(_eventQueue);
            state.SetTree(_statusTree);
            state.SetResult(_result);
            return await new ValueTask<ActionResult<LineIntersectionsState>>(state);
        }
    }
}