using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Friends
{
    /// <summary>
    /// Specifies that a friend has moved position in the client's list of friends (for example, by being promoted or demoted).
    /// </summary>
    [DataContract]
    public class FriendMovedEventArgs : BaseEventArgs
    {
        [DataMember(Name = "Friend")]
        private FriendUser m_user;
        [DataMember(Name = "NewIndex")]
        private int m_newIndex;

        /// <summary>
        /// Creates a new <see>FriendMovedEventArgs</see>.
        /// </summary>
        /// <param name="friend">The friend whose position changed.</param>
        /// <param name="newIndex">The new 0-based index of the friend's position.</param>
        public FriendMovedEventArgs(FriendUser friend, int newIndex)
        {
            m_user = friend;
            m_newIndex = newIndex;
        }

        /// <summary>
        /// Gets a reference to the friend whose position changed.
        /// </summary>
        /// <remarks>
        /// <para>When this property's backing store is serialized as part of a WCF data contract,
        /// it is given the name <c>Friend</c>.</para>
        /// </remarks>
        public FriendUser Friend
        {
            get { return m_user; }
        }

        /// <summary>
        /// Gets the new position of the friend (0-based).
        /// </summary>
        /// <remarks>
        /// <para>When this property's backing store is serialized as part of a WCF data contract,
        /// it is given the name <c>NewIndex</c>.</para>
        /// </remarks>
        public int NewIndex
        {
            get { return m_newIndex; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for friend moved events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void FriendMovedEventHandler(object sender, FriendMovedEventArgs e);
}
