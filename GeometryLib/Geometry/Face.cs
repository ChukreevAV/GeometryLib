using static System.Net.Mime.MediaTypeNames;

namespace GeometryLib.Geometry
{
    public class Face
    {
        public HalfEdge? Outher { get; set; }

        public List<HalfEdge> Inner { get; set; } = new();

        public Face()
        {
            //
        }

        public void Reverse()
        {
            Outher?.Reverse();
            Outher = Outher?.Twin;
        }
    }
}