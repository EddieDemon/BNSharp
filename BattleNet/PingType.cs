using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Specifies the type of client ping that should be used.
    /// </summary>
    public enum PingType
    {
        /// <summary>
        /// Specifies that the connection should have normal ping.
        /// </summary>
        Normal,
        /// <summary>
        /// Specifies that the connection should attempt to have a -1ms ping.
        /// </summary>
        MinusOneMs,
        /// <summary>
        /// Specifies that the connection should attempt to have a 0ms ping.
        /// </summary>
        ZeroMs,
        /// <summary>
        /// Specifies that the client should reply to the ping packet before beginning the version check.
        /// </summary>
        ReplyBeforeVersioning,
    }
}
