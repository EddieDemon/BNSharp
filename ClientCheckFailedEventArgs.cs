using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp
{
    /// <summary>
    /// Specifies the contract for event handlers that want to handle the client versioning check failure event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClientCheckFailedEventHandler(object sender, ClientCheckFailedEventArgs e);

    /// <summary>
    /// Specifies the arguments for a client versioning check failure event.
    /// </summary>
    [DataContract]
    public class ClientCheckFailedEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "Reason")]
        private ClientCheckFailureCause m_reason;
        [DataMember(Name = "AdditionalInformation")]
        private string m_info; 
        #endregion

        /// <summary>
        /// Creates a new instance of <see>ClientCheckFailedEventArgs</see>.
        /// </summary>
        /// <param name="reason">The failure code for version checking.</param>
        /// <param name="additionalInformation">Additional information, if available.</param>
        public ClientCheckFailedEventArgs(ClientCheckFailureCause reason, string additionalInformation)
        {
            m_reason = reason;
            m_info = additionalInformation;
        }

        /// <summary>
        /// Gets the reason provided by Battle.net.
        /// </summary>
        public ClientCheckFailureCause Reason
        {
            get
            {
                return m_reason;
            }
        }

        /// <summary>
        /// Gets additional information, if any, provided by Battle.net about the problem.
        /// </summary>
        public string AdditionalInformation
        {
            get
            {
                return m_info;
            }
        }
    }
}
