using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BNSharp
{
    /// <summary>
    /// Specifies the contract for clients wishing to register for the channel list event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ChannelListEventHandler(object sender, ChannelListEventArgs e);

    /// <summary>
    /// Specifies the channel list event arguments.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{m_list.Length} channel(s).")]
    public class ChannelListEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "Channels")]
        private string[] m_list;
        #endregion
        /// <summary>
        /// Creates a new instance of <see>ChannelListEventArgs</see>.
        /// </summary>
        /// <param name="channels">The channels to list.</param>
        public ChannelListEventArgs(string[] channels)
        {
            m_list = channels;
        }

        /// <summary>
        /// Gets the copy of the list of channels sent by the server.
        /// </summary>
        public ReadOnlyCollection<string> Channels
        {
            get
            {
                return new ReadOnlyCollection<string>(m_list);
            }
        }
    }
}
