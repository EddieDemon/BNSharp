using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Represents a Battle.net chat product.  This class cannot be instantiated.
    /// </summary>
    /// <remarks>
    /// <para>This class is primarily designed to provide information about products supported on Battle.net.  In order to obtain an instance of it,
    /// access one of the static fields.  Equality can also be tested by comparing a user's product to an instance retrieved from the fields exposed
    /// by this class.</para>
    /// </remarks>
    [DataContract]
    public sealed class Product : IEquatable<Product>, IEquatable<string>
    {
        private static Dictionary<string, Product> s_products;

        #region fields
        [DataMember(Name = "ProductCode")]
        private string m_prodCode;
        [DataMember(Name = "Name")]
        private string m_descriptiveTitle;

        private bool m_canConnect, m_needs2Keys, m_needsLockdown, m_usesUdp;
        #endregion

        private Product(string productCode, string descriptiveTitle)
        {
            m_prodCode = productCode;
            m_descriptiveTitle = descriptiveTitle;

            if (s_products == null)
                s_products = new Dictionary<string, Product>();

            s_products.Add(productCode, this);
        }

        private Product(string productCode, string descriptiveTitle, bool canConnect, bool needs2Keys)
            : this(productCode, descriptiveTitle)
        {
            m_canConnect = canConnect;
            m_needs2Keys = needs2Keys;
        }

        private Product(string productCode, string descriptiveTitle, bool canConnect, bool needs2Keys, bool needsLockdown)
            : this(productCode, descriptiveTitle, canConnect, needs2Keys)
        {
            m_needsLockdown = needsLockdown;
        }

        private Product(string productCode, string descriptiveTitle, bool canConnect, bool needs2Keys, bool needsLockdown, bool needsUdp)
            : this(productCode, descriptiveTitle, canConnect, needs2Keys, needsLockdown)
        {
            m_usesUdp = needsUdp;
        }

        /// <summary>
        /// The <see>Product</see> object for a telnet chat client.
        /// </summary>
        public static readonly Product ChatClient = new Product("CHAT", Strings.ProdCHAT);

        /// <summary>
        /// The <see>Product</see> object for Starcraft (Retail).
        /// </summary>
        public static readonly Product StarcraftRetail = new Product("STAR", Strings.ProdSTAR, true, false, true, true);
        /// <summary>
        /// The <see>Product</see> object for Starcraft Shareware.
        /// </summary>
        public static readonly Product StarcraftShareware = new Product("SSHR", Strings.ProdSSHR);
        /// <summary>
        /// The <see>Product</see> object for Starcraft: Brood War.
        /// </summary>
        public static readonly Product StarcraftBroodWar = new Product("SEXP", Strings.ProdSEXP, true, false, true, true);
        /// <summary>
        /// The <see>Product</see> object for Japan Starcraft.
        /// </summary>
        public static readonly Product JapanStarcraft = new Product("JSTR", Strings.ProdJSTR, true, false, false, true);

        /// <summary>
        /// The <see>Product</see> object for Warcraft II: Battle.net Edition.
        /// </summary>
        public static readonly Product Warcraft2BNE = new Product("W2BN", Strings.ProdW2BN, true, false, true, true);

        /// <summary>
        /// The <see>Product</see> object for Diablo (Retail).
        /// </summary>
        public static readonly Product DiabloRetail = new Product("DRTL", Strings.ProdDRTL);
        /// <summary>
        /// The <see>Product</see> object for Diablo (Shareware).
        /// </summary>
        public static readonly Product DiabloShareware = new Product("DSHR", Strings.ProdDSHR);

        /// <summary>
        /// The <see>Product</see> object for Diablo 2 Shareware.
        /// </summary>
        public static readonly Product Diablo2Shareware = new Product("D2SH", Strings.ProdD2SH);
        /// <summary>
        /// The <see>Product</see> object for Diablo 2 (Retail).
        /// </summary>
        public static readonly Product Diablo2Retail = new Product("D2DV", Strings.ProdD2DV, true, false);
        /// <summary>
        /// The <see>Product</see> object for Diablo 2: The Lord of Destruction.
        /// </summary>
        public static readonly Product Diablo2Expansion = new Product("D2XP", Strings.ProdD2XP, true, true);
        
        /// <summary>
        /// The <see>Product</see> object for Warcraft 3: The Reign of Chaos.
        /// </summary>
        public static readonly Product Warcraft3Retail = new Product("WAR3", Strings.ProdWAR3, true, false);

        /// <summary>
        /// The <see>Product</see> object for Warcraft 3: The Frozen Throne.
        /// </summary>
        public static readonly Product Warcraft3Expansion = new Product("W3XP", Strings.ProdW3XP, true, true);

        /// <summary>
        /// The <see>Product</see> object that represents any product unrecognized by BN#.
        /// </summary>
        public static readonly Product UnknownProduct = new Product("UNKN", Strings.ProdUNKN);

        /// <summary>
        /// Gets the <see>Product</see> associated with the specified product code.
        /// </summary>
        /// <param name="productCode">The four-character product code to check.</param>
        /// <returns>A <see>Product</see> object associated with the product code if it is found; otherwise <see langword="null" />.</returns>
        public static Product GetByProductCode(string productCode)
        {
            if (s_products.ContainsKey(productCode))
                return s_products[productCode];
            return null;
        }

        /// <summary>
        /// Gets the product code for this Product.
        /// </summary>
        public string ProductCode
        {
            get { return m_prodCode; }
        }

        /// <summary>
        /// Gets the name of this product.
        /// </summary>
        /// <remarks>
        /// <para>If localized resources exist for the current language, they are retrieved.</para>
        /// </remarks>
        public string Name
        {
            get { return m_descriptiveTitle; }
        }

        /// <summary>
        /// Gets an array of all supported products.
        /// </summary>
        /// <returns>An array of recognized products.</returns>
        public static ReadOnlyCollection<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>(s_products.Values);
            return new ReadOnlyCollection<Product>(products);
        }

        internal bool CanConnect
        {
            get { return m_canConnect; }
        }

        internal bool NeedsTwoKeys
        {
            get { return m_needs2Keys; }
        }

        internal bool NeedsLockdown
        {
            get { return m_needsLockdown; }
        }

        internal bool UsesUdpPing
        {
            get { return m_usesUdp; }
        }

        #region IEquatable<string> Members

        /// <summary>
        /// Determines whether the specified product's product code matches the specified product code.
        /// </summary>
        /// <param name="other">The product code to test.</param>
        /// <returns><see langword="true" /> if this product matches the tested product code; otherwise <see langword="false" />.</returns>
        public bool Equals(string other)
        {
            return m_prodCode.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region IEquatable<Product> Members
        /// <summary>
        /// Determines whether the specified product and this product represent the same Battle.net client.
        /// </summary>
        /// <param name="other">The client to test.</param>
        /// <returns><see langword="true" /> if the products match; otherwise <see langword="false" />.</returns>
        public bool Equals(Product other)
        {
            return m_prodCode.Equals(other.m_prodCode, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
