using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Contains information about when a Warcraft 3 profile lookup failed.
    /// </summary>
    [DataContract]
    public class ProfileLookupFailedEventArgs : BaseEventArgs
    {
        [DataMember(Name = "Username")]
        private string m_userName;
        [DataMember(Name = "Product")]
        private Product m_product;

        /// <summary>
        /// Creates a new <see>ProfileLookupFailedEventArgs</see>.
        /// </summary>
        /// <param name="userName">The name of the user whose profile was looked.</param>
        /// <param name="product">The product for the request.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="userName"/> is <see langword="null" /> or empty, or if <paramref name="Product"/>
        /// is <see langword="null" />.</exception>
        public ProfileLookupFailedEventArgs(string userName, Product product)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (object.ReferenceEquals(null, product))
                throw new ArgumentNullException("product");

            m_userName = userName;
            m_product = product;
        }

        /// <summary>
        /// Gets the username of the requested profile.
        /// </summary>
        public string Username
        {
            get { return m_userName; }
        }

        /// <summary>
        /// Gets the product with which the request failed.
        /// </summary>
        public Product Product
        {
            get { return m_product; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for profile lookup failure events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ProfileLookupFailedEventHandler(object sender, ProfileLookupFailedEventArgs e);
}
