using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using BNSharp.BattleNet;

namespace BNSharp
{
    /// <summary>
    /// Specifies the flags that can be related to user-specific <see cref="ChatUser.Flags">chat events</see>.
    /// </summary>
    [Flags]
    [DataContract]
    public enum UserFlags
    {
        /// <summary>
        /// Indicates that the user is normal.
        /// </summary>
        [EnumMember]
        None = 0,
        /// <summary>
        /// Indicates that the user is a Blizzard representative.
        /// </summary>
        [EnumMember]
        BlizzardRepresentative = 1,
        /// <summary>
        /// Indicates that the user is a channel operator.
        /// </summary>
        [EnumMember]
        ChannelOperator = 2,
        /// <summary>
        /// Indicates that the user has a Speaker icon.
        /// </summary>
        [EnumMember]
        Speaker = 4,
        /// <summary>
        /// Indicates that the user is a Battle.net Administrator.
        /// </summary>
        [EnumMember]
        BattleNetAdministrator = 8,
        /// <summary>
        /// Indicates that the user's client expects UDP support but lacks it.
        /// </summary>
        [EnumMember]
        NoUDP = 0x10,
        /// <summary>
        /// Indicates that the user is currently squelched by the client.
        /// </summary>
        [EnumMember]
        Squelched = 0x20,
        /// <summary>
        /// Indicates a channel special guest.
        /// </summary>
        [EnumMember]
        SpecialGuest = 0x40,
        /// <summary>
        /// Represented when the client had "beep" enabled, a client-side setting.  No longer supported on Battle.net.
        /// </summary>
        [Obsolete("No longer seen on Battle.net.  For more information see http://www.bnetdocs.org/?op=doc&did=15.", false)]
        [EnumMember]
        BeepEnabled = 0x100,
        /// <summary>
        /// Represented PGL players.  No longer seen on Battle.net.
        /// </summary>
        [Obsolete("No longer seen on Battle.net.  For more information see http://www.bnetdocs.org/?op=doc&did=15.", false)]
        [EnumMember]
        PglPlayer = 0x200,
        /// <summary>
        /// Represented PGL officials.  No longer seen on Battle.net.
        /// </summary>
        [Obsolete("No longer seen on Battle.net.  For more information see http://www.bnetdocs.org/?op=doc&did=15.", false)]
        [EnumMember]
        PglOfficial = 0x400,
        /// <summary>
        /// Represented KBK players.  No longer seen on Battle.net.
        /// </summary>
        [Obsolete("No longer seen on Battle.net.  For more information see http://www.bnetdocs.org/?op=doc&did=15.", false)]
        [EnumMember]
        KbkPlayer = 0x800,
        /// <summary>
        /// The flag for WCG officials.
        /// </summary>
        [EnumMember]
        WcgOfficial = 0x1000,
        /// <summary>
        /// Represented KBK singles players.  No longer seen on Battle.net.
        /// </summary>
        [Obsolete("No longer seen on Battle.net.  For more information see http://www.bnetdocs.org/?op=doc&did=15.", false)]
        [EnumMember]
        KbkSingles = 0x2000,
        /// <summary>
        /// Represented beginner KBK players.  No longer seen on Battle.net.
        /// </summary>
        [Obsolete("No longer seen on Battle.net.  For more information see http://www.bnetdocs.org/?op=doc&did=15.", false)]
        [EnumMember]
        KbkBeginner = 0x10000,
        /// <summary>
        /// Represented a single bar for KBK players.  No longer seen on Battle.net.
        /// </summary>
        [Obsolete("No longer seen on Battle.net.  For more information see http://www.bnetdocs.org/?op=doc&did=15.", false)]
        [EnumMember]
        WhiteKbk = 0x20000,
        /// <summary>
        /// The flag for GF officials.
        /// </summary>
        [EnumMember]
        GFOfficial = 0x100000,
        /// <summary>
        /// The flag for GF players.
        /// </summary>
        [EnumMember]
        GFPlayer = 0x200000,
        /// <summary>
        /// The current flag for PGL players.
        /// </summary>
        [EnumMember]
        PglPlayer2 = 0x2000000,
    }
}
