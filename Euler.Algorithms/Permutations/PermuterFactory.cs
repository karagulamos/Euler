using System;
using Euler.Algorithms.Permutations.Permuters;

namespace Euler.Algorithms.Permutations
{
    public class PermuterFactory
    {
        public static IPermuter Get(PermutationType permutationType = PermutationType.Default)
        {
            switch (permutationType)
            {
                case PermutationType.Default:
                    return new DefaultPermuter();
                case PermutationType.Lexicographical:
                    return new LexicographicalPermuter();
                case PermutationType.RecursiveInplaceSwap:
                    return new RecursiveInplaceSwapPermuter();
                case PermutationType.RecursiveBooleanBacktracking:
                    return new RecursiveBooleanBacktrackingPermuter();
                case PermutationType.RecursiveBufferedBacktracking:
                    return new RecursiveBufferedBacktrackingPermuter();
              
                default:
                    throw new ArgumentOutOfRangeException(nameof(permutationType), permutationType, null);
            }
        }
    }
}