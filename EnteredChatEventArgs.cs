using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp
{
    /// <summary>
    /// Specifies the contract for event handlers wishing to listen to the entered chat event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void EnteredChatEventHandler(object sender, EnteredChatEventArgs e);

    /// <summary>
    /// Specifies the event arguments for when the client entered chat.
    /// </summary>
    [DataContract]
    public class EnteredChatEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "UniqueUsername")]
        private string m_uun;
        [DataMember(Name = "Statstring")]
        private string m_ss;
        [DataMember(Name = "AccountName")]
        private string m_an;
        #endregion

        /// <summary>
        /// Creates a new instance of <see>EnteredChatEventArgs</see>.
        /// </summary>
        /// <param name="uniqueName">The unique display name of the client.</param>
        /// <param name="statstring">The client's stat string.</param>
        /// <param name="acctName">The client's account name.</param>
        public EnteredChatEventArgs(string uniqueName, string statstring, string acctName)
        {
            m_uun = uniqueName;
            m_ss = statstring;
            m_an = acctName;
        }

        /// <summary>
        /// Gets the unique username assigned to the client.
        /// </summary>
        public string UniqueUsername { get { return m_uun; } }

        /// <summary>
        /// Gets the user's client information string.
        /// </summary>
        public string Statstring { get { return m_ss; } }

        /// <summary>
        /// Gets the user's login account name.
        /// </summary>
        public string AccountName { get { return m_an; } }
    }
}
