using GeometryLib.Geometry;

namespace GeometryLib.Intersections
{
    /// <summary>Узел структуры состояния алгоритма поиска пересечения</summary>
    public class StateNode
    {
        private StateNode? _parent;

        /// <summary>Ключевая линия</summary>
        public IEventLine? Line { get; set; }

        /// <summary>Левая линия</summary>
        public IEventLine? LeftLine { get; set; }

        /// <summary>Правая линия</summary>
        public IEventLine? RightLine { get; set; }

        /// <summary>Левый узел</summary>
        public StateNode? LeftNode { get; set; }

        /// <summary>Правый узел</summary>
        public StateNode? RightNode { get; set; }

        /// <summary>Определяем направление для точки</summary>
        /// <param name="p">Точка на линии заметания</param>
        /// <returns></returns>
        private bool IsRight(Point2d p)
        {
            if (Line == null) return false;
            var tp = Line.GetPointByY(p.Y);
            return p.X > tp.X;
        }

        private static double Distance(IEventLine test, Point2d p)
        {
            var tp = test.GetPointByY(p.Y);
            return tp.X - p.X;
        }

        /// <summary>Определяем положение отрезка относительно опорного</summary>
        /// <param name="p">Точка на линии заметания</param>
        /// <param name="left">Опорный отрезок</param>
        /// <param name="test">Проверяемый отрезок</param>
        /// <returns></returns>
        private static bool IsRight(Point2d p, IEventLine left, IEventLine test)
        {
            var p1 = left.GetPointByY(p.Y);
            var p2 = test.GetPointByY(p.Y);
            return p2.X > p1.X;
        }

        /// <summary>Конструктор</summary>
        public StateNode() {}

        public StateNode(StateNode? parent)
        {
            _parent = parent;
        }

        /// <summary>Конструктор</summary>
        /// <param name="parent">Родитель</param>
        /// <param name="p">Точка на линии заметания</param>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        public StateNode(StateNode parent, Point2d p, IEventLine line1, IEventLine line2)
        {
            _parent = parent;
            if (IsRight(p, line1, line2))
            {
                Line = line1;
                LeftLine = line1;
                RightLine = line2;
            }
            else
            {
                Line = line2;
                LeftLine = line2;
                RightLine = line1;
            }
        }

        /// <summary>Поиск отрезков по точке</summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<IEventLine> Find(Point2d p)
        {
            var rList = new List<IEventLine>();

            if (LeftLine?.Contain(p) == true) rList.Add(LeftLine);
            if (RightLine?.Contain(p) == true) rList.Add(RightLine);

            if (LeftNode != null) rList.AddRange(LeftNode.Find(p));
            if (RightNode != null) rList.AddRange(RightNode.Find(p));

            return rList;
        }

        /// <summary>Получаем левый отрезок через предка</summary>
        /// <param name="child">Дочерний узел</param>
        /// <returns>Левый отрезок</returns>
        private IEventLine? GetParentLeft(StateNode child)
        {
            if (RightNode == child) return LeftLine ?? LeftNode?.GetRight();
            return _parent?.GetParentLeft(this);
        }

        /// <summary>Получаем правый отрезок через предка</summary>
        /// <param name="child">Дочерний узел</param>
        /// <returns>Правый отрезок</returns>
        private IEventLine? GetParentRight(StateNode child)
        {
            if (LeftNode == child) return RightLine ?? RightNode?.GetLeft();
            return _parent?.GetParentRight(this);
        }

        /// <summary>Получить отрезок слева от заданного отрезка</summary>
        /// <param name="p">Точка на линии заметания</param>
        /// <param name="l">Заданный отрезок</param>
        /// <returns>Левый отрезок</returns>
        public IEventLine? FindLeft(Point2d p, IEventLine l)
        {
            IEventLine? result = null;

            if (RightNode != null) result = RightNode.FindLeft(p, l);
            if (RightLine == l) result = LeftNode != null ? LeftNode.GetRight() : LeftLine;

            if (result != null) return result;

            if (LeftNode != null) result = LeftNode.FindLeft(p, l);
            if (LeftLine == l) result = _parent?.GetParentLeft(this);

            return result;
        }

        /// <summary>Получить отрезок справа от заданного отрезка</summary>
        /// <param name="p">Точка на линии заметания</param>
        /// <param name="l">Заданный отрезок</param>
        /// <returns>Правый отрезок</returns>
        public IEventLine? FindRight(Point2d p, IEventLine l)
        {
            IEventLine? result = null;

            if (RightNode != null) result = RightNode.FindRight(p, l);
            if (RightLine == l) result = _parent?.GetParentRight(this);

            if (result != null) return result;

            if (LeftNode != null) result = LeftNode.FindRight(p, l);
            if (LeftLine == l) result = RightNode != null ? RightNode.GetLeft() : RightLine;

            return result;
        }

