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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return InOrderTraversal(_root);

            //var values = new List<TKey>();
            //Walk(key => values.Add(key));
            //return values.GetEnumerator();
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

        public void Walk(Action<TKey> action)
        {
            InOrderTraversal(_root, action);
        }

        private static void InOrderTraversal(TreeNode<TKey> currentNode, Action<TKey> process)
        {
            var enumerator = InOrderTraversal(currentNode);

            while (enumerator.MoveNext())
            {
                process(enumerator.Current);
            }

            //if (currentNode == null) return;

            //InOrderTraversal(currentNode.Left, process);
            //process(currentNode.Value);
            //InOrderTraversal(currentNode.Right, process);
        }
    }
}