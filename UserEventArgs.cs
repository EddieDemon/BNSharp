using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using BNSharp.BattleNet;
using System.Diagnostics;

namespace BNSharp
{
    /// <summary>
    /// Specifies the contract for chat events that involve another user, but not specifically communication.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void UserEventHandler(object sender, UserEventArgs e);

    /// <summary>
    /// Specifies event information for chat events that involve another user, but not specifically communication.
    /// </summary>
    /// <para>An example of when this class would be used is for a user joined or user left event.</para>
    [DataContract]
    public class UserEventArgs : ChatEventArgs
    {
        #region fields
        [DataMember(Name = "User")]
        private ChatUser m_user;
        #endregion

        /// <summary>
        /// Creates a new <see>UserEventArgs</see> with the specified settings.
        /// </summary>
        /// <param name="eventType">The type of chat event.</param>
        /// <param name="user">A reference to the user involved in the event.</param>
        public UserEventArgs(ChatEventType eventType, ChatUser user)
            : base(eventType)
        {
            Debug.Assert(user != null);

            m_user = user;
        }

        /// <summary>
        /// Gets a reference to the user who was involved in the event.
        /// </summary>
        public ChatUser User
        {
            get { return m_user; }
        }
    }
}
