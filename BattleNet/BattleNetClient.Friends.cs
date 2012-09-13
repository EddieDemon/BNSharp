using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.BattleNet.Friends;
using BNSharp.MBNCSUtil;
using BNSharp.BattleNet;
using System.Diagnostics;

namespace BNSharp.BattleNet
{
    partial class BattleNetClient
    {
        private List<FriendUser> m_friendsList = new List<FriendUser>();

        partial void ResetFriendsState()
        {
            m_friendsList.Clear();
        }

        private void HandleFriendsList(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int numEntries = dr.ReadByte();
            FriendUser[] list = new FriendUser[numEntries];
            for (int i = 0; i < numEntries; i++)
            {
                FriendUser friend = __ParseNewFriend(dr, i);
                list[i] = friend;
            }

            m_friendsList.AddRange(list);

            Debug.WriteLine("Received friends list; " + list.Length + " user on it.");

            FriendListReceivedEventArgs args = new FriendListReceivedEventArgs(list) { EventData = pd };
            OnFriendListReceived(args);
        }

        private static FriendUser __ParseNewFriend(DataReader dr, int i)
        {
            string acct = dr.ReadCString();
            FriendStatus status = (FriendStatus)dr.ReadByte();
            FriendLocation location = (FriendLocation)dr.ReadByte();
            string productID = dr.ReadDwordString(0);
            Product prod = null;
            string locationName = string.Empty;
            if (location == FriendLocation.Offline)
            {
                dr.Seek(1);
            }
            else
            {
                prod = Product.GetByProductCode(productID);
                locationName = dr.ReadCString();
            }

            FriendUser friend = new FriendUser(i, acct, status, location, prod, locationName);
            return friend;
        }

        private void HandleFriendUpdate(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            byte entry = dr.ReadByte();
            if (m_friendsList.Count <= entry)
            {
                return;
            }
            FriendUser friend = m_friendsList[entry];
            friend.Status = (FriendStatus)dr.ReadByte();
            friend.LocationType = (FriendLocation)dr.ReadByte();
            string prodID = dr.ReadDwordString(0);
            friend.Location = dr.ReadCString();

            if (friend.LocationType != FriendLocation.Offline)
            {
                friend.Product = Product.GetByProductCode(prodID);
            }
            else
            {
                friend.Product = null;
            }

            FriendUpdatedEventArgs args = new FriendUpdatedEventArgs(friend) { EventData = pd };
            OnFriendUpdated(args);
        }

        private void HandleFriendAdded(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int nextIndex = m_friendsList.Count;
            FriendUser newFriend = __ParseNewFriend(dr, nextIndex);
            m_friendsList.Add(newFriend);

            FriendAddedEventArgs args = new FriendAddedEventArgs(newFriend) { EventData = pd };
            OnFriendAdded(args);
        }

        private void HandleFriendRemoved(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            byte index = dr.ReadByte();

            FriendUser removed = m_friendsList[index];
            m_friendsList.RemoveAt(index);

            for (int i = index; i < m_friendsList.Count; i++)
            {
                m_friendsList[i].Index -= 1;
            }

            FriendRemovedEventArgs args = new FriendRemovedEventArgs(removed) { EventData = pd };
            OnFriendRemoved(args);
        }

        private void HandleFriendMoved(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            byte index = dr.ReadByte();
            byte newIndex = dr.ReadByte();

            FriendUser friend = m_friendsList[index];
            friend.Index = newIndex;
            m_friendsList.Insert(newIndex, friend);

            if (newIndex < index)
            {
                for (int i = newIndex + 1; i <= index; i++)
                {
                    m_friendsList[i].Index += 1;
                }
            }
            else if (newIndex > index)
            {
                for (int i = index; i < newIndex; i++)
                {
                    m_friendsList[i].Index -= 1;
                }
            }

            FriendMovedEventArgs args = new FriendMovedEventArgs(friend, newIndex) { EventData = pd };
            OnFriendMoved(args);
        }
    }
}
