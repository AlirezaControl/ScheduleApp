using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GuardScheduler.Models
{
    public class RotationQueue<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _items = new LinkedList<T>();

        public RotationQueue() { }

        public RotationQueue(IEnumerable<T> items)
        {
            foreach (var it in items)
                _items.AddLast(it);
        }

        public int Count => _items.Count;

        public T Peek()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Queue is empty");
            return _items.First.Value;
        }

        public T DequeueAndRotate()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            var first = _items.First.Value;
            _items.RemoveFirst();
            _items.AddLast(first);
            return first;
        }

        public void Enqueue(T item) => _items.AddLast(item);
        public void Add(T item) => Enqueue(item);
        public void RotateOnce() { if (_items.Count > 0) DequeueAndRotate(); }
        public bool RotateTo(T target)
        {
            if (_items.Count == 0) return false;
            int maxRotations = _items.Count;
            for (int i = 0; i < maxRotations; i++)
            {
                if (EqualityComparer<T>.Default.Equals(_items.First.Value, target))
                    return true;
                DequeueAndRotate();
            }
            return false;
        }

        public IEnumerable<T> Snapshot() => _items.ToList();
        public List<T> ToList() => _items.ToList();

        // --- Add this to enable foreach ---
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
