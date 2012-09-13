using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BNSharp.Plugins
{
    [DebuggerDisplay("(pass-through command queue)")]
    internal sealed class DefaultCommandQueue : ICommandQueue
    {
        #region ICommandQueue Members

        public void EnqueueMessage(string message, Priority priority)
        {
            OnMessageReady(message);
        }

        public void Clear()
        {
            
        }

        private void OnMessageReady(string message)
        {
            if (MessageReady != null)
                MessageReady(message);
        }

        public event QueuedMessageReadyCallback MessageReady;

        #endregion
    }
}
