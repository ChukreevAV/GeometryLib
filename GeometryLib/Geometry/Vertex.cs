namespace GeometryLib.Geometry
{
    public class Vertex
    {
        public Point2d Point { get; set; }

        public HalfEdge? IncidentEdge { get; set; }

        public Vertex(Point2d point)
        {
            Point = point;
        }

        public Vertex(Point2d point, HalfEdge incidentEdge)
        {
            Point = point;
            IncidentEdge = incidentEdge;
        }

        public bool IsEquals(Point2d point) 
            => Point.Distance(point) < Point2d.Epsilon;
    }
}