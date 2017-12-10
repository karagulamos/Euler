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

        public void Push(TKey itemToInsert)
        {
            _heap.Add(itemToInsert);

            var childIndex = _heap.Count - 1;
            var parentIndex = (childIndex - 1) >> 1;

            while (childIndex > 0 && !_heapCondition.IsValid(_heap[parentIndex], itemToInsert))
            {
                _heap[childIndex] = _heap[parentIndex];
                childIndex = parentIndex;
                parentIndex = (parentIndex - 1) >> 1;
            }

            _heap[childIndex] = itemToInsert;
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

            var parentIndex = 0;
            _heap[parentIndex] = _heap[_heap.Count - 1];

            TKey itemToReposition = _heap[parentIndex];

            var midpoint = _heap.Count >> 1;

            while (parentIndex < midpoint)
            {
                int leftChildIndex = (parentIndex << 1) + 1, rightChildIndex = leftChildIndex + 1;

                var selectedChildIndex = 
                    rightChildIndex < _heap.Count && !_heapCondition.IsValid(_heap[leftChildIndex], _heap[rightChildIndex]) 
                    ? rightChildIndex : leftChildIndex;

                if (_heapCondition.IsValid(itemToReposition, _heap[selectedChildIndex]))
                    break;

                _heap[parentIndex] = _heap[selectedChildIndex];
                parentIndex = selectedChildIndex;
            }

            _heap[parentIndex] = itemToReposition;

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
                return first.CompareTo(second) >= 0;
            }
        }

        public struct MinHeap : IHeapCondition<TKey>
        {
            public bool IsValid(TKey first, TKey second)
            {
                return first.CompareTo(second) < 0;
            }
        }
    }
}
