// Author: Alexander Karagulamos
// Date:
// Comment:

using Euler.Common.Extensions;
using System;

namespace Euler.Algorithms.Permutations.Permuters
{
    internal class LexicographicalPermuter : IPermuter
    {
        public void Execute(string pattern)
        {
            var elements = pattern.ToCharArray();

            // Uncomment the line below so that the list starts out initially sorted
            // Array.Sort(elements);

            Console.WriteLine(elements);

            var nFactorial = Factorial(pattern.Length);

            for (long idx = 1; idx < nFactorial; ++idx)
            {
                var permutation = GetNextLexicographicalOrderingOf(elements);
                Console.WriteLine(permutation);
            }
        }
        
        private static char[] GetNextLexicographicalOrderingOf(char[] elements)
        {
            const int negative = -1;

            var posX = negative;

            for (var x = 0; x < elements.Length - 1; ++x)
            {
                if (elements[x] < elements[x + 1])
                    posX = x;
            }

            var posY = negative;

            for (var y = posX != negative ? posX : elements.Length; y < elements.Length; ++y)
            {
                if (elements[posX] < elements[y])
                    posY = y;
            }

            if (posY != negative)
            {
                elements.Swap(posX, posY);
            }

            elements.Reverse(posX + 1);

            return elements;
        }

        private static long Factorial(long n)
        {
            long result = 1;
            for (long x = 1; x <= n; ++x)
                result *= x;

            return result;
        }
    }
}