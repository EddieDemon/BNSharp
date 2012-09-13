using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp.Plugins
{
    /// <summary>
    /// Implements an outgoing command queue, which allows the client to delay, filter, or reorder messages that are waiting to be sent to prevent 
    /// flooding.  The default implementation does not delay or reorder messages.
    /// </summary>
    public interface ICommandQueue
    {
        /// <summary>
        /// Queues a message to be sent.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <remarks>
        /// <para>This method should not need to be called from user code; it is called automatically when <see cref="BNSharp.BattleNet.BattleNetClient.Send(string, Priority)">BattleNetClient.Send</see>
        /// is called.</para>
        /// <para>When implementing this interface, the <paramref name="priority"/> parameter may be ignored depending on implementation.</para>
        /// <para>In addition to enqueuing a message, it is possible to implement this method to filter specific types of messages from being sent.  For 
        /// instance, a simple filter may prevent a password from being sent over the wire.</para>
        /// </remarks>
        void EnqueueMessage(string message, Priority priority);

        /// <summary>
        /// Clears any messages enqueued for sending.
        /// </summary>
        /// <remarks>
        /// <para>This method is called when the <see>BattleNetClient</see> is disconnected.</para>
        /// </remarks>
        void Clear();

        /// <summary>
        /// Informs listeners that a message is ready to be sent over the wire.
        /// </summary>
        event QueuedMessageReadyCallback MessageReady;
    }

    /// <summary>
    /// Informs the <see>BattleNetClient</see> that a queued message is ready to be sent to the server.
    /// </summary>
    /// <param name="message">The message to be sent to the server.</param>
    public delegate void QueuedMessageReadyCallback(string message);
}
