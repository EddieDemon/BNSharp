using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.Plugins;
using BNSharp.Net;

namespace BNSharp.BattleNet
{
    partial class BattleNetClient
    {
        private EventSink m_customEventSink;
        private CombinedPacketPriorityProvider m_priorityProvider;

        /// <summary>
        /// Registers a custom handler for a specific packet ID.  This method is not CLS-compliant.
        /// </summary>
        /// <param name="packetID">The packet to register to handle.</param>
        /// <param name="parser">A callback that will handle the data.</param>
        /// <param name="previousParser">A previous parser, or <see langword="null" /> if no parser was set to handle such a packet.</param>
        /// <returns>A sink to fire off <see>BattleNetClient</see> events.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="parser"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="packetID"/> refers to the <c>Warden</c> value, or if the 
        /// value is greater than 255 or less than 0.</exception>
        /// <remarks>
        /// <para>This method can be used to replace the default BN# packet handlers or to augment them with new handlers.  For example, BN# does 
        /// not and will not likely ever support Diablo II Realms and Realm Character login.  However, third-party developers can augment BN# to 
        /// handle those packets and support events on their own.</para>
        /// <para>The <see>IBattleNetEvents</see> sink is the same for subsequent calls to this method per-instance of <see>BattleNetClient</see>,
        /// so you don't need to do anything special with the return value each time you call this method.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public virtual IBattleNetEvents RegisterCustomPacketHandler(BncsPacketId packetID, ParseCallback parser, out ParseCallback previousParser)
        {
            if (object.ReferenceEquals(null, parser))
                throw new ArgumentNullException("parser");

            if (packetID == BncsPacketId.Warden)
                throw new ArgumentOutOfRangeException("packetID", packetID, "Cannot register a custom packet handler for the Warden packet.  Use the WardenHandler property instead.");

            if (packetID > (BncsPacketId)255 || packetID < BncsPacketId.Null)
                throw new ArgumentOutOfRangeException("packetID", packetID, "Cannot register a custom packet handler for a packet ID greater than 255 or less than 0.");

            if (m_packetToParserMap.ContainsKey(packetID))
            {
                previousParser = m_packetToParserMap[packetID];
            }
            else
            {
                previousParser = null;
            }
            m_packetToParserMap[packetID] = parser;

            return m_customEventSink;
        }

        /// <summary>
        /// Unregisters a custom handler for a specific packet ID, restoring the previous handler.  This method is not CLS-compliant.
        /// </summary>
        /// <param name="packetID">The packet ID for which to unregister.</param>
        /// <param name="previousParser">The previous parser.  If none was provided during registration, this value may be <see langword="null" />.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="packetID"/> refers to the <c>Warden</c> value, or if the 
        /// value is greater than 255 or less than 0.</exception>
        [CLSCompliant(false)]
        public virtual void UnregisterCustomPacketHandler(BncsPacketId packetID, ParseCallback previousParser)
        {
            if (packetID == BncsPacketId.Warden)
                throw new ArgumentOutOfRangeException("packetID", packetID, "Cannot register a custom packet handler for the Warden packet.  Use the WardenHandler property instead.");

            if (packetID > (BncsPacketId)255 || packetID < BncsPacketId.Null)
                throw new ArgumentOutOfRangeException("packetID", packetID, "Cannot register a custom packet handler for a packet ID greater than 255 or less than 0.");

            if (object.ReferenceEquals(previousParser, null))
            {
                m_packetToParserMap.Remove(packetID);
            }
            else
            {
                m_packetToParserMap[packetID] = previousParser;
            }
        }

        /// <summary>
        /// Gets or sets the Warden module currently in use.
        /// </summary>
        /// <remarks>
        /// <para>The warden implementation must use this property to set itself before beginning the connection </para>
        /// </remarks>
        public virtual IWardenModule WardenHandler
        {
            get { return m_warden; }
            set { m_warden = value; }
        }

        /// <summary>
        /// Registers a custom packet priority list for consideration during packet parsing.
        /// </summary>
        /// <param name="newProvider">The new priority provider to use for priority lookups.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newProvider"/> is <see langword="null" />.</exception>
        public void RegisterCustomPacketPriorities(IPacketPriorityProvider newProvider)
        {
            m_priorityProvider.RegisterNewProvider(newProvider);
        }

        /// <summary>
        /// Unregisters a custom packet priority list from consideration during packet parsing.
        /// </summary>
        /// <param name="providerToRemove">The priority provider to remove from consideration.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="providerToRemove"/> is 
        /// <see langword="null" />.</exception>
        public void UnregisterCustomPacketPriorities(IPacketPriorityProvider providerToRemove)
        {
            m_priorityProvider.UnregisterProvider(providerToRemove);
        }
    }
}
