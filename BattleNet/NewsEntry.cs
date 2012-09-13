using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Security.Permissions;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Represents a news entry.
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class NewsEntry
    {
        #region fields
        [DataMember(Name = "DatePosted")]
        private DateTime m_ts;
        [DataMember(Name = "News")]
        private string m_news; 
        #endregion

        /// <summary>
        /// Creates a new <see>NewsEntry</see> with the specified timestamp and content.
        /// </summary>
        /// <param name="timestamp">The time at which the news was posted in UTC.</param>
        /// <param name="news">The content of the news.</param>
        public NewsEntry(DateTime timestamp, string news)
        {
            m_ts = timestamp;
            m_news = news;
        }

        /// <summary>
        /// Gets the date at which this news was posted in local time.
        /// </summary>
        public DateTime DatePosted
        {
            get { return m_ts.ToLocalTime(); }
        }

        /// <summary>
        /// Gets the text of the news entry.
        /// </summary>
        public string News
        {
            get { return m_news; }
        }

        /// <summary>
        /// Gets a string representation of this news entry.
        /// </summary>
        /// <returns>A string of the date's long date string.</returns>
        public override string ToString()
        {
            return string.Concat(DatePosted.ToShortDateString(), " ", DatePosted.ToShortTimeString());
        }
    }
}
