using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace BNSharp
{
    /// <summary>
    /// Contains information about an attempted account creation event.
    /// </summary>
    [DataContract]
    public class AccountCreationEventArgs : BaseEventArgs
    {
        [DataMember(Name = "AccountName")]
        private string m_acctName;

        /// <summary>
        /// Creates a new <see>AccountCreationEventArgs</see> for the specified account name.
        /// </summary>
        /// <param name="accountName">The name of the account being created.</param>
        public AccountCreationEventArgs(string accountName)
        {
            Debug.Assert(!string.IsNullOrEmpty(accountName));
            m_acctName = accountName;
        }

        /// <summary>
        /// Gets the name of the account being created.
        /// </summary>
        public string AccountName
        {
            get { return m_acctName; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for account creation events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void AccountCreationEventHandler(object sender, AccountCreationEventArgs e);
}
