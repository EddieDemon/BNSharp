using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp
{
    /// <summary>
    /// Specifies priorities for data handling. 
    /// </summary>
    /// <remarks>
    /// <para>When using BNSharp for a simple client, use of the Priority enumeration is optional.  However, if you want
    /// to develop something such as an extremely secure channel moderation client, it is highly recommended that you utilize both 
    /// the prioritized packet parsing engine and the prioritized event handler system.</para>
    /// </remarks>
    public enum Priority
    {
        /// <summary>
        /// Specifies the highest priority.  This enumeration value is 5.
        /// </summary>
        High = 3,
        /// <summary>
        /// Specifies the normal priority.  This enumeration value is 3.
        /// </summary>
        Normal = 2,
        /// <summary>
        /// Specifies the lowest priority.  This enumeration value is 1.
        /// </summary>
        Low = 1,
    }
}
