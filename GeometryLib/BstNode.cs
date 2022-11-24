namespace GeometryLib
{
    public class BstNode<T> where T : class
    {
        public Point2d Key { get; private set; }

        public T Value { get; private set; }

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

        public T? Find(Point2d k)
        {
            if (Key.Equals(k)) return Value;
            if (k > Key) return Right?.Find(k) ?? default;
            return Left?.Find(k) ?? default;
        }

        public BstNode<T> GetLeft() => Left == null ? this : Left.GetLeft();

        public BstNode<T> GetRight()
        {
            if (Right == null)
            {
                //return Left != null ? this.Left.GetRight() : this;
                return this;
            }
            else
                return Right.GetRight();
        }

        public bool Remove(Point2d k)
        {
            if (k > Key)
            {
                var b1 = Right?.Remove(k);
                if (b1 == true) Right = null;
            }
            else if (k < Key)
            {
                var b2 = Left?.Remove(k);
                if (b2 == true) Left = null;
            }
            else if (k.Equals(Key))
            {
                if (Left == null && Right == null) return true;
                if (Left == null || Right == null)
                {
                    var m = Left ?? Right;
                    if (m == null) 
                        throw new Exception("Почему то m = null");
                    Key = m.Key;
                    Value = m.Value;
                    Left = m.Left;
                    Right = m.Right;
                    return false;
                }

                if (Right is { Left: null })
                {
                    Key = Right.Key;
                    Value = Right.Value;
                    Right = Right.Right;
                }
                else if (Right != null)
                {
                    var m = Right.Left;
                    Key = m.Key;
                    Value = m.Value;
                    var b1 = m.Remove(Key);
                    if (b1) Right.Left = null;
                }
            }

            return false;
        }
    }
}