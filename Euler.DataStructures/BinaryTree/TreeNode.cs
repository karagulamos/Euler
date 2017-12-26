namespace Euler.DataStructures.BinaryTree
{
    public class TreeNode<TKey> 
    {
        public TreeNode(TKey value)
        {
            Value = value;
        }

        public TKey Value { get; set; }
        public TreeNode<TKey> Left { get; set; }
        public TreeNode<TKey> Right { get; set; }
    }
}