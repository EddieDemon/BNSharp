using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Friends
{
    /// <summary>
    /// Specifies the level of information available about a Battle.net user who is on the client's friend list.
    /// </summary>
    [DataContract]
    public enum FriendLocation
    {
        /// <summary>
        /// Specifies that the user is offline.
        /// </summary>
        [EnumMember]
        Offline = 0,
        /// <summary>
        /// Specifies that the user is not in a chat channel.
        /// </summary>
        [EnumMember]
        NotInChat = 1,
        /// <summary>
        /// Specifies that the user is in a chat channel.
        /// </summary>
        [EnumMember]
        InChat = 2,
        /// <summary>
        /// Specifies that the user is in a public game.
        /// </summary>
        [EnumMember]
        InPublicGame = 3,
        /// <summary>
        /// Specifies that the user is in a private (password-protected) game, but you are not mutual friends, and so the user's game information
        /// will not be made available to you.
        /// </summary>
        [EnumMember]
        InPrivateGame = 4,
        /// <summary>
        /// Specifies that the user is in a private (password-protected) game, but because you are mutual friends, you will be provided
        /// the name of the game.
        /// </summary>
        [EnumMember]
        InPrivateGameMutualFriends = 5,
    }
}
