using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Contains information about a user's statistics and profile for Warcraft 3 clients.
    /// </summary>
    [DataContract]
    public class WarcraftProfileEventArgs : BaseEventArgs
    {
        [DataMember(Name = "Username")]
        private string m_userName;
        [DataMember(Name = "Product")]
        private Product m_product;
        [DataMember(Name = "Profile")]
        private WarcraftProfile m_stats;
        [DataMember(Name = "Clan")]
        private ClanProfile m_clanStats;

        /// <summary>
        /// Creates a new instance of <see>WarcraftProfileEventArgs</see>.
        /// </summary>
        /// <param name="userName">The name of the user profile.</param>
        /// <param name="product">The product for which the profile represents.</param>
        protected internal WarcraftProfileEventArgs(string userName, Product product)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (object.ReferenceEquals(null, product))
                throw new ArgumentNullException("product");

            m_userName = userName;
            m_product = product;
        }

        /// <summary>
        /// Gets the name of the user whose profile was requested.
        /// </summary>
        public string Username
        {
            get { return m_userName; }
        }

        /// <summary>
        /// Gets the product for which the profile was requested.
        /// </summary>
        public Product Product
        {
            get { return m_product; }
        }

        /// <summary>
        /// Gets (and in derived classes, sets) the <see>WarcraftProfile</see> for the given user.
        /// </summary>
        public WarcraftProfile Profile
        {
            get { return m_stats; }
            internal protected set { m_stats = value; }
        }

        /// <summary>
        /// Gets (and in derived classes, sets) the <see>ClanProfile</see> for the given user.
        /// </summary>
        /// <remarks>
        /// <para>This member may be <see langword="null" /> if the user is not in a clan.  To detect whether the user is in a clan, 
        /// check the <see cref="Profile">Profile property's</see> <see cref="WarcraftProfile.ClanTag">ClanTag</see> property.  If that 
        /// value is <see langword="null" />, this property will be <see langword="null" />.</para>
        /// </remarks>
        public ClanProfile Clan
        {
            get { return m_clanStats; }
            internal protected set { m_clanStats = value; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for Warcraft 3 Profile events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void WarcraftProfileEventHandler(object sender, WarcraftProfileEventArgs e);
}
