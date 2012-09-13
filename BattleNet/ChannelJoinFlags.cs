using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp.BattleNet
{
    [Flags]
    internal enum ChannelJoinFlags
    {
        Standard = 0,
        FirstJoin = 1,
        ForcedJoin = 2,
        Diablo2FirstJoin = 5,
    }
}
