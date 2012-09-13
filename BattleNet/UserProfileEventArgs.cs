using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Specifies a user profile that has been populated by Battle.net.
    /// </summary>
    //[DataContract]
    public class UserProfileEventArgs : BaseEventArgs
    {
        [DataMember(Name = "Profile")]
        private UserProfileRequest m_profile;

        /// <summary>
        /// Creates a new <see>UserProfileEventArgs</see> with the specified profile.
        /// </summary>
        /// <param name="filledProfile">The profile that had been populated.</param>
        public UserProfileEventArgs(UserProfileRequest filledProfile)
        {
            Debug.Assert(!object.ReferenceEquals(filledProfile, null));

            m_profile = filledProfile;
        }

        /// <summary>
        /// Gets the user profile result from the request.
        /// </summary>
        public UserProfileRequest Profile
        {
            get { return m_profile; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for user profile events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void UserProfileEventHandler(object sender, UserProfileEventArgs e);
}
