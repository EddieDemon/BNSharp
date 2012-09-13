using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp
{
    /// <summary>
    /// Specifies the flags that can be applied to channel-related <see cref="ServerChatEventArgs.Flags">chat events</see>.
    /// </summary>
    [Flags]
    [DataContract]
    public enum ChannelFlags
    {
        /// <summary>
        /// Specifies that the channel is a normal private channel.
        /// </summary>
        [EnumMember]
        None = 0,
        /// <summary>
        /// Specifies that the channel is public.
        /// </summary>
        [EnumMember]
        PublicChannel = 1,
        /// <summary>
        /// Specifies that the channel is moderated by a Blizzard representative.
        /// </summary>
        [EnumMember]
        ModeratedChannel = 2,
        /// <summary>
        /// Specifies that the channel is restricted.
        /// </summary>
        [EnumMember]
        RestrictedChannel = 4,
        /// <summary>
        /// Specifies that the channel is silent.
        /// </summary>
        [EnumMember]
        SilentChannel = 8,
        /// <summary>
        /// Specifies that the channel is provided by the system.
        /// </summary>
        [EnumMember]
        SystemChannel = 0x10,
        /// <summary>
        /// Specifies that the channel is specific to a product.
        /// </summary>
        [EnumMember]
        ProductSpecificChannel = 0x20,
        /// <summary>
        /// Specifies that the channel is globally-accessible.
        /// </summary>
        [EnumMember]
        GloballyAccessibleChannel = 0x1000,
    }
}
