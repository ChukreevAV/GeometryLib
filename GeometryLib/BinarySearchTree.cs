using GeometryLib.Geometry;

namespace GeometryLib
{
    /// <summary>Двоичное дерево поиска по точкам</summary>
    /// <typeparam name="T">Тип данных</typeparam>
    public class BinarySearchTree<T> where T : class
    {
        /// <summary>Корневой узел</summary>
        private BstNode<T>? Root { get; set; }

        /// <summary>Добавление узла</summary>
        /// <param name="k">Ключевая точка</param>
        /// <param name="v">Тип данных</param>
        public void Add(Point2d k, T v)
        {
            if (Root == null) Root = new BstNode<T>(k, v);
            else Root.Add(k, v);
        }

        /// <summary>Поиск данных</summary>
        /// <param name="k">Ключ</param>
        /// <returns>Элемент данных</returns>
        public T? Find(Point2d k) => Root?.Find(k) ?? default;

        /// <summary>Получаем крайне левый узел</summary>
        /// <returns></returns>
        public BstNode<T>? GetLeft() => Root?.GetLeft();

        /// <summary>Получаем крайне правый узел</summary>
        /// <returns></returns>
        public BstNode<T>? GetRight() => Root?.GetRight();

        /// <summary>Признак пустого дерева</summary>
        /// <returns></returns>
        public bool IsEmpty() => Root == null;

        /// <summary>Удаление данных</summary>
        /// <param name="k"></param>
        public void Remove(Point2d k)
        {
            if (Root?.Remove(k) == true) Root = null;
        }
    }
}