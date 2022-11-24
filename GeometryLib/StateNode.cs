using GeometryLib.Geometry;

namespace GeometryLib
{
    public class StateNode
    {
        private StateNode? _parent;

        public Line2d? Line { get; set; }

        public Line2d? LeftLine { get; set; }

        public Line2d? RightLine { get; set; }

        public StateNode? LeftNode { get; set; }

        public StateNode? RightNode { get; set; }

        public StateNode() {}

        public StateNode(StateNode parent, Line2d line)
        {
            _parent = parent;
            Line = line;
            LeftLine = line;
        }

        public StateNode(StateNode parent, Line2d line1, Line2d line2)
        {
            _parent = parent;
            if (IsRight(line1, line2))
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
        
        public List<Line2d> Find(Point2d p)
        {
            var rList = new List<Line2d>();
            //if (Line?.Contain(p) == true) rList.Add(Line);

            if (LeftLine?.Contain(p) == true) rList.Add(LeftLine);
            if (RightLine?.Contain(p) == true) rList.Add(RightLine);

            if (LeftNode != null) rList.AddRange(LeftNode.Find(p));
            if (RightNode != null) rList.AddRange(RightNode.Find(p));

            return rList;
        }

        private Line2d? GetParentLeft(StateNode child)
        {
            if (RightNode == child) return LeftLine ?? LeftNode?.GetRight();

            return _parent?.GetParentLeft(this);
        }

        private Line2d? GetParentRight(StateNode child)
        {
            if (LeftNode == child) return RightLine ?? RightNode?.GetLeft();

            return _parent?.GetParentRight(this);
        }

        public Line2d? FindLeft(Line2d l)
        {
            var p = l.First();

            if (IsRight(p))
            {
                if (RightNode != null) return RightNode.FindLeft(l);
                if (RightLine == l)
                {
                    return LeftNode != null ? LeftNode.GetRight() : LeftLine;
                }
            }
            else
            {
                if (LeftNode != null) return LeftNode.FindLeft(l);
                if (LeftLine == l) return _parent?.GetParentLeft(this);
            }

            return null;
        }

        public Line2d? FindRight(Line2d l)
        {
            var p = l.First();

            if (IsRight(p))
            {
                if (RightNode != null) return RightNode.FindRight(l);
                if (RightLine == l) return _parent?.GetParentRight(this);
            }
            else
            {
                if (LeftNode != null) return LeftNode.FindRight(l);
                if (LeftLine == l)
                {
                    return RightNode != null ? RightNode.GetLeft() : RightLine;
                }
            }

            return null;
        }

        public Line2d? FindLeft(Point2d p)
        {
            if (IsRight(p))
            {
                if (RightNode != null) return RightNode.FindLeft(p);
                else
                    return LeftNode != null ? LeftNode.FindLeft(p) : LeftLine;
            }

            return LeftNode != null ? LeftNode.FindLeft(p) : LeftLine;
        }

        public Line2d? FindRight(Point2d p)
        {
            if (IsRight(p))
                return RightNode == null ? RightLine : RightNode.FindRight(p);

            if (LeftNode != null) return LeftNode.FindRight(p);

            return RightNode != null ? RightNode.FindRight(p) : RightLine;
        }

        public bool IsRight(Point2d p)
        {
            if (Line == null) return false; //!!!

            var tp = Line.First();
            return (tp.Y < p.Y);
        }

        public bool IsRight(Line2d left, Line2d test)
        {
            var f1 = left.First();
            var f2 = test.First();

            return (f1.Y < f2.Y);
        }

        public Line2d? GetLeft() => LeftLine ?? LeftNode?.GetLeft();

        public Line2d? GetRight() => RightLine ?? RightNode?.GetRight();

        public void Add(List<Line2d> lines)
        {
            foreach (var line in lines)
            {
                Add(line);
            }
        }

        public void Add(Line2d line)
        {
            if (Line == null)
            {
                Line = line;
                LeftLine = line;
                return;
            }

            if (IsRight(Line, line))
            {
                if (RightNode != null)
                {
                    RightNode.Add(line);
                    return;
                }

                if (RightLine == null) RightLine = line;
                else
                {
                    RightNode = 
                        new StateNode(this, RightLine, line);
                    RightLine = null;
                }
                return;
            }

            if (LeftNode != null) LeftNode.Add(line);
            else
            {
                if (LeftLine == null) LeftLine = line;
                else
                {
                    LeftNode = 
                        new StateNode(this, LeftLine, line);
                    LeftLine = null;
                }
            }
        }

        private void Replace(StateNode oldNode, Line2d? line)
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

        public void Remove(List<Line2d> lines)
        {
            foreach (var line in lines)
            {
                Remove(line);
            }
        }

        private void RemoveRight(Line2d line)
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

        private void RemoveLeft(Line2d line)
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

        public void Remove(Line2d line)
        {
            if (Line == null) return;

            if (IsRight(Line, line)) RemoveRight(line);
            else RemoveLeft(line);
        }
    }
}