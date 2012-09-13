using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using BNSharp.MBNCSUtil;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// When implemented in a derived class, provides statistical information about a user.
    /// </summary>
    /// <remarks>
    /// <para>More specific information is available from most users by casting an object of this class to one of the 
    /// derived classes.  Products and their relevant classes are provided in the following table:</para>
    /// <list type="table">
    ///     <listheader>
    ///         <item>
    ///             <term>Product</term>
    ///             <description>Stats subclass type</description>
    ///         </item>
    ///     </listheader>
    ///     <item>
    ///         <term>Starcraft Retail, Starcraft: Brood War, Starcraft Shareware, Japan Starcraft,
    ///         Warcraft II: Battle.net Edition, Diablo Retail, Diablo Shareware</term>
    ///         <description><see>StarcraftStats</see></description>
    ///     </item>
    ///     <item>
    ///         <term>Diablo II Retail, Diablo II: Lord of Destruction</term>
    ///         <description><see>Diablo2Stats</see></description>
    ///     </item>
    ///     <item>
    ///         <term>Warcraft III: The Reign of Chaos, Warcraft III: The Frozen Throne</term>
    ///         <description><see>Warcraft3Stats</see></description>
    ///     </item>
    ///     <item>
    ///         <term>Others (unknown clients)</term>
    ///         <description><see>DefaultStats</see></description>
    ///     </item>
    /// </list>
    /// </remarks>
    [DataContract]
    [KnownType(typeof(Diablo2Stats))]
    [KnownType(typeof(Warcraft3Stats))]
    [KnownType(typeof(StarcraftStats))]
    [KnownType(typeof(DefaultStats))]
    public abstract class UserStats
    {
        /// <summary>
        /// Parses a user statstring and returns an object representing the stats in a meaningful way.
        /// </summary>
        /// <param name="userName">The name of the user whose stats are being examined.</param>
        /// <param name="statsData">The stats of the user.</param>
        /// <returns>An instance of a class derived from <see>UserStats</see> based on the user's 
        /// <see cref="BNSharp.BattleNet.Product">Product</see>.  To check the product, check the 
        /// <see cref="UserStats.Product">Product property</see>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="userName"/> or <paramref name="statsData"/>
        /// are <see langword="null" />.</exception>
        /// <remarks>
        /// <para>More specific information is available from most users by casting an object of this class to one of the 
        /// derived classes.  Products and their relevant classes are provided in the following table:</para>
        /// <list type="table">
        ///     <listheader>
        ///         <item>
        ///             <term>Product</term>
        ///             <description>Stats subclass type</description>
        ///         </item>
        ///     </listheader>
        ///     <item>
        ///         <term>Starcraft Retail, Starcraft: Brood War, Starcraft Shareware, Japan Starcraft,
        ///         Warcraft II: Battle.net Edition, Diablo Retail, Diablo Shareware</term>
        ///         <description><see>StarcraftStats</see></description>
        ///     </item>
        ///     <item>
        ///         <term>Diablo II Retail, Diablo II: Lord of Destruction</term>
        ///         <description><see>Diablo2Stats</see></description>
        ///     </item>
        ///     <item>
        ///         <term>Warcraft III: The Reign of Chaos, Warcraft III: The Frozen Throne</term>
        ///         <description><see>Warcraft3Stats</see></description>
        ///     </item>
        ///     <item>
        ///         <term>Others (unknown clients)</term>
        ///         <description><see>DefaultStats</see></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static UserStats Parse(string userName, byte[] statsData)
        {
            DataReader dr = new DataReader(statsData);
            string productCode = dr.ReadDwordString(0);
            UserStats result = null;
            switch (productCode)
            {
                case "STAR":
                case "SEXP":
                case "SSHR":
                case "JSTR":
                case "W2BN":
                case "DSHR":
                case "DRTL":
                    result = new StarcraftStats(statsData);
                    break;
                case "D2DV":
                case "D2XP":
                    result = new Diablo2Stats(userName, statsData);
                    break;
                case "WAR3":
                case "W3XP":
                    result = new Warcraft3Stats(statsData);
                    break;
                default:
                    result = new DefaultStats(productCode, statsData);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates a default statistics object with information only about the product.
        /// </summary>
        /// <param name="product">The product for which to create information.</param>
        /// <returns>An instance of <see>UserStats</see> with only product information.</returns>
        public static UserStats CreateDefault(Product product)
        {
            return new DefaultStats(product.ProductCode, Encoding.ASCII.GetBytes(product.ProductCode));
        }

        /// <summary>
        /// When implemented in a derived class, gets the product with which the user is logged on.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given 
        /// the name <c>Product</c>.</para>
        /// </remarks>
        public abstract Product Product
        {
            get;
        }

        /// <summary>
        /// When implemented in a derived class, gets the literal text of the stat string.
        /// </summary>
        public abstract string LiteralText
        {
            get;
        }
    }
}
