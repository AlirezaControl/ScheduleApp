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

        /// <summary>
        /// Removes the first item and rotates it to the end.
        /// </summary>
        public T DequeueAndRotate()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            var first = _items.First.Value;
            _items.RemoveFirst();
            _items.AddLast(first);
            return first;
        }

        /// <summary>
        /// Standard queue-like enqueue.
        /// </summary>
        public void Enqueue(T item) => _items.AddLast(item);

        /// <summary>
        /// Alias for Enqueue to match your earlier "Add".
        /// </summary>
        public void Add(T item) => Enqueue(item);

        /// <summary>
        /// Rotates without returning.
        /// </summary>
        public void RotateOnce()
        {
            if (_items.Count > 0)
                DequeueAndRotate();
        }

        /// <summary>
        /// Snapshot of current queue order.
        /// </summary>
        public IEnumerable<T> Snapshot() => _items.ToList();

        internal object Dequeue()
        {
            throw new NotImplementedException();
        }
    }
}