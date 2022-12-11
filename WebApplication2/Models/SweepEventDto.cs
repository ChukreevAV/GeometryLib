using GeometryLib.Geometry;

namespace WebApplication2.Models
{
    public class SweepEventDto
    {
        /// <summary>Точка события</summary>
        public Point2d? Point { get; set; }

        /// <summary>Список отрезков совпадающих в точке</summary>
        public List<IndexedLine2d>? Lines { get; set; }

        public SweepEventDto() {}

        public SweepEventDto(Point2d p, IndexedLine2d l)
        {
            Point = p;
            Lines = new List<IndexedLine2d> { l };
        }
    }
}