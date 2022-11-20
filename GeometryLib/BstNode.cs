namespace GeometryLib
{
    public class BstNode<T>
    {
        public Point2d Key { get; }

        public T Value { get; }

        public BstNode<T>? Left { get; set; }

        public BstNode<T>? Right { get; set; }

        public BstNode(Point2d k, T v)
        {
            Key = k;
            Value = v;
        }

        public void Add(Point2d k, T v)
        {
            if (k > Key)
            {
                if (Right == null) Right = new BstNode<T>(k, v);
                else Right.Add(k, v);
            }
            else
            {
                if (Left == null) Left = new BstNode<T>(k, v);
                else Left.Add(k, v);
            }
        }
    }
}