        /// <summary>Получить крайне левый отрезок</summary>
        /// <param name="p">Точка на линии заметания</param>
        /// <returns>Крайне левый отрезок</returns>
        public IEventLine? FindLeft(Point2d p)
        {
            var ll = LeftNode != null ? LeftNode?.FindLeft(p) : LeftLine;
            var rl = RightNode != null ? RightNode?.FindLeft(p) : RightLine;

            if (ll == null) return rl;
            if (rl == null) return ll;
            var d1 = Distance(ll, p);
            var d2 = Distance(rl, p);

            if (d1 > 0) return rl;
            if (d2 > 0) return ll;

            return d1 > d2 ? ll : rl;
        }

        /// <summary>Получить крайне правый отрезок</summary>
        /// <param name="p">Точка на линии заметания</param>
        /// <returns>Крайне правый отрезок</returns>

        public IEventLine? FindRight(Point2d p)
        {
            var ll = LeftNode != null ? LeftNode?.FindRight(p) : LeftLine;
            var rl = RightNode != null ? RightNode?.FindRight(p) : RightLine;

            if (ll == null) return rl;
            if (rl == null) return ll;
            var d1 = Distance(ll, p);
            var d2 = Distance(rl, p);

            if (d1 < 0) return rl;
            if (d2 < 0) return ll;

            return d1 < d2 ? ll : rl;
        }

        private IEventLine? GetLeft() => LeftLine ?? LeftNode?.GetLeft();

        private IEventLine? GetRight() => RightLine ?? RightNode?.GetRight();

        /// <summary>Добавление отрезков в структуру</summary>
        /// <param name="eventPoint">Точка на линии заметания</param>
        /// <param name="lines">Отрезки для добавления</param>
        public void Add(Point2d eventPoint, List<IEventLine> lines)
        {
            foreach (var line in lines)
            {
                var p = line.GetPointByY(eventPoint.Y);
                Add(p, line);
            }
        }

        /// <summary>Добавление отрезка в структуру</summary>
        /// <param name="eventPoint">Точка на линии заметания</param>
        /// <param name="line">Отрезок для отопления</param>
        public void Add(Point2d eventPoint, IEventLine line)
        {
            if (Line == null)
            {
                Line = line;
                LeftLine = line;
                return;
            }

            if (IsRight(eventPoint))
            {
                if (RightNode != null)
                {
                    RightNode.Add(eventPoint, line);
                    return;
                }

                if (RightLine == null) RightLine = line;
                else
                {
                    RightNode = new StateNode(this, eventPoint, RightLine, line);
                    RightLine = null;
                }
                return;
            }

            if (LeftNode != null) LeftNode.Add(eventPoint, line);
            else
            {
                if (LeftLine == null) LeftLine = line;
                else
                {
                    LeftNode = new StateNode(this, eventPoint, line, LeftLine);
                    LeftLine = null;
                }
            }
        }

        private void Replace(StateNode oldNode, IEventLine? line)
        {
            if (RightNode == oldNode)
            {
                RightNode = null;
                RightLine = line;
            }
            else
            {
                LeftNode = null;
                LeftLine = line;
                Line = line;
            }
        }

        private void Replace(StateNode oldNode, StateNode? newNode)
        {
            if (RightNode == oldNode)
            {
                RightNode = newNode;
                if (newNode != null) newNode._parent = this;
            }
            else
            {
                LeftNode = newNode;
                if (newNode != null) newNode._parent = this;
                Line = LeftNode?.GetRight();
            }
        }

        /// <summary>Удалить отрезки</summary>
        /// <param name="lines">Список отрезков</param>
        public void Remove(List<IEventLine> lines)
        {
            foreach (var line in lines)
            {
                Remove(line);
            }
        }

        private void RemoveRight(IEventLine line)
        {
            if (RightLine == line)
            {
                RightLine = null;
                if (_parent == null) return;

                if (LeftNode == null)
                {
                    _parent.Replace(this, LeftLine);
                    return;
                }

                _parent.Replace(this, LeftNode);
                return;
            }

            RightNode?.Remove(line);
        }

        private void RemoveLeft(IEventLine line)
        {
            if (LeftLine == line)
            {
                LeftLine = null;
                if (_parent == null)
                {
                    if (RightNode == null)
                    {
                        Replace(this, RightLine);
                        RightLine = null;
                        return;
                    }

                    Replace(this, RightNode);
                    RightNode = null;

                    return;
                }

                if (RightNode == null)
                {
                    _parent.Replace(this, RightLine);
                    return;
                }

                _parent.Replace(this, RightNode);
                return;
            }

            LeftNode?.Remove(line);

            if (Line == line) Line = LeftNode?.GetRight();
        }

        /// <summary>Удалить отрезок</summary>
        /// <param name="line">Отрезок на удаление</param>
        public void Remove(IEventLine line)
        {
            if (Line == null) return;

            RemoveRight(line);
            RemoveLeft(line);
        }
    }
}