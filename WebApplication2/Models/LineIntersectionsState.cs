using GeometryLib.Geometry;
using GeometryLib.Intersections;

namespace WebApplication2.Models
{
    public class LineIntersectionsState
    {
        public List<IndexedLine2d>? Lines { get; set; }

        public List<SweepEventDto>? SweepEvents { get; set; }

        public List<SweepEventDto>? Result { get; set; }

        public StateNodeDto? Tree { get; set; }

        public LineIntersectionsState() {}

        private SweepEventDto ConvertEvent(SweepEvent ev)
        {
            return new SweepEventDto
            {
                Point = ev.Point,
                Lines = ev.Lines.Cast<IndexedLine2d>().ToList()
            };
        }

        public void SetEvents(EventQueue eventQueue)
        {
            SweepEvents = eventQueue.GetList().Select(ConvertEvent).ToList();
        }

        public void SetResult(List<SweepEvent>? eventQueue)
        {
            if (eventQueue == null) return;
            Result = eventQueue.Select(ConvertEvent).ToList();
        }

        public void SetTree(StateNode node)
        {
            Tree = StateNodeDto.Read(node);
        }

        public LineIntersectionsState(List<Line2d> lines)
        {
            Lines = IndexedLine2d.Create(lines);

            var eventQueue = new EventQueue();

            foreach (var l in Lines)
            {
                eventQueue.AddEvent(l);
            }

            SetEvents(eventQueue);
        }
    }
}