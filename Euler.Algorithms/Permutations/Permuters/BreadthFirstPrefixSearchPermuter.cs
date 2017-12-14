using System;
using System.Collections.Generic;

namespace Euler.Algorithms.Permutations.Permuters
{
    public class BreadthFirstPrefixSearchPermuter : IPermuter
    {
        public void Execute(string pattern)
        {
            Permute(string.Empty, pattern);
        }

        private static void Permute(string prefix, string suffix)
        {
            Queue<ValueNode> temp = new Queue<ValueNode>();

            temp.Enqueue(new ValueNode
            {
                Prefix = prefix,
                Suffix = suffix
            });

            while (temp.Count > 0)
            {
                var current = temp.Dequeue();

                if (string.IsNullOrEmpty(current.Suffix))
                {
                    Console.WriteLine(current.Prefix);
                }

                for (var idx = 0; idx < current.Suffix.Length; ++idx)
                {
                    temp.Enqueue(new ValueNode
                    {
                        Prefix = current.Prefix + current.Suffix[idx],
                        Suffix = current.Suffix.Remove(idx, 1)
                    });
                }
            }
        }

        private struct ValueNode
        {
            public string Prefix { get; set; }
            public string Suffix { get; set; }
        }
    }
}