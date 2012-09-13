using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.Plugins;

namespace BNSharp.Net
{
    internal sealed class CombinedPacketPriorityProvider : IPacketPriorityProvider
    {
        private Dictionary<BncsPacketId, Priority> m_priorityMap;
        private List<IPacketPriorityProvider> m_providerList;
        private object syncObject = new object();

        public CombinedPacketPriorityProvider()
        {
            m_priorityMap = new Dictionary<BncsPacketId, Priority>();
            m_providerList = new List<IPacketPriorityProvider>();
            m_providerList.Add(new DefaultPacketPriorityProvider());
        }

        private void InvalidateDictionary()
        {
            m_priorityMap.Clear();
        }

        public void RegisterNewProvider(IPacketPriorityProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            lock (syncObject)
            {
                m_providerList.Add(provider);
                InvalidateDictionary();
            }
        }

        public void UnregisterProvider(IPacketPriorityProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            lock (syncObject)
            {
                m_providerList.Remove(provider);
                InvalidateDictionary();
            }
        }

        #region IPacketPriorityProvider Members

        public bool Defines(BncsPacketId packetToSearch)
        {
            return true;
        }

        public Priority GetPriority(BncsPacketId packetToSearch)
        {
            if (m_priorityMap.ContainsKey(packetToSearch))
            {
                return m_priorityMap[packetToSearch];
            }
            else
            {
                lock (syncObject)
                {
                    // do a double lookup; gain speed on cache hits by sacrificing a double-check 
                    // on cache misses.
                    if (!m_priorityMap.ContainsKey(packetToSearch))
                    {
                        Priority result = default(Priority);
                        for (int i = m_providerList.Count - 1; i >= 0; i--)
                        {
                            IPacketPriorityProvider provider = m_providerList[i];
                            if (provider.Defines(packetToSearch))
                            {
                                result = provider.GetPriority(packetToSearch);
                                break;
                            }
                        }

                        m_priorityMap[packetToSearch] = result;
                        return result;
                    }
                    else
                    {
                        return m_priorityMap[packetToSearch];
                    }
                }
            }
        }

        #endregion
    }
}
