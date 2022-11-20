namespace GeometryLib
{
    public class StateNode
    {
        public Line2d? Line { get; set; }

        public Line2d? LeftLine { get; set; }

        public Line2d? RightLine { get; set; }

        public StateNode? LeftNode { get; set; }

        public StateNode? RightNode { get; set; }

        public StateNode() {}

        public StateNode(Line2d line)
        {
            Line = line;
            LeftLine = line;
        }

        public StateNode(Line2d line1, Line2d line2)
        {
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

        public bool IsRight(Line2d left, Line2d test)
        {
            var f1 = left.First();
            var f2 = test.First();

            return (f1.Y < f2.Y);
        }

        public void AddLine(Line2d line)
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
                    RightNode.AddLine(line);
                    return;
                }

                if (RightLine == null) RightLine = line;
                else
                {
                    RightNode = new StateNode(RightLine, line);
                    RightLine = null;
                }
                return;
            }

            if (LeftNode != null) LeftNode.AddLine(line);
            else
            {
                if (LeftLine == null) LeftLine = line;
                else
                {
                    LeftNode = new StateNode(LeftLine, line);
                    LeftLine = null;
                }
            }
        }
    }
}