using GeometryLib.Geometry;
using GeometryLib.Trees;

namespace GeometryLib.Intersections
{
    /// <summary>Очередь событий</summary>
    public class EventQueue
    {
        private BinarySearchTree<SweepEvent> Tree { get; } = new();

        /// <summary>Добавить событие</summary>
        /// <param name="p"></param>
        public void AddEvent(Point2d p)
        {
            var even = Tree.Find(p);
            if (even != null) return;
            even = new SweepEvent(p);
            Tree.Add(p, even);
        }

        public void AddEvent(Point2d p, List<IEventLine> lines)
        {
            var even = Tree.Find(p);
            if (even != null) return;
            even = new SweepEvent(p, lines);
            Tree.Add(p, even);
        }

        /// <summary>Добавить событие</summary>
        /// <param name="p"></param>
        /// <param name="line"></param>
        public void AddEvent(Point2d p, IEventLine line)
        {
            var even = Tree.Find(p);
            if (even == null) Tree.Add(p, new SweepEvent(p, line));
            else even.Add(line);
        }

        /// <summary>Добавить событие</summary>
        /// <param name="p"></param>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        public void AddEvent(Point2d p, IEventLine line1, IEventLine line2)
        {
            var even = Tree.Find(p);
            if (even == null)
            {
                even = new SweepEvent(p, line1, line2);
                Tree.Add(p, even);
            }
            else
            {
                even.Add(line1);
                even.Add(line2);
            }
        }

        /// <summary>Добавить события отрезка</summary>
        /// <param name="line"></param>
        public void AddEvent(IEventLine line)
        {
            AddEvent(line.First(), line);
            AddEvent(line.Last());
        }

        /// <summary>Получить следующее событие</summary>
        /// <returns></returns>
        public SweepEvent? GetNextEvent()
        {
            var nn = Tree.GetRight();
            if (nn == null) return null;
            var ev = nn.Value;
            Tree.Remove(nn.Key);
            return ev;
        }

        public List<SweepEvent> GetList()
        {
            var list = new List<SweepEvent>();
            while (!IsEmpty())
            {
                var ev = GetNextEvent();
                if (ev != null) list.Add(ev);
            }

            return list;
        }

        /// <summary>Признак пустой очереди</summary>
        /// <returns></returns>
        public bool IsEmpty() => Tree.IsEmpty();
    }
}