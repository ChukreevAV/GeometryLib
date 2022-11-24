using GeometryLib.Geometry;

namespace GeometryLib
{
    /// <summary>Узел дерева <see cref="BinarySearchTree{T}"/></summary>
    /// <typeparam name="T"></typeparam>
    public class BstNode<T> where T : class
    {
        /// <summary>Ключ</summary>
        public Point2d Key { get; private set; }

        /// <summary>Элемент данных</summary>
        public T Value { get; private set; }

        /// <summary>Левый потомок</summary>
        public BstNode<T>? Left { get; set; }

        /// <summary>Правый потомок</summary>
        public BstNode<T>? Right { get; set; }

        /// <summary>Конструктор</summary>
        /// <param name="k">Ключ</param>
        /// <param name="v">Элемент данных</param>
        public BstNode(Point2d k, T v)
        {
            Key = k;
            Value = v;
        }

        /// <summary>Добавить потомка</summary>
        /// <param name="k">Ключ</param>
        /// <param name="v">Элемент данных</param>
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

        /// <summary>Поиск по ключу</summary>
        /// <param name="k">Ключ</param>
        /// <returns>Элемент данных</returns>
        public T? Find(Point2d k)
        {
            if (Key.Equals(k)) return Value;
            if (k > Key) return Right?.Find(k) ?? default;
            return Left?.Find(k) ?? default;
        }

        /// <summary>Получить крайний левый узел</summary>
        /// <returns></returns>
        public BstNode<T> GetLeft() => Left == null ? this : Left.GetLeft();

        /// <summary>Получить крайний правый узел</summary>
        /// <returns></returns>
        public BstNode<T> GetRight() => Right == null ? this : Right.GetRight();

        /// <summary>Удалить узел по ключу</summary>
        /// <param name="k">Ключ</param>
        /// <returns>Признак пустого узла</returns>
        /// <exception cref="Exception"></exception>
        public bool Remove(Point2d k)
        {
            if (k > Key)
            {
                if (Right?.Remove(k) == true) Right = null;
            }
            else if (k < Key)
            {
                if (Left?.Remove(k) == true) Left = null;
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