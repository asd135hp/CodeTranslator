using System;
using System.Collections.Generic;

namespace CodeTranslator.Tree
{
    public class NTree : IDisposable
    {
        public NTree Parent { get; internal set; }

        protected readonly List<NTree> _children = null;
        public IList<NTree> Children => _children ?? new List<NTree>();

        public bool IsParentNull => Parent == null;
        public int N => _children?.Count ?? 0;
        public int Depth { get; private set; }
        public NTree RootNode
        {
            get
            {
                var currentNode = this;
                while(currentNode.Parent != null)
                {
                    currentNode = currentNode.Parent;
                }
                return currentNode;
            }
        }

        public NTree()
        {
            Parent = null;
            _children = new List<NTree>();
        }

        /// <summary>
        /// Add new node to the tree without ordering
        /// </summary>
        /// <param name="node"></param>
        public void AddChildNode(NTree node)
        {
            if (!Equals(node))
            {
                node.Parent = this;
                node.Depth = Depth + 1;
                _children.Add(node);
            }
        }

        /// <summary>
        /// Swap this node with other node in the tree
        /// </summary>
        /// <param name="node">Any node in the tree but the </param>
        /// <exception cref="ArgumentException"></exception>
        public void SwapNode(NTree node)
        {
            if (IsParentNull && node.IsParentNull)
                throw new ArgumentException("No point in swapping two root nodes!");

            if ((IsParentNull && node.RootNode.Equals(this)) ||
                (node.IsParentNull && RootNode.Equals(node)))
                throw new ArgumentException("Could not swap with root node of the same tree!");

            // index of children in subtree's children list
            int thisIndex = Parent._children.IndexOf(this),
                otherIndex = node.Parent._children.IndexOf(node);

            // swap children first
            Parent._children[thisIndex] = node;
            node.Parent._children[otherIndex] = this;

            // then swap parents
            (Parent, node.Parent) = (node.Parent, Parent);
        }

        /// <summary>
        /// Swap child nodes whenever possible
        /// </summary>
        /// <param name="start">Out of bounds index will render this method doing nothing</param>
        /// <param name="end">Out of bounds index will render this method doing nothing</param>
        public void SwapNthChildNode(int start, int end)
        {
            if (start < 0 || end < 0 || start > N || end > N) return;

            (_children[start], _children[end]) = (_children[end], _children[start]);
        }

        /// <summary>
        /// Delete a node from the tree at the current position
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNode(NTree node)
        {
            // its children could never be terminal nodes
            if(!Equals(node) && _children.Remove(node))
                node.Dispose();
        }

        public void DeleteNthNode(int index)
        {
            _children[index].Dispose();
            _children.RemoveAt(index);
        }
    
        public void Dispose()
        {
            Parent = null;
            foreach(var childNode in _children) childNode.Dispose();
            _children.Clear();
        }
    }

    public class NTree<T> : NTree
    {
        public T Data { get; private set; }

        public NTree(T data) : base()
        {
            Data = data;
        }
    }
}
