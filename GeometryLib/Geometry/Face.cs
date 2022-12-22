using System.Linq;

namespace GeometryLib.Geometry
{
    public class Face
    {
        public List<string> Names { get; } = new();

        public string FullName => string.Join(" ", Names);

        public HalfEdge? Outher { get; set; }

        public List<HalfEdge> Inner { get; set; } = new();

        public List<HalfEdge> GetEdges()
        {
            var fistEdge = Outher;
            var workEdge = Outher.Next;

            var edges = new List<HalfEdge> { fistEdge, workEdge };

            while (workEdge != fistEdge)
            {
                workEdge = workEdge.Next;
                edges.Add(workEdge);
            }
            return edges;
        }

        public Face() { }

        public Face(string name)
        {
            Names.Add(name);
        }

        public Face(IEnumerable<string> names)
        {
            Names.AddRange(names);
        }

        public void Reverse()
        {
            Outher?.Reverse();
            Outher = Outher?.Twin;
        }
    }
}