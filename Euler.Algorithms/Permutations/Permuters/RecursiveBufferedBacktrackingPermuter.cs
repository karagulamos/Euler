// Author: Alexander Karagulamos
// Date:
// Comment:

using System;
using System.Text;

namespace Euler.Algorithms.Permutations.Permuters
{
    internal class RecursiveBufferedBacktrackingPermuter : IPermuter
    {
        public void Execute(string pattern)
        {
            Permute(new StringBuilder(pattern), new StringBuilder());
        }

        public void Permute(StringBuilder elements, StringBuilder buffer)
        {
            if (elements.Length <= 0)
            {
                Console.WriteLine(buffer);
                return;
            }

            for (var idx = 0; idx < elements.Length; ++idx)
            {
                var c = elements[idx];

                buffer.Append(c);
                elements.Remove(idx, 1);

                Permute(elements, buffer);

                elements.Insert(idx, c.ToString());
                buffer.Length--;
            }
        }
    }
}