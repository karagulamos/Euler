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
            var values = new List<TKey>();
            Walk(key => values.Add(key));
            return values.GetEnumerator();
        }

        public void Walk(Action<TKey> action)
        {
            InOrderTraversal(_root, action);
        }

        private static void InOrderTraversal(TreeNode<TKey> root, Action<TKey> action)
        {
            if (root == null)
                return;

            InOrderTraversal(root.Left, action);
            action(root.Value);
            InOrderTraversal(root.Right, action);
        }
    }
}