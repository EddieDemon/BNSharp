using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp.Plugins
{
    /// <summary>
    /// When implemented, allows a custom class to handle Warden packets.
    /// </summary>
    public interface IWardenModule
    {
        /// <summary>
        /// Initializes the Warden module with the specified CD key hash part, the native socket handle, and the game file.
        /// </summary>
        /// <param name="keyHashPart">The key hash part provided.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />.</returns>
        bool InitWarden(int keyHashPart);
        /// <summary>
        /// Uninitializes the Warden module.
        /// </summary>
        void UninitWarden();
        /// <summary>
        /// Processes the warden challenge.
        /// </summary>
        /// <param name="wardenPacket">A copy of the original buffer sent by the server.</param>
        void ProcessWarden(byte[] wardenPacket);
    }
}
