// Author: Alexander Karagulamos
// Date:
// Comment:

using System;
using System.Collections.Generic;
using System.Text;

namespace Euler.Algorithms.Permutations.Permuters
{
    internal class RecursiveBooleanBacktrackingPermuter : IPermuter
    {
     
        public void Execute(string pattern)
        {
            Permute(pattern, new bool[pattern.Length], new StringBuilder());
        }

        private static void Permute(string pattern, IList<bool> used, StringBuilder result)
        {
            if (result.Length == pattern.Length)
            {
                Console.WriteLine(result.ToString());
                return;
            }

            for (var idx = 0; idx < pattern.Length; ++idx)
            {
                if (used[idx]) continue;

                result.Append(pattern[idx]);
                used[idx] = true;

                Permute(pattern, used, result);

                used[idx] = false;
                result.Length = result.Length - 1;
            }
        }
    }
}