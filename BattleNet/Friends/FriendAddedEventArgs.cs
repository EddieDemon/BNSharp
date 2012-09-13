using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Friends
{
    /// <summary>
    /// Specifies that a new friend was added to the list of friends.
    /// </summary>
    [DataContract]
    public class FriendAddedEventArgs : BaseEventArgs
    {
        [DataMember(Name = "NewFriend")]
        private FriendUser m_newFriend;

        /// <summary>
        /// Creates a new <see>FriendAddedEventArgs</see>.
        /// </summary>
        /// <param name="newFriend">The friend that was added to the list.</param>
        public FriendAddedEventArgs(FriendUser newFriend)
        {
            m_newFriend = newFriend;
        }

        /// <summary>
        /// Gets a reference to the friend that was added.
        /// </summary>
        /// <remarks>
        /// <para>When this property's backing store is serialized as part of a WCF data contract,
        /// it is given the name <c>NewFriend</c>.</para>
        /// </remarks>
        public FriendUser NewFriend
        {
            get { return m_newFriend; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for friend added events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void FriendAddedEventHandler(object sender, FriendAddedEventArgs e);
}
