using GeometryLib.Geometry;

namespace WebApplication2.Models
{
    public class LineIntersectionsState
    {
        public List<IndexedLine2d>? Lines { get; set; }

        public List<SweepEventDto>? SweepEvents { get; set; }

        public LineIntersectionsState() {}

        public LineIntersectionsState(List<Line2d> lines)
        {
            Lines = IndexedLine2d.Create(lines);

            SweepEvents = new List<SweepEventDto>();

            foreach (var line in Lines)
            {
                SweepEvents.Add(new SweepEventDto(line.First(), line));
            }
        }
    }
}