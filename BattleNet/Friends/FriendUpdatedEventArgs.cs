using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Friends
{
    /// <summary>
    /// Specifies that a friend's status has been updated.
    /// </summary>
    [DataContract]
    public class FriendUpdatedEventArgs : BaseEventArgs
    {
        [DataMember(Name = "Friend")]
        private FriendUser m_updated;

        /// <summary>
        /// Creates a new <see>FriendUpdatedEventArgs</see>.
        /// </summary>
        /// <param name="updatedFriend">The friend that was updated.</param>
        public FriendUpdatedEventArgs(FriendUser updatedFriend)
        {
            m_updated = updatedFriend;
        }

        /// <summary>
        /// Gets a reference to the friend that was updated.
        /// </summary>
        /// <remarks>
        /// <para>When this property's backing store is serialized as part of a WCF data contract,
        /// it is given the name <c>Friend</c>.</para>
        /// </remarks>
        public FriendUser Friend
        {
            get { return m_updated; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for friend updated events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void FriendUpdatedEventHandler(object sender, FriendUpdatedEventArgs e);
}
