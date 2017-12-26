using System;
using System.Collections;
using System.Collections.Generic;

namespace Euler.DataStructures.BinaryTree
{
    public class Tree<TKey> : IEnumerable<TKey> 
    where TKey : IComparable<TKey>
    {
        public TreeNode<TKey> Root { get; private set; }

        public void Add(TKey value)
        {
            if (Root == null)
            {
                Root = new TreeNode<TKey>(value);
            }
            else
            {
                Add(Root, value);
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
        
        public bool Contains(TKey item)
        {
            var current = Root;

            while (current != null)
            {
                if (current.Value.CompareTo(item) == 0)
                    return true;

                current =
                    current.Value.CompareTo(item) > 0
                    ? current.Left
                    : current.Right;
            }

            return false;
        }
        
        public int Count { get; private set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            var values = new List<TKey>();
            InOrderWalk(Root, key => values.Add(key));
            return values.GetEnumerator();
        }

        public static void InOrderWalk(TreeNode<TKey> root, Action<TKey> action)
        {
            if (root == null)
                return;

            InOrderWalk(root.Left, action);
            action(root.Value);
            InOrderWalk(root.Right, action);
        }
    }
}