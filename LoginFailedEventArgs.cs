using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp
{
    /// <summary>
    /// Contains information about a situation in which the client failed to log into Battle.net.
    /// </summary>
    [DataContract]
    public class LoginFailedEventArgs : BaseEventArgs
    {
        [DataMember(Name = "StatusCode")]
        private int m_statusCode;
        [DataMember(Name = "Reason")]
        private LoginFailureReason m_reason;
        [DataMember(Name = "Description")]
        private string m_description;
        [DataMember(Name = "ProvidesExtendedInformation")]
        private bool m_supportsExtendedInformation;

        /// <summary>
        /// Creates a new <see>LoginFailedEventArgs</see> that does not support extended information.
        /// </summary>
        /// <param name="reason">The login failure reason associated with this event.</param>
        /// <param name="statusCode">The underlying message status code, which may be useful if the <paramref name="reason"/> parameter is 
        /// <see cref="LoginFailureReason">Unknown</see>.</param>
        public LoginFailedEventArgs(LoginFailureReason reason, int statusCode)
        {
            m_reason = reason;
            m_statusCode = statusCode;
        }

        /// <summary>
        /// Creates a new <see>LoginFailedEventArgs</see> that does supports extended information.
        /// </summary>
        /// <param name="reason">The login failure reason associated with this event.</param>
        /// <param name="statusCode">The underlying message status code, which may be useful if the <paramref name="reason"/> parameter is 
        /// <see cref="LoginFailureReason">Unknown</see>.</param>
        /// <param name="description">Additional textual information optionally provided by the Battle.net server.</param>
        public LoginFailedEventArgs(LoginFailureReason reason, int statusCode, string description)
            : this(reason, statusCode)
        {
            m_description = description;
            m_supportsExtendedInformation = true;
        }

        /// <summary>
        /// Gets whether information besides that an invalid username or password was provided.
        /// </summary>
        public bool ProvidesExtendedInformation
        {
            get { return m_supportsExtendedInformation; }
        }

        /// <summary>
        /// Gets a textual reason for the login failure, if one was provided by the server.
        /// </summary>
        /// <remarks>
        /// <para>This property is only meaningful if <see>ProvidesExtendedInformation</see> is <see langword="true" />.</para>
        /// </remarks>
        public string Description
        {
            get { return m_description; }
        }

        /// <summary>
        /// Gets the literal status code returned from the server.
        /// </summary>
        public int StatusCode
        {
            get { return m_statusCode; }
        }

        /// <summary>
        /// Gets the basic login failure reason.
        /// </summary>
        public LoginFailureReason Reason
        {
            get { return m_reason; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for login failure events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void LoginFailedEventHandler(object sender, LoginFailedEventArgs e);
}
