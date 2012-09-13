using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.Net;
using BNSharp.BattleNet;
using BNSharp.Security;

namespace BNSharp
{
    /// <summary>
    /// Provides global resources preallocated for performance.
    /// </summary>
    /// <remarks>
    /// <para>When custom packet handlers are being used, if the next handler is null, the handler should free the packet (return it to the pool) 
    /// by calling <c>BattleNetClientResources.IncomingBufferPool.FreeBuffer()</c> on the related packet.</para>
    /// </remarks>
    public static class BattleNetClientResources
    {
        private static List<BattleNetClient> s_activeClients = new List<BattleNetClient>();

        private static BufferPool s_incoming = new BufferPool("Incoming Packets", 1024, 5);
        private static BufferPool s_outgoing = new BufferPool("Outgoing Packets", 768, 5) { ClearOnFree = true };

        private const int INCOMING_BUFFERS_PER_CLIENT = 25;
        private const int OUTGOING_BUFFERS_PER_CLIENT = 10;

        private static EventDispatcher s_sharedDispatcher = new EventDispatcher();

        /// <summary>
        /// Gets the <see>BufferPool</see> used for incoming packets.
        /// </summary>
        public static BufferPool IncomingBufferPool
        {
            get { return s_incoming; }
        }

        /// <summary>
        /// Gets the <see>BufferPool</see> used for outgoing packets.
        /// </summary>
        public static BufferPool OutgoingBufferPool
        {
            get { return s_outgoing; }
        }

        /// <summary>
        /// Registers a client connection, tracking it and increasing the available buffer pool.
        /// </summary>
        /// <param name="client">The client connection that is being registered.</param>
        public static void RegisterClient(BattleNetClient client)
        {
            s_activeClients.Add(client);
            s_incoming.IncreaseBufferCount(INCOMING_BUFFERS_PER_CLIENT);
            s_outgoing.IncreaseBufferCount(OUTGOING_BUFFERS_PER_CLIENT);
        }

        /// <summary>
        /// Unregisters a client connection, halting tracking and decreasing the available buffer pool.
        /// </summary>
        /// <param name="client">The client connection being unregistered.</param>
        public static void UnregisterClient(BattleNetClient client)
        {
            s_activeClients.Remove(client);
            s_incoming.DecreaseBufferCount(INCOMING_BUFFERS_PER_CLIENT);
            s_outgoing.DecreaseBufferCount(OUTGOING_BUFFERS_PER_CLIENT);
        }

        /// <summary>
        /// Acquires the shared event dispatcher used to dispatch normal- and low-priority events for
        /// client implementations.
        /// </summary>
        /// <returns>The shared <see>EventDispatcher</see>.</returns>
        internal static EventDispatcher AcquireDispatcher()
        {
            ImmediateCallerIsOfTypePermission<BattleNetClient>.Demand();
            return s_sharedDispatcher;
        }
    }
}
