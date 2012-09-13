using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Contains event data about when the client's user account leaves a clan.
    /// </summary>
    [DataContract]
    public class LeftClanEventArgs : BaseEventArgs
    {
        [DataMember(Name = "RemovedByLeader")]
        private bool m_removed;

        /// <summary>
        /// Creates a new <see>LeftClanEventArgs</see>.
        /// </summary>
        /// <param name="removed">Specifies whether the client was removed from the clan.</param>
        /// <seealso cref="RemovedByLeader"/>
        public LeftClanEventArgs(bool removed)
        {
            m_removed = removed;
        } 

        /// <summary>
        /// Gets whether the client was removed from the clan by a leader.
        /// </summary>
        /// <remarks>
        /// <para>If this property returns <see langword="false" />, it means that the user left of his or her own accord.</para>
        /// </remarks>
        public bool RemovedByLeader
        {
            get { return m_removed; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for client clan departure events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void LeftClanEventHandler(object sender, LeftClanEventArgs e);
}
