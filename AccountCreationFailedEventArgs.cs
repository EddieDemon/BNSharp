using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp
{
    /// <summary>
    /// Contains information about an account creation attempt that failed.
    /// </summary>
    [DataContract]
    public class AccountCreationFailedEventArgs : AccountCreationEventArgs
    {
        [DataMember(Name = "Reason")]
        private CreationFailureReason m_reason;

        /// <summary>
        /// Creates a new <see>AccountCreationFailedEventArgs</see> for the specifiec account.
        /// </summary>
        /// <param name="accountName">The name that failed to be created.</param>
        /// <param name="reason">The reason provided by Battle.net for the failure.</param>
        public AccountCreationFailedEventArgs(string accountName, CreationFailureReason reason)
            : base(accountName)
        {
            m_reason = reason;
        }

        /// <summary>
        /// Gets the reason for the account creation failure.
        /// </summary>
        public CreationFailureReason Reason
        {
            get { return m_reason; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for account creation failure events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void AccountCreationFailedEventHandler(object sender, AccountCreationFailedEventArgs e);
}
