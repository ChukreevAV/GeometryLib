using GeometryLib.Intersections;

namespace WebApplication2.Models
{
    public class StateNodeDto
    {
        /// <summary>Ключевая линия</summary>
        public IndexedLine2d? Line { get; set; }

        /// <summary>Левая линия</summary>
        public IndexedLine2d? LeftLine { get; set; }

        /// <summary>Правая линия</summary>
        public IndexedLine2d? RightLine { get; set; }

        /// <summary>Левый узел</summary>
        public StateNodeDto? LeftNode { get; set; }

        /// <summary>Правый узел</summary>
        public StateNodeDto? RightNode { get; set; }

        public static StateNodeDto? Read(StateNode? node)
        {
            if (node == null) return null;

            return new StateNodeDto
            {
                Line = node.Line as IndexedLine2d,
                LeftLine = node.LeftLine as IndexedLine2d,
                RightLine = node.RightLine as IndexedLine2d,
                LeftNode = Read(node.LeftNode),
                RightNode = Read(node.RightNode)
            };
        }

        public static StateNode? Load(StateNodeDto? node)
        {
            if (node == null) return null;

            return new StateNode
            {
                Line = node.Line,
                LeftLine = node.LeftLine,
                RightLine = node.RightLine,
                LeftNode = Load(node.LeftNode),
                RightNode = Load(node.RightNode)
            };
        }
    }
}