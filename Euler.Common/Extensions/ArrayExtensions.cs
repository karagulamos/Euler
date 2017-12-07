namespace Euler.Common.Extensions
{
    public static class ArrayExtensions
    {
        public static void Reverse<T>(this T[] elements)
        {
            elements.Reverse(0, elements.Length);
        }

        public static void Reverse<T>(this T[] elements, int start)
        {
            elements.Reverse(start, elements.Length);
        }

        public static void Reverse<T>(this T[] elements, int start, int length)
        {
            var middle = (length + start) >> 1;
            var last = length - 1;

            for (var current = start; current < middle; ++current, --last)
            {
                elements.Swap(current, last);
            }
        }

        public static void Swap<TElem>(this TElem[] elements, int first, int second)
        {
            var temp = elements[first];
            elements[first] = elements[second];
            elements[second] = temp;
        }
    }
}