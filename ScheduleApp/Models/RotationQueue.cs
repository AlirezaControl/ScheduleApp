using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardScheduler.Models
{
    public class RotationQueue<T>
    {
        private readonly LinkedList<T> _items = new LinkedList<T>();

        public RotationQueue() { }
        public RotationQueue(IEnumerable<T> items)
        {
            foreach (var it in items) _items.AddLast(it);
        }

        public T Peek() => _items.First.Value;

        public T DequeueAndRotate()
        {
            if (_items.Count == 0) throw new InvalidOperationException("Queue is empty");
            var first = _items.First.Value;
            _items.RemoveFirst();
            _items.AddLast(first);
            return first;
        }

        public void Add(T item) => _items.AddLast(item);

        public void RotateOnce() => DequeueAndRotate();

        public IEnumerable<T> Snapshot() => _items.ToList();
    }
}