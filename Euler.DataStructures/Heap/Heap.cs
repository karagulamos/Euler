using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Euler.DataStructures.Heap
{
    public class Heap<TKey> : IEnumerable<TKey>
    where TKey : IComparable
    {
        private readonly IHeapCondition<TKey> _heapCondition;
        private readonly List<TKey> _heap;

        public Heap() : this(0) {}

        public Heap(int capacity) : this(capacity, new MaxHeap()) {}

        public Heap(IHeapCondition<TKey> heapCondition) : this(0, heapCondition) {}

        public Heap(int capacity, IHeapCondition<TKey> heapCondition)
        {
            _heapCondition = heapCondition;
            _heap = new List<TKey>(capacity);
        }

        public void Push(List<TKey> elements)
        {
            foreach (var element in elements)
                Push(element);
        }

        public void Push(TKey element)
        {
            _heap.Add(element);

            var index = _heap.Count - 1;
            var temp = _heap[index];

            var parent = (index - 1) >> 1;

            while (index > 0 && !_heapCondition.IsValid(_heap[parent], temp))
            {
                _heap[index] = _heap[parent];
                index = parent;
                parent = (parent - 1) >> 1;
            }

            _heap[index] = temp;
        }

        public void Pop()
        {
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException(
                    "You cannot remove from an empty heap. " +
                    "Consider checking the size of the heap before attempting this operation."
                );
            }

            var index = 0;
            _heap[index] = _heap[_heap.Count - 1];

            TKey temp = _heap[index];

            var midpoint = _heap.Count >> 1;

            while (index < midpoint)
            {
                int left = (index << 1) + 1, right = left + 1;

                var currentIndex = 
                    right < _heap.Count && !_heapCondition.IsValid(_heap[left], _heap[right]) 
                    ? right : left;

                if (_heapCondition.IsValid(temp, _heap[currentIndex]))
                    break;

                _heap[index] = _heap[currentIndex];
                index = currentIndex;
            }

            _heap[index] = temp;

            _heap.RemoveAt(_heap.Count - 1);
        }

        public TKey Peek() => _heap[0];

        public int Size => _heap.Count;

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var element in this)
            {
                builder.Append($"{element} ");
            }

            return builder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TKey> GetEnumerator() => _heap.GetEnumerator();

        public struct MaxHeap : IHeapCondition<TKey>
        {
            public bool IsValid(TKey first, TKey second)
            {
                return second.CompareTo(first) < 0;
            }
        }

        public struct MinHeap : IHeapCondition<TKey>
        {
            public bool IsValid(TKey first, TKey second)
            {
                return second.CompareTo(first) >= 0;
            }
        }
    }
}
