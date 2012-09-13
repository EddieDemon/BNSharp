using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp.BattleNet.Clans
{
    internal enum ClanResponseCode : byte
    {
        Success = 0,
        InUse = 1,
        TooSoon = 2,
        NotEnoughMembers = 3,
        InvitationDeclined = 4,
        Decline = 5,
        Accept = 6,
        NotAuthorized = 7,
        UserNotFound = 8,
        ClanFull = 9,
        BadTag = 10,
        BadName = 11,
        UserNotFoundInClan = 12,
    }
}
