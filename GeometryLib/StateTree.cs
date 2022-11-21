namespace GeometryLib
{
    public class StateTree
    {
        private StateNode _root = new ();

        public void Add(Line2d line)
        {
            _root.Add(line);
        }

        public List<Line2d> Find(Point2d p) => _root.Find(p);

        public void Remove(Line2d line)
        {
            //
        }
    }
}