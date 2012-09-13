using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Friends
{
    /// <summary>
    /// Represents a Battle.net friend user.
    /// </summary>
    [DataContract]
    public class FriendUser
    {
        #region fields
        [DataMember(Name = "Index")]
        private int m_index;
        [DataMember(Name = "AccountName")]
        private string m_acctName;
        [DataMember(Name = "Status")]
        private FriendStatus m_status;
        [DataMember(Name = "LocationType")]
        private FriendLocation m_location;
        [DataMember(Name = "Product")]
        private Product m_product;
        [DataMember(Name = "Location")]
        private string m_locationName;
        #endregion

        /// <summary>
        /// Creates a new <see>FriendUser</see>.
        /// </summary>
        /// <param name="index">The 0-based index of the user's location.</param>
        /// <param name="accountName">The account name of the friend.</param>
        /// <param name="status">The friend's current status.</param>
        /// <param name="locationType">The friend's current location information.</param>
        /// <param name="product">The product with which the friend is currently logged on, otherwise <see langword="null" />.</param>
        /// <param name="location">The name of the friend's current location.</param>
        public FriendUser(int index, string accountName, FriendStatus status, FriendLocation locationType, Product product, string location)
        {
            m_index = index;
            m_acctName = accountName;
            m_status = status;
            m_location = locationType;
            m_product = product;
            m_locationName = location;
        }

        /// <summary>
        /// Gets, and in derived classes sets, the index (0-based) of the user on the client's friends list.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given 
        /// the name <c>Index</c>.</para>
        /// </remarks>
        public int Index
        {
            get { return m_index; }
            protected internal set { m_index = value; } 
        }

        /// <summary>
        /// Gets, and in derived classes sets, the account name of the friend.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given 
        /// the name <c>AccountName</c>.</para>
        /// </remarks>
        public string AccountName
        {
            get { return m_acctName; }
            protected internal set { m_acctName = value; }
        }

        /// <summary>
        /// Gets, and in derived classes sets, a reference to the product information about the user's current logged on state.
        /// </summary>
        /// <remarks>
        /// <para>This property will return <see langword="null" /> if the user is currently offline.</para>
        /// <para>When exposed under a WCF data contract, this property's backing store is given 
        /// the name <c>Product</c>.</para>
        /// </remarks>
        public Product Product
        {
            get { return m_product; }
            protected internal set { m_product = value; }
        }

        /// <summary>
        /// Gets, and in derived classes sets, contextual information about the user's status.
        /// </summary>
        /// <remarks>
        /// <para>This property will return <see langword="null" /> if the user is currently offline.</para>
        /// <para>When exposed under a WCF data contract, this property's backing store is given 
        /// the name <c>Product</c>.</para>
        /// </remarks>
        public FriendStatus Status
        {
            get { return m_status; }
            protected internal set { m_status = value; }
        }

        /// <summary>
        /// Gets, and in derived classes sets, the type of location information provided by Battle.net.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given 
        /// the name <c>LocationType</c>.</para>
        /// </remarks>
        public FriendLocation LocationType
        {
            get { return m_location; }
            protected internal set { m_location = value; }
        }

        /// <summary>
        /// Gets, and in derived classes sets, the name of the location of the current user.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given 
        /// the name <c>Location</c>.</para>
        /// </remarks>
        public string Location
        {
            get { return m_locationName; }
            protected internal set { m_locationName = value; }
        }
    }
}
