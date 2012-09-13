using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Specifies the contract for an event handler that wishes to handle the server news event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ServerNewsEventHandler(object sender, ServerNewsEventArgs e);

    /// <summary>
    /// Represents a single news entry.
    /// </summary>
    [DataContract]
    public class ServerNewsEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "Entry")]
        private NewsEntry m_entry; 
        #endregion

        /// <summary>
        /// Creates a new instance of <see>ServerNewsEventArgs</see>.
        /// </summary>
        /// <param name="entry">The entry for this news event.</param>
        public ServerNewsEventArgs(NewsEntry entry)
        {
            Debug.Assert(entry != null);

            m_entry = entry;
        }

        /// <summary>
        /// Gets the news entry that triggered the event..
        /// </summary>
        public NewsEntry Entry
        {
            get { return m_entry; }
        }
    }
}
