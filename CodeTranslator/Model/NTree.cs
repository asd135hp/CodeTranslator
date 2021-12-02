using System;
using System.Collections.Generic;

namespace CodeTranslator.Model
{
    public class NTree : IDisposable
    {
        protected List<NTree> _children = null;

        public bool IsParentNull => Parent == null;
        public int N => _children?.Count ?? 0;
        public int Depth { get; private set; }
        public NTree Parent { get; internal set; }
        public IList<NTree> Children => _children ?? new List<NTree>();
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

        #region Adding operation

        /// <summary>
        /// Add a new node as a child of this node specifically without ordering
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
        /// Add a range of new nodes as children of this node specifically without ordering
        /// </summary>
        /// <param name="nodes"></param>
        public void AddChildNodeRange(params NTree[] nodes)
        {
            foreach (var node in nodes) AddChildNode(node);
        }

        #endregion

        #region Swapping operation

        /// <summary>
        /// Swap this node with other node in the tree
        /// </summary>
        /// <param name="otherNode">Any node in the tree but the </param>
        /// <exception cref="ArgumentException"></exception>
        public void SwapNode(NTree otherNode)
        {
            if (IsParentNull && otherNode.IsParentNull)
                throw new ArgumentException("No point in swapping two root nodes!");

            if ((IsParentNull && otherNode.RootNode.Equals(this)) ||
                (otherNode.IsParentNull && RootNode.Equals(otherNode)))
                throw new ArgumentException("Could not swap with root node of the same tree!");

            // index of children in subtree's children list
            int thisIndex = Parent._children.IndexOf(this),
                otherIndex = otherNode.Parent._children.IndexOf(otherNode);

            // swap children first
            Parent._children[thisIndex] = otherNode;
            otherNode.Parent._children[otherIndex] = this;

            // then swap parents
            (Parent, otherNode.Parent) = (otherNode.Parent, Parent);
        }

        /// <summary>
        /// Swap child nodes whenever possible
        /// </summary>
        /// <param name="start">Out of bounds index will render this method doing nothing</param>
        /// <param name="end">Out of bounds index will render this method doing nothing</param>
        public void SwapChildNode(int start, int end)
        {
            if (start < 0 || start > N)
                throw new ArgumentOutOfRangeException($"Could not find child node at {start}");
            if (end < 0 || end > N)
                throw new ArgumentOutOfRangeException($"Could not find child node at {end}");
            if (start == end) return;

            (_children[start], _children[end]) = (_children[end], _children[start]);
        }

        #endregion

        #region Deleting operation

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

        /// <summary>
        /// Delete a node from the tree at the current position
        /// </summary>
        /// <param name="index">
        /// 0-based index representing the index of the child
        /// node in this specific subtree at Depth + 1
        /// </param>
        public void DeleteNthNode(int childIndex)
        {
            if (childIndex < 0 || childIndex < 0)
                throw new ArgumentOutOfRangeException($"Could not find child node at {childIndex}");

            _children[childIndex].Dispose();
            _children.RemoveAt(childIndex);
        }

        #endregion

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

        public void UpdateData(T newData) => Data = newData;

        public void UpdateNthChildData(int index, T newData)
        {
            if (index < 0 || index > N)
                throw new ArgumentOutOfRangeException($"Could not find child node at {index}");

            (_children[index] as NTree<T>).Data = newData;
        }

        public void DeleteData() => Data = default;
    }
}
