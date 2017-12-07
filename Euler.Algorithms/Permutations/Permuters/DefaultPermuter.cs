// Author: Alexander Karagulamos
// Date:
// Comment:

using System;

namespace Euler.Algorithms.Permutations.Permuters
{
    internal class DefaultPermuter : IPermuter
    {
        public void Execute(string pattern)
        {
            Permute(string.Empty, pattern);
        }

        public void Permute(string prefix, string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
            {
                Console.WriteLine(prefix);
                return;
            }

            for (var idx = 0; idx < suffix.Length; ++idx)
            {
                Permute(prefix + suffix[idx], suffix.Remove(idx, 1));
            }
        }
    }
}