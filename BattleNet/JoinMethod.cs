using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Specifies functionality differences when instructing the client to join a channel.
    /// </summary>
    public enum JoinMethod
    {
        /// <summary>
        /// Specifies the default channel join.
        /// </summary>
        Default,
        /// <summary>
        /// Joins the channel only if it is not empty.
        /// </summary>
        NoCreate,
        /// <summary>
        /// Joins the client's product-specific channel and is sent only upon logging in.
        /// </summary>
        FirstJoin,
        /// <summary>
        /// Sent when leaving a game or joining an empty public channel from the channels list.
        /// </summary>
        Forced,
        /// <summary>
        /// Equivalent to <see>FirstJoin</see>, but specific to Diablo 2 clients.
        /// </summary>
        FirstJoinDiablo2,
    }
}
