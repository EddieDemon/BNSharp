using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Gets information about the user's product when the product is otherwise unrecognized.
    /// </summary>
    [DataContract]
    public class DefaultStats : UserStats
    {
        #region fields
        [DataMember(Name = "Product")]
        private Product m_prod;
        [DataMember(Name = "LiteralText")]
        private string m_lit;
        #endregion

        internal DefaultStats(string productID, byte[] literal)
        {
            Debug.Assert(literal != null);

            m_prod = Product.GetByProductCode(productID);
            if (m_prod == null)
                m_prod = Product.UnknownProduct;

            m_lit = Encoding.ASCII.GetString(literal);
        }

        /// <summary>
        /// Gets the <see cref="BNSharp.BattleNet.Product">Product</see> with which the user is currently logged on.
        /// </summary>
        public override Product Product
        {
            get
            {
                return m_prod;
            } 
        }

        /// <summary>
        /// Gets the literal statstring text.
        /// </summary>
        public override string LiteralText
        {
            get { return m_lit; }
        }
    }
}
