using System;
using System.Collections;
using System.Collections.Generic;

namespace Euler.DataStructures.BinaryTree
{
    public class Tree<TKey> : IEnumerable<TKey>
    where TKey : IComparable<TKey>
    {
        public static Tree<TKey> Create() => new Tree<TKey>();

        public static Tree<TKey> Create(IEnumerable<TKey> keys)
        {
            return new Tree<TKey>(keys);
        }

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

        private static void Add(TreeNode<TKey> currentNode, TKey value)
        {
            while (true)
            {
                var addingToLeft = currentNode.Value.CompareTo(value) > 0;

                if (addingToLeft && currentNode.Left == null)
                {
                    currentNode.Left = new TreeNode<TKey>(value);
                    break;
                }

                if (!addingToLeft && currentNode.Right == null)
                {
                    currentNode.Right = new TreeNode<TKey>(value);
                    break;
                }

                currentNode = addingToLeft ? currentNode.Left : currentNode.Right;
            }
        }

        public int Count { get; private set; }

        public void Walk(Action<TKey> process)
        {
            foreach (var nodeValue in this)
                process(nodeValue);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TKey> GetEnumerator() => InOrderTraversal(_root);

        private static IEnumerator<TKey> InOrderTraversal(TreeNode<TKey> currentNode)
        {
            if (currentNode == null) yield break;

            var nodes = new Stack<TreeNode<TKey>>(new[] { currentNode });

            var visitingLeft = true;

            while (nodes.Count > 0)
            {
                while (visitingLeft && currentNode.Left != null)
                {
                    nodes.Push(currentNode);
                    currentNode = currentNode.Left;
                }

                yield return currentNode.Value;

                visitingLeft = currentNode.Right != null;
                currentNode = currentNode.Right ?? nodes.Pop();
            }
        }

        protected Tree() { }

        protected Tree(IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
                Add(key);
        }

        private TreeNode<TKey> _root;
    }
}