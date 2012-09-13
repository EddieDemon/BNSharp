using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Friends
{
    /// <summary>
    /// Specifies that a friend has been removed from the client's friends list.
    /// </summary>
    [DataContract]
    public class FriendRemovedEventArgs : BaseEventArgs
    {
        [DataMember(Name = "Friend")]
        private FriendUser m_removed;

        /// <summary>
        /// Creates a new <see>FriendRemovedEventArgs</see>.
        /// </summary>
        /// <param name="friend">The friend who was removed.</param>
        public FriendRemovedEventArgs(FriendUser friend)
        {
            m_removed = friend;
        }

        /// <summary>
        /// Gets a reference to the friend who was removed.
        /// </summary>
        /// <remarks>
        /// <para>When this property's backing store is serialized as part of a WCF data contract,
        /// it is given the name <c>Friend</c>.</para>
        /// </remarks>
        public FriendUser Friend
        {
            get { return m_removed; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for friend removed events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void FriendRemovedEventHandler(object sender, FriendRemovedEventArgs e);
}
