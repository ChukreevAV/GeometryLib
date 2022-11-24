using GeometryLib.Geometry;

namespace GeometryLib
{
    public class BinarySearchTree<T> where T : class
    {
        private BstNode<T>? Root { get; set; }

        public void Add(Point2d k, T v)
        {
            if (Root == null) Root = new BstNode<T>(k, v);
            else Root.Add(k, v);
        }

        public T? Find(Point2d k) => Root?.Find(k) ?? default;

        public BstNode<T>? GetLeft() => Root?.GetLeft();

        public BstNode<T>? GetRight() => Root?.GetRight();

        public bool IsEmpty() => Root == null;

        public void Remove(Point2d k)
        {
            if (Root?.Remove(k) == true) Root = null;
        }
    }
}