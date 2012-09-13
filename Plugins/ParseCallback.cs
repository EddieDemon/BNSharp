using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.Net;
using BNSharp.BattleNet;

namespace BNSharp.Plugins
{
    /// <summary>
    /// Designates a callback method that can be used to parse a specific packet.  This delegate is not CLS-compliant.
    /// </summary>
    /// <param name="packetData">The contents of the packet.</param>
    /// <remarks>
    /// <para>This delegate should only be used by advanced developers when registering custom packet handlers with BN#.</para>
    /// <para>When overriding default behavior, the implementer should be careful to ensure to free the parse data.  This can be done by, when 
    /// bubbling the event, by setting the <see>BaseEventArgs.EventData</see> property.  Otherwise, call 
    /// <see>BufferPool.FreeBuffer</see> on the <see>BattleNetClientResources.IncomingBufferPool</see>.</para>
    /// </remarks>
    [CLSCompliant(false)]
    public delegate void ParseCallback(BattleNetClient.ParseData packetData);
}
