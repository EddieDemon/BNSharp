using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace BNSharp
{
    /// <summary>
    /// Implements a basic priority queue.
    /// </summary>
    /// <typeparam name="T">The type of item to queue.</typeparam>
    public class PriorityQueue<T>
    {
        #region priority comparer class
        private class PriorityComparer : IComparer<Priority>
        {
            #region IComparer<Priority> Members
            public int Compare(Priority x, Priority y)
            {
                return y.CompareTo(x);
            }
            #endregion
        }
        #endregion

        private SortedList<Priority, Queue<T>> m_list;

        /// <summary>
        /// Creates a new <see>PriorityQueue</see>.
        /// </summary>
        public PriorityQueue()
        {
            m_list = new SortedList<Priority, Queue<T>>(new PriorityComparer());
            m_list.Add(Priority.High, new Queue<T>());
            m_list.Add(Priority.Normal, new Queue<T>());
            m_list.Add(Priority.Low, new Queue<T>());
        }

        /// <summary>
        /// Enqueues an item at the specified priority.
        /// </summary>
        /// <param name="priority">The <see>Priority</see> at which to enqueue the item.</param>
        /// <param name="item">The item to enqueue.</param>
        /// <exception cref="InvalidEnumArgumentException">Thrown if <paramref name="priority"/> is not a valid enumeration value.</exception>
        public void Enqueue(Priority priority, T item)
        {
            if (!Enum.IsDefined(typeof(Priority), priority))
                throw new InvalidEnumArgumentException("priority", (int)priority, typeof(Priority));

            lock (m_list)
            {
                m_list[priority].Enqueue(item);
            }
        }

        /// <summary>
        /// Gets an item from the queue.
        /// </summary>
        /// <returns>The next highest-priority item in the queue.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the queue is empty.</exception>
        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("This queue is empty.");
            }

            T item = default(T);
            lock (m_list)
            {
                foreach (Queue<T> queue in m_list.Values)
                {
                    if (queue.Count > 0)
                    {
                        item = queue.Dequeue();
                        break;
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Clears all queued items.
        /// </summary>
        /// <remarks>
        /// <para>This method is thread-safe; however, if another thread is actively dequeuing an item and already has taken a lock, that operation will
        /// complete first.</para>
        /// </remarks>
        public void Clear()
        {
            lock (m_list)
            {
                m_list[Priority.Low].Clear();
                m_list[Priority.Normal].Clear();
                m_list[Priority.High].Clear();
            }
        }

        /// <summary>
        /// Gets the current number of items in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                foreach (Queue<T> queue in m_list.Values)
                {
                    count += queue.Count;
                }
                return count;
            }
        }
    }
}
