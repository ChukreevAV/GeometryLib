using GeometryLib.Geometry;

namespace GeometryLib.Intersections
{
    public class EdgeLine : Line2d, IEventLine
    {
        private HalfEdge _edge;

        public EdgeLine(HalfEdge edge)
        {
            _edge = edge;
            Start = _edge.Origin.Point;
            End = _edge.End?.Point ?? new Point2d(0, 0);
        }
    }
}