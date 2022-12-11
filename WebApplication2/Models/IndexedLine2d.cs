using GeometryLib.Geometry;

namespace WebApplication2.Models
{
    public class IndexedLine2d : Line2d
    {
        public int Id { get; set; }

        public IndexedLine2d() {}

        public IndexedLine2d(int id, Line2d line)
        {
            Id = id;
            Start = line.Start;
            End = line.End;
        }

        public static List<IndexedLine2d> Create(List<Line2d> lines)
        {
            var id = 1;
            return lines.Select(line => new IndexedLine2d(id++, line)).ToList();
        }
    }
}