using System;
using System.Collections;
using System.Collections.Generic;

namespace Euler.DataStructures.BinaryTree
{
    public class Tree<TKey> : IEnumerable<TKey>
    where TKey : IComparable
    {
        private TreeNode<TKey> _root;

        public void Add(TKey value)
        {
            if (_root == null)
            {
                _root = new TreeNode<TKey>(value);
            }
            else
            {
                Add(_root, value);
            }

            Count++;
        }

        private static void Add(TreeNode<TKey> root, TKey value)
        {
            while (true)
            {
                if (root.Value.CompareTo(value) > 0)
                {
                    if (root.Left == null)
                    {
                        root.Left = new TreeNode<TKey>(value);
                        return;
                    }

                    root = root.Left;
                    continue;
                }

                if (root.Right == null)
                {
                    root.Right = new TreeNode<TKey>(value);
                    return;
                }

                root = root.Right;
            }
        }

        public int Count { get; private set; }

        public void Walk(Action<TKey> process)
        {
            using (var enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    process(enumerator.Current);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return InOrderTraversal(_root);
        }

        private static IEnumerator<TKey> InOrderTraversal(TreeNode<TKey> currentNode)
        {
            if (currentNode == null) yield break;

            var nodes = new Stack<TreeNode<TKey>>(new[] { currentNode });
            var visitLeft = true;

            while (nodes.Count > 0)
            {
                while (visitLeft && currentNode.Left != null)
                {
                    nodes.Push(currentNode);
                    currentNode = currentNode.Left;
                }

                yield return currentNode.Value;

                visitLeft = currentNode.Right != null;
                currentNode = currentNode.Right ?? nodes.Pop();
            }
        }
    }
}