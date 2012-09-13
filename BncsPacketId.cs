using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp
{
    /// <summary>
    /// Specifies symbolic name IDs for Battle.net binary protocol messages.
    /// </summary>
    public enum BncsPacketId : int
    {
        /// <summary>
        /// Specifies SID_NULL (0x00)
        /// </summary>
        Null = 0,
        /// <summary>
        /// Specifies SID_STOPADV (0x02)
        /// </summary>
        StopAdv = 2,
        /// <summary>
        /// Specifies SID_STARTADVEX (0x08)
        /// </summary>
        StartAdvEx = 8,
        /// <summary>
        /// Specifies SID_GETADVLISTEX (0x09)
        /// </summary>
        GetAdvListEx = 9,
        /// <summary>
        /// Specifies SID_ENTERCHAT (0x0a)
        /// </summary>
        EnterChat = 0x0a,
        /// <summary>
        /// Specifies SID_GETCHANNELLIST (0x0b)
        /// </summary>
        GetChannelList = 0x0b,
        /// <summary>
        /// Specifies SID_JOINCHANNEL (0x0c)
        /// </summary>
        JoinChannel = 0x0c,
        /// <summary>
        /// Specifies SID_CHATCOMMAND (0x0e)
        /// </summary>
        ChatCommand = 0x0e,
        /// <summary>
        /// Specifies SID_CHATEVENT (0x0f)
        /// </summary>
        ChatEvent = 0x0f,
        /// <summary>
        /// Specifies SID_LEAVECHAT (0x10)
        /// </summary>
        LeaveChat = 0x10,
        /// <summary>
        /// Specifies SID_FLOODDETECTED (0x13)
        /// </summary>
        FloodDetected = 0x13,
        /// <summary>
        /// Specifies SID_UDPPINGRESPONSE (0x14)
        /// </summary>
        UdpPingResponse = 0x14,
        /// <summary>
        /// Specifies SID_CHECKAD (0x15)
        /// </summary>
        CheckAd = 0x15,
        /// <summary>
        /// Specifies SID_CLICKAD (0x16)
        /// </summary>
        ClickAd = 0x16,
        /// <summary>
        /// Specifies SID_MESSAGEBOX (0x19)
        /// </summary>
        MessageBox = 0x19,
        /// <summary>
        /// Specifies SID_STARTADVEX3 (0x1c)
        /// </summary>
        StartAdvEx3 = 0x1c,
        /// <summary>
        /// Specifies SID_LOGONCHALLENGEEX (0x1d)
        /// </summary>
        LoginChallengeEx = 0x1d,
        /// <summary>
        /// Specifies SID_LEAVEGAME (0x1f)
        /// </summary>
        LeaveGame = 0x1f,
        /// <summary>
        /// Specifies SID_DISPLAYAD (0x21)
        /// </summary>
        DisplayAd = 0x21,
        /// <summary>
        /// Specifies SID_NOTIFYJOIN (0x22)
        /// </summary>
        NotifyJoin = 0x22,
        /// <summary>
        /// Specifies SID_PING (0x25)
        /// </summary>
        Ping = 0x25,
        /// <summary>
        /// Specifies SID_READUSERDATA (0x26)
        /// </summary>
        ReadUserData = 0x26,
        /// <summary>
        /// Specifies SID_WRITEUSERDATA (0x27)
        /// </summary>
        WriteUserData = 0x27,
        /// <summary>
        /// Specifies SID_LOGONCHALLENGE (0x28)
        /// </summary>
        LogonChallenge = 0x28,
        /// <summary>
        /// Specifies SID_LOGONRESPONSE (0x29)
        /// </summary>
        LogonResponse = 0x29,
        /// <summary>
        /// Specifies SID_CREATEACCOUNT (0x2a)
        /// </summary>
        CreateAccount = 0x2a,
        /// <summary>
        /// Specifies SID_GAMERESULT (0x2c)
        /// </summary>
        GameResult = 0x2c,
        /// <summary>
        /// Specifies SID_GETICONDATA (0x2d)
        /// </summary>
        GetIconData = 0x2d,
        /// <summary>
        /// Specifies SID_GETLADDERDATA (0x2e)
        /// </summary>
        GetLadderData = 0x2e,
        /// <summary>
        /// Specifies SID_FINDLADDERUSER (0x2f)
        /// </summary>
        FindLadderUser = 0x2f,

        /// <summary>
        /// Specifies SID_CDKEY (0x30)
        /// </summary>
        CdKey = 0x30,
        /// <summary>
        /// Specifies SID_CHANGEPASSWORD (0x31)
        /// </summary>
        ChangePassword = 0x31,
        /// <summary>
        /// Specifies SID_QUERYREALMS (0x34)
        /// </summary>
        QueryRealms = 0x34,
        /// <summary>
        /// Specifies SID_PROFILE (0x35)
        /// </summary>
        Profile = 0x35,
        /// <summary>
        /// Specifies SID_CDKEY2 (0x36)
        /// </summary>
        CdKey2 = 0x36,
        /// <summary>
        /// Specifies SID_LOGONRESPONSE2 (0x3a)
        /// </summary>
        LogonResponse2 = 0x3a,
        /// <summary>
        /// Specifies SID_CREATEACCOUNT2 (0x3d)
        /// </summary>
        CreateAccount2 = 0x3d,
        /// <summary>
        /// Specifies SID_LOGONREALMEX (0x3e)
        /// </summary>
        LogonRealmEx = 0x3e,
        /// <summary>
        /// Specifies SID_QUERYREALMS2 (0x40)
        /// </summary>
        QueryRealms2 = 0x40,
        /// <summary>
        /// Specifies SID_QUERYADURL (0x41)
        /// </summary>
        QueryAdUrl = 0x41,
        /// <summary>
        /// Specifies SID_WARCRAFTGENERAL (0x44)
        /// </summary>
        WarcraftGeneral = 0x44,
        /// <summary>
        /// Specifies SID_NETGAMEPORT (0x45)
        /// </summary>
        NetGamePort = 0x45,
        /// <summary>
        /// Specifies SID_NEWSINFO (0x46)
        /// </summary>
        NewsInfo = 0x46,
        /// <summary>
        /// Specifies SID_OPTIONALWORK (0x4a)
        /// </summary>
        OptionalWork = 0x4a,
        /// <summary>
        /// Specifies SID_EXTRAWORK (0x4b)
        /// </summary>
        ExtraWork = 0x4b,
        /// <summary>
        /// Specifies SID_REQUIREDWORK (0x4c)
        /// </summary>
        RequiredWork = 0x4c,

        /// <summary>
        /// Specifies SID_AUTHINFO (0x50)
        /// </summary>
        AuthInfo = 0x50,
        /// <summary>
        /// Specifies SID_AUTHCHECK (0x51)
        /// </summary>
        AuthCheck = 0x51,
        /// <summary>
        /// Specifies SID_AUTHACCOUNTCREATE (0x52)
        /// </summary>
        AuthAccountCreate = 0x52,
        /// <summary>
        /// Specifies SID_AUTHACCOUNTLOGON (0x53)
        /// </summary>
        AuthAccountLogon = 0x53,
        /// <summary>
        /// Specifies SID_AUTHACCOUNTLOGONPROOF (0x54)
        /// </summary>
        AuthAccountLogonProof = 0x54,
        /// <summary>
        /// Specifies SID_AUTHACCOUNTCHANGE (0x55)
        /// </summary>
        AuthAccountChange = 0x55,
        /// <summary>
        /// Specifies SID_AUTHACCOUNTCHANGEPROOF (0x56)
        /// </summary>
        AuthAccountChangeProof = 0x56,
        /// <summary>
        /// Specifies SID_AUTHACCOUNTUPGRADE (0x57)
        /// </summary>
        AuthAccountUpgrade = 0x57,
        /// <summary>
        /// Specifies SID_AUTHACCOUNTUPGRADEPROOF (0x58)
        /// </summary>
        AuthAccountUpgradeProof = 0x58,

        /// <summary>
        /// Specifies SID_SETEMAIL (0x59)
        /// </summary>
        SetEmail = 0x59,
        /// <summary>
        /// Specifies SID_RESETPASSWORD (0x5a)
        /// </summary>
        ResetPassword = 0x5a,
        /// <summary>
        /// Specifies SID_CHANGEEMAIL (0x5b)
        /// </summary>
        ChangeEmail = 0x5b,
        /// <summary>
        /// Specifies SID_SWITCHPRODUCT (0x5c)
        /// </summary>
        SwitchProduct = 0x5c,

        /// <summary>
        /// Specifies SID_WARDEN (0x5e)
        /// </summary>
        Warden = 0x5e,

        /// <summary>
        /// Specifies SID_GAMEPLAYERSEARCH (0x60)
        /// </summary>
        GamePlayerSearch = 0x60,

        /// <summary>
        /// Specifies SID_FRIENDSLIST (0x65)
        /// </summary>
        FriendsList = 0x65,
        /// <summary>
        /// Specifies SID_FRIENDUPDATED (0x66)
        /// </summary>
        FriendsUpdate = 0x66,
        /// <summary>
        /// Specifies SID_FRIENDADDED (0x67)
        /// </summary>
        FriendsAdd = 0x67,
        /// <summary>
        /// Specifies SID_FRIENDREMOVED (0x68)
        /// </summary>
        FriendsRemove = 0x68,
        /// <summary>
        /// Specifies SID_FRIENDPOSITION (0x69).
        /// </summary>
        FriendsPosition = 0x69,

        /// <summary>
        /// Specifies SID_CLANFINDCANDIDATES (0x70)
        /// </summary>
        ClanFindCandidates = 0x70,
        /// <summary>
        /// Specifies SID_CLANINVITEMULTIPLE (0x71)
        /// </summary>
        ClanInviteMultiple = 0x71,
        /// <summary>
        /// Specifies SID_CLANCREATIONINVITATION (0x72)
        /// </summary>
        ClanCreationInvitation = 0x72,
        /// <summary>
        /// Specifies SID_CLANDISBAND (0x73)
        /// </summary>
        ClanDisband = 0x73,
        /// <summary>
        /// Specifies SID_CLANMAKECHEIFTAN (0x74)
        /// </summary>
        ClanMakeChieftan = 0x74,
        /// <summary>
        /// Specifies SID_CLANINFO (0x75)
        /// </summary>
        ClanInfo = 0x75,
        /// <summary>
        /// Specifies SID_CLANQUITNOTIFY (0x76)
        /// </summary>
        ClanQuitNotify = 0x76,
        /// <summary>
        /// Specifies SID_CLANINVITATION (0x77)
        /// </summary>
        ClanInvitation = 0x77,
        /// <summary>
        /// Specifies SID_CLANREMOVEMEMBER (0x78)
        /// </summary>
        ClanRemoveMember = 0x78,
        /// <summary>
        /// Specifies SID_CLANINVITATIONRESPONSE (0x79)
        /// </summary>
        ClanInvitationResponse = 0x79,
        /// <summary>
        /// Specifies SID_CLANRANKCHANGE (0x7a)
        /// </summary>
        ClanRankChange = 0x7a,
        /// <summary>
        /// Specifies SID_CLANSETMOTD (0x7b)
        /// </summary>
        ClanSetMOTD = 0x7b,
        /// <summary>
        /// Specifies SID_CLANMOTD (0x7c)
        /// </summary>
        ClanMOTD = 0x7c,
        /// <summary>
        /// Specifies SID_CLANMEMBERLIST (0x7d)
        /// </summary>
        ClanMemberList = 0x7d,
        /// <summary>
        /// Specifies SID_CLANMEMBERREMOVED (0x7e)
        /// </summary>
        ClanMemberRemoved = 0x7e,
        /// <summary>
        /// Specifies SID_CLANMEMBERSTATUSCHANGED (0x7f)
        /// </summary>
        ClanMemberStatusChanged = 0x7f,
        /// <summary>
        /// Specifies SID_CLANMEMBERRANKCHANGE (0x81)
        /// </summary>
        ClanMemberRankChange = 0x81,
        /// <summary>
        /// Specifies SID_CLANMEMBERINFORMATION (0x82)
        /// </summary>
        ClanMemberInformation = 0x82,

    }
}
