// Author: Alexander Karagulamos
// Date:
// Comment:

using Euler.Common.Extensions;
using System;

namespace Euler.Algorithms.Permutations.Permuters
{
    internal class RecursiveInplaceSwapPermuter : IPermuter
    {
        public void Execute(string pattern)
        {
            var elements = pattern.ToCharArray();
            Permute(elements, 0);
        }

        private static void Permute(char[] elements, int k)
        {
            if (k == elements.Length)
            {
                Console.WriteLine(new string(elements));
                return;
            }

            for (var idx = k; idx < elements.Length; ++idx)
            {
                elements.Swap(k, idx);

                Permute(elements, k + 1);

                if (elements[idx] == elements[k])
                    continue;

                elements.Swap(k, idx);
            }

        }
    }
}