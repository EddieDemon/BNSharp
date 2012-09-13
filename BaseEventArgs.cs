using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using BNSharp.Net;
using BNSharp.BattleNet;

namespace BNSharp
{
    /// <summary>
    /// Provides the base class from which all BN# event argument class should derive.
    /// </summary>
    [Serializable]
#if !NET_2_ONLY
    [DataContract]
    [KnownType("GetKnownTypes")]
#endif
    public class BaseEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see>BaseEventArgs</see>.
        /// </summary>
        protected BaseEventArgs() { }

        [NonSerialized]
        private BattleNetClient.ParseData m_parse;
        /// <summary>
        /// Gets or sets the underlying connection data that was used to drive this event.  This property is not CLS-compliant.
        /// </summary>
        [CLSCompliant(false)]
        public BattleNetClient.ParseData EventData
        {
            get { return m_parse; }
            set { m_parse = value; }
        }

        /// <summary>
        /// Gets a new empty BaseEventArgs object for a specified event data object.  This method is not CLS-compliant.
        /// </summary>
        /// <param name="eventData">The client parsing data.</param>
        /// <returns>An empty instance with the specified client parsing data.</returns>
        [CLSCompliant(false)]
        public static BaseEventArgs GetEmpty(BattleNetClient.ParseData eventData)
        {
            BaseEventArgs e = new BaseEventArgs();
            e.EventData = eventData;
            return e;
        }

#if !NET_2_ONLY
        /// <summary>
        /// This method is provided as infrastructure code for WCF services.  This allows inheritence
        /// to function correctly on all types derived from BaseEventArgs that are known to the server.
        /// For more information, see the MSDN Library article "Data Contract Known Types" at 
        /// http://msdn.microsoft.com/en-us/library/ms730167.aspx
        /// </summary>
        private static Type[] GetKnownTypes()
        {
            List<Type> types = new List<Type>();
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type t in asm.GetTypes())
            {
                DataContractAttribute[] atts = t.GetCustomAttributes(typeof(DataContractAttribute), true) as DataContractAttribute[];
                if (atts.Length > 0)
                    types.Add(t);
            }

            return types.ToArray();
        }
#endif
    }
}
