using System;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Specifies the type of arranged team for an <see>ArrangedTeamRecord</see> when viewing a player's profile.
    /// </summary>
    public enum ArrangedTeamType
    {
        /// <summary>
        /// Specifies that an arranged team record represents a 2vs2 group.  Values: 0x32565332 (844518194).
        /// </summary>
        TwoVsTwo = 0x32565332,
        /// <summary>
        /// Specifies that an arranged team record represents a 3vs3 group.  Value: 0x33565333 (861295411).
        /// </summary>
        ThreeVsThree = 0x33565333,
        /// <summary>
        /// Specifies that an arranged team is a 4vs4 group.  Value: 0x34565334 (878072627).
        /// </summary>
        FourVsFour = 0x34565334,
    }
}
