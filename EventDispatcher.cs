using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BNSharp
{
    internal sealed class EventDispatcher : IDisposable
    {
        private Thread m_low, m_normal;
        private EventWaitHandle m_normalBlocker, m_lowBlocker;
        private Queue<InvokeHelperBase> m_normalQueue, m_lowQueue;

        internal EventDispatcher()
        {
            m_normalQueue = new Queue<InvokeHelperBase>();
            m_lowQueue = new Queue<InvokeHelperBase>();

            m_normalBlocker = new EventWaitHandle(false, EventResetMode.AutoReset);
            m_lowBlocker = new EventWaitHandle(false, EventResetMode.AutoReset);

            m_normal = new Thread(__NormalPriorityThread)
            {
                Priority = ThreadPriority.BelowNormal,
                IsBackground = true
            };
            m_normal.Start();

            m_low = new Thread(__LowPriorityThread)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
            m_low.Start();
        }

        public void EnqueueEvent(InvokeHelperBase invokee)
        {
            m_normalQueue.Enqueue(invokee);
            m_normalBlocker.Set();
            m_lowQueue.Enqueue(invokee);
        }

        private void __NormalPriorityThread()
        {
            try
            {
                while (true)
                {
                    m_normalBlocker.Reset();

                    while (m_normalQueue.Count == 0)
                    {
                        m_lowBlocker.Set();
                        m_normalBlocker.WaitOne();
                    }

                    if (m_normalQueue.Count > 0)
                    {
                        InvokeHelperBase helper = m_normalQueue.Dequeue();
                        if (helper != null)
                            helper.Invoke(Priority.Normal);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // exit gracefully
            }
        }

        private void __LowPriorityThread()
        {
            try
            {
                while (true)
                {
                    m_lowBlocker.Reset();

                    while (m_lowQueue.Count == 0 || m_normalQueue.Count > 0)
                        m_lowBlocker.WaitOne();

                    if (m_lowQueue.Count > 0)
                    {
                        InvokeHelperBase helper = m_lowQueue.Dequeue();
                        if (helper != null)
                        {
                            helper.Invoke(Priority.Low);
                            helper.FreeArguments();
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // exit gracefully
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EventDispatcher()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_normal != null)
                {
                    m_normal.Abort();
                    m_normal = null;
                }

                if (m_low != null)
                {
                    m_low.Abort();
                    m_low = null;
                }

                if (m_normalBlocker != null)
                {
                    m_normalBlocker.Close();
                    m_normalBlocker = null;
                }

                if (m_lowBlocker != null)
                {
                    m_lowBlocker.Close();
                    m_lowBlocker = null;
                }

                if (m_normalQueue != null)
                {
                    m_normalQueue.Clear();
                    m_normalQueue = null;
                }

                if (m_lowQueue != null)
                {
                    m_lowQueue.Clear();
                    m_lowQueue = null;
                }
            }
        }

        #endregion
    }
}
