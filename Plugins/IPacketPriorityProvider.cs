using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.BattleNet;

namespace BNSharp.Plugins
{
    /// <summary>
    /// When implemented, allows a class to provide custom packet parsing priorities for BN# via the 
    /// <see cref="BattleNetClient.RegisterCustomPacketPriorities">RegisterCustomPacketPriorities</see> method.
    /// </summary>
    /// <remarks>
    /// <para>BN# uses a layered approach to custom packet priorities.  By default, all packet priorities 
    /// are Normal.  Registering a new packet priority provider will allow the new provider to allow High or Low 
    /// priorities to be defined without needing to define priorities for all packets.</para>
    /// <para>Once a lookup is performed for a given packet ID, the result is cached and reused.  Consequently, changing
    /// the priority value returned across multiple calls will only be reflected if the registered priority providers
    /// change across calls, at which point the cache is invalidated.</para>
    /// <para>The priority providers are checked in the most-recently-registered order; so consequently, if three 
    /// priority providers are registered in the order A, B, then C, then C will be checked first.</para>
    /// </remarks>
    public interface IPacketPriorityProvider
    {
        /// <summary>
        /// Determines whether the packet provider specifies a priority for the given packet ID.
        /// </summary>
        /// <param name="packetToSearch">The ID of the packet to look up.</param>
        /// <returns><see langword="true" /> if the packet ID is supported by this priority provider; otherwise 
        /// <see langword="false" />.</returns>
        bool Defines(BncsPacketId packetToSearch);
        /// <summary>
        /// Gets a priority for the specified packet ID.
        /// </summary>
        /// <param name="packetToSearch">The packet ID to look up.</param>
        /// <returns>A <see>Priority</see> for the specified packet ID that determines its parse order.</returns>
        Priority GetPriority(BncsPacketId packetToSearch);
    }
}
