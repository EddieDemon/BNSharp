using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.Plugins;

namespace BNSharp.Net
{
    internal class DefaultPacketPriorityProvider : IPacketPriorityProvider
    {
        #region IPacketPriorityProvider Members

        public bool Defines(BncsPacketId packetToSearch)
        {
            return true;
        }

        public Priority GetPriority(BncsPacketId packetToSearch)
        {
            return Priority.Normal;
        }

        #endregion
    }
}
