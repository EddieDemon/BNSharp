using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.BattleNet.Stats;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Represents a user found in a channel.
    /// </summary>
    [DataContract]
    public class ChatUser
    {
        [DataMember(Name = "Ping")]
        private int m_ping;
        [DataMember(Name = "Flags")]
        private UserFlags m_flags;
        [DataMember(Name = "Username")]
        private string m_un;
        [DataMember(Name = "Stats")]
        private UserStats m_stats;

        /// <summary>
        /// Creates a new <see>ChatUser</see>.
        /// </summary>
        /// <param name="userName">Specifies the user's fully-qualified username, including possibly the character name, 
        /// name separator (for Diablo 2 characters), and realm namespace qualifier.</param>
        /// <param name="ping">The user's latency.</param>
        /// <param name="flags">The user's flags.</param>
        /// <param name="stats">The user's stats.</param>
        /// <remarks>
        /// <para>The user's stats can be determined by passing the username and binary statsring value to 
        /// <see cref="UserStats.Parse">UserStats.Parse</see>.</para>
        /// </remarks>
        public ChatUser(string userName, int ping, UserFlags flags, UserStats stats)
        {
            m_un = userName;
            m_ping = ping;
            m_flags = flags;
            m_stats = stats;
        }

        /// <summary>
        /// Gets, and in derived classes sets, the user's latency to Battle.net.
        /// </summary>
        public int Ping
        {
            get { return m_ping; }
            protected internal set
            {
                m_ping = value;
            }
        }

        /// <summary>
        /// Gets, and in derived classes sets, user-specific flags.
        /// </summary>
        public UserFlags Flags
        {
            get { return m_flags; }
            protected internal set
            {
                m_flags = value;
            } 
        }

        /// <summary>
        /// Gets the user's full display name.
        /// </summary>
        public string Username
        {
            get { return m_un; }
        }

        /// <summary>
        /// Gets the user's stats.
        /// </summary>
        /// <remarks>
        /// <para>For more information about the user's stats, you should check the <see cref="UserStats.Product">Product</see>
        /// property of the object and then cast to one of the descendant classes.  For more information, see 
        /// <see>UserStats</see>.</para>
        /// </remarks>
        /// <seealso cref="UserStats"/>
        /// <seealso cref="Warcraft3Stats"/>
        /// <seealso cref="Diablo2Stats"/>
        /// <seealso cref="StarcraftStats"/>
        /// <seealso cref="DefaultStats"/>
        public UserStats Stats
        {
            get { return m_stats; }
        }
    }
}
