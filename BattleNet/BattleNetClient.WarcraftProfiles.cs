using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.BattleNet.Stats;
using System.Threading;
using BNSharp.MBNCSUtil;
using System.Diagnostics;
using BNSharp.BattleNet.Clans;
using System.Globalization;

namespace BNSharp.BattleNet
{
    partial class BattleNetClient
    {
        private int m_curProfileCookie;
        private Dictionary<int, WarcraftProfileEventArgs> m_warcraftProfileRequests;

        /// <summary>
        /// Requests a Warcraft 3 profile.
        /// </summary>
        /// <param name="username">The name of the user to request.</param>
        /// <param name="getFrozenThroneProfile"><see langword="true" /> to get the Frozen Throne profile;
        /// <see langword="false" /> to get the Reign of Chaos profile.</param>
        public virtual void RequestWarcraft3Profile(string username, bool getFrozenThroneProfile)
        {
            Product pr = getFrozenThroneProfile ? Product.Warcraft3Expansion : Product.Warcraft3Retail;

            int cookie = Interlocked.Increment(ref m_curProfileCookie);
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.Profile);
            pck.InsertInt32(cookie);
            pck.InsertCString(username);

            WarcraftProfileEventArgs args = new WarcraftProfileEventArgs(username, pr);
            m_warcraftProfileRequests.Add(cookie, args);

            Send(pck);
        }

        /// <summary>
        /// Requests a Warcraft 3 profile for the specified user, requesting them for the user's 
        /// specific product.
        /// </summary>
        /// <param name="user">The user for whom to request a profile.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="user"/> does 
        /// not on Warcraft III: The Reign of Chaos or The Frozen Throne.</exception>
        public virtual void RequestWarcraft3Profile(ChatUser user)
        {
            if (user.Stats.Product != Product.Warcraft3Expansion && user.Stats.Product != Product.Warcraft3Retail)
                throw new ArgumentOutOfRangeException("user", user.Stats.Product, "Cannot request Warcraft 3 Profile statistics for the specified product.");

            RequestWarcraft3Profile(user.Username, user.Stats.Product == Product.Warcraft3Expansion);
        }

        private void HandleClanMemberInformation(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int cookie = dr.ReadInt32();
            if (!m_warcraftProfileRequests.ContainsKey(cookie))
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to locate profile request with cookie {0:x2}", cookie));
                return;
            }
            WarcraftProfileEventArgs args = m_warcraftProfileRequests[cookie];

            byte success = dr.ReadByte();
            if (success != 0)
            {
                m_warcraftProfileRequests.Remove(cookie);
                ProfileLookupFailedEventArgs profileFailed = new ProfileLookupFailedEventArgs(args.Username, args.Product) { EventData = pd };
                OnProfileLookupFailed(profileFailed);
                return;
            }

            string clanName = dr.ReadCString();
            ClanRank rank = (ClanRank)dr.ReadByte();
            DateTime joined = DateTime.FromFileTime(dr.ReadInt64());

            args.Clan = new ClanProfile(clanName, rank, joined);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.WarcraftGeneral);
            pck.InsertByte((byte)WarcraftCommands.ClanInfoRequest);
            pck.InsertInt32(cookie);
            pck.InsertDwordString(args.Profile.ClanTag, 0);
            pck.InsertDwordString(args.Product.ProductCode);
            Send(pck);

            BattleNetClientResources.IncomingBufferPool.FreeBuffer(pd.Data);
        }

        private void HandleProfile(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int cookie = dr.ReadInt32();
            if (!m_warcraftProfileRequests.ContainsKey(cookie))
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to locate profile request with cookie {0:x2}", cookie));
                return;
            }
            WarcraftProfileEventArgs args = m_warcraftProfileRequests[cookie];

            byte success = dr.ReadByte();
            if (success != 0)
            {
                m_warcraftProfileRequests.Remove(cookie);
                ProfileLookupFailedEventArgs profileFailed = new ProfileLookupFailedEventArgs(args.Username, args.Product) { EventData = pd };
                OnProfileLookupFailed(profileFailed);
                return;
            }

            string desc = dr.ReadCString();
            string location = dr.ReadCString();
            string tag = dr.ReadDwordString(0);

            WarcraftProfile profile = new WarcraftProfile(desc, location, tag);
            args.Profile = profile;
            if (!string.IsNullOrEmpty(tag))
            {
                BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanMemberInformation);
                pck.InsertInt32(cookie);
                pck.InsertDwordString(tag, 0);
                pck.InsertCString(args.Username);
                Send(pck);
            }
            else
            {
                BncsPacket pck = new BncsPacket((byte)BncsPacketId.WarcraftGeneral);
                pck.InsertByte((byte)WarcraftCommands.UserInfoRequest);
                pck.InsertInt32(cookie);
                pck.InsertCString(args.Username);
                pck.InsertDwordString(args.Product.ProductCode);
                Send(pck);
            }
            

            BattleNetClientResources.IncomingBufferPool.FreeBuffer(pd.Data);
        }

        private void HandleWarcraftClanInfoRequest(DataReader dr)
        {
            int cookie = dr.ReadInt32();
            if (!m_warcraftProfileRequests.ContainsKey(cookie))
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to locate profile request with cookie {0:x2}", cookie));
                return;
            }
            WarcraftProfileEventArgs args = m_warcraftProfileRequests[cookie];

            int recordCount = dr.ReadByte();
            WarcraftClanLadderRecord[] ladderRecords = new WarcraftClanLadderRecord[recordCount];
            for (int i = 0; i < recordCount; i++)
            {
                WarcraftClanLadderType ladderType = (WarcraftClanLadderType)dr.ReadInt32();
                int wins = dr.ReadInt16();
                int losses = dr.ReadInt16();
                int level = dr.ReadByte();
                int hrs = dr.ReadByte();
                int xp = dr.ReadInt16();
                int rank = dr.ReadInt32();

                WarcraftClanLadderRecord record = new WarcraftClanLadderRecord(ladderType, wins, losses, level, hrs, xp, rank);
                ladderRecords[i] = record;
            }

            int raceRecordCount = dr.ReadByte();
            Warcraft3IconRace[] raceOrder = new Warcraft3IconRace[] { Warcraft3IconRace.Random, Warcraft3IconRace.Human, Warcraft3IconRace.Orc, Warcraft3IconRace.Undead, Warcraft3IconRace.NightElf, Warcraft3IconRace.Tournament };
            WarcraftRaceRecord[] raceRecords = new WarcraftRaceRecord[raceRecordCount];
            for (int i = 0; i < raceRecordCount; i++)
            {
                int wins = dr.ReadInt16();
                int losses = dr.ReadInt16();

                WarcraftRaceRecord record = new WarcraftRaceRecord(raceOrder[i], wins, losses);
                raceRecords[i] = record;
            }

            args.Clan.SetStats(ladderRecords, raceRecords);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.WarcraftGeneral);
            pck.InsertByte((byte)WarcraftCommands.UserInfoRequest);
            pck.InsertInt32(cookie);
            pck.InsertCString(args.Username);
            pck.InsertDwordString(args.Product.ProductCode);
            Send(pck);
        }

        private void HandleWarcraftUserInfoRequest(ParseData data, DataReader dr)
        {
            int cookie = dr.ReadInt32();
            if (!m_warcraftProfileRequests.ContainsKey(cookie))
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to locate profile request with cookie {0:x2}", cookie));
                return;
            }
            WarcraftProfileEventArgs args = m_warcraftProfileRequests[cookie];

            string iconID = dr.ReadDwordString(0);
            args.Profile.IconID = iconID;

            int recordCount = dr.ReadByte();
            WarcraftLadderRecord[] ladderRecords = new WarcraftLadderRecord[recordCount];
            for (int i = 0; i < recordCount; i++)
            {
                WarcraftLadderType ladderType = (WarcraftLadderType)dr.ReadInt32();
                int wins = dr.ReadInt16();
                int losses = dr.ReadInt16();
                int level = dr.ReadByte();
                int hrs = dr.ReadByte();
                int xp = dr.ReadInt16();
                int rank = dr.ReadInt32();

                WarcraftLadderRecord record = new WarcraftLadderRecord(ladderType, wins, losses, level, hrs, xp, rank);
                ladderRecords[i] = record;
            }

            int raceRecordCount = dr.ReadByte();
            Warcraft3IconRace[] raceOrder = new Warcraft3IconRace[] { Warcraft3IconRace.Random, Warcraft3IconRace.Human, Warcraft3IconRace.Orc, Warcraft3IconRace.Undead, Warcraft3IconRace.NightElf, Warcraft3IconRace.Tournament };
            WarcraftRaceRecord[] raceRecords = new WarcraftRaceRecord[raceRecordCount];
            for (int i = 0; i < raceRecordCount; i++)
            {
                int wins = dr.ReadInt16();
                int losses = dr.ReadInt16();

                WarcraftRaceRecord record = new WarcraftRaceRecord(raceOrder[i], wins, losses);
                raceRecords[i] = record;
            }

            int teamRecordsCount = dr.ReadByte();
            ArrangedTeamRecord[] teamRecords = new ArrangedTeamRecord[teamRecordsCount];
            for (int i = 0; i < teamRecordsCount; i++)
            {
                ArrangedTeamType teamType = (ArrangedTeamType)dr.ReadInt32();
                int wins = dr.ReadInt16();
                int losses = dr.ReadInt16();
                int level = dr.ReadByte();
                int hrs = dr.ReadByte();
                int xp = dr.ReadInt16();
                int rank = dr.ReadInt32();
                long ftLastGameplay = dr.ReadInt64();
                DateTime lastGamePlayed = DateTime.FromFileTime(ftLastGameplay);
                int numPartners = dr.ReadByte();
                string[] partnerList = new string[numPartners];
                for (int p = 0; p < numPartners; p++)
                    partnerList[p] = dr.ReadCString();

                ArrangedTeamRecord record = new ArrangedTeamRecord(teamType, wins, losses, level, hrs, xp, rank, lastGamePlayed, partnerList);
                teamRecords[i] = record;
            }

            args.Profile.SetStats(ladderRecords, teamRecords, raceRecords);

            args.EventData = data;

            OnWarcraftProfileReceived(args);
        }

        #region events
        #region WarcraftProfileReceived event
        [NonSerialized]
        private Dictionary<Priority, List<WarcraftProfileEventHandler>> __WarcraftProfileReceived = new Dictionary<Priority, List<WarcraftProfileEventHandler>>(3)
        {
            { Priority.High, new List<WarcraftProfileEventHandler>() },
            { Priority.Normal, new List<WarcraftProfileEventHandler>() },
            { Priority.Low, new List<WarcraftProfileEventHandler>() }
        };
        /// <summary>
        /// Informs listeners that a Warcraft 3 profile has been received.
        /// </summary>
        /// <remarks>
        /// <para>Registering for this event with this member will register with <see cref="Priority">Normal priority</see>.  To register for 
        /// <see cref="Priority">High</see> or <see cref="Priority">Low</see> priority, use the <see>RegisterWarcraftProfileReceivedNotification</see> and
        /// <see>UnregisterWarcraftProfileReceivedNotification</see> methods.</para>
        /// <para>Events in the JinxBot API are never guaranteed to be executed on the UI thread.  Events that affect the user interface should
        /// be marshaled back to the UI thread by the event handling code.  Generally, high-priority event handlers are
        /// raised on the thread that is parsing data from Battle.net, and lower-priority event handler are executed from the thread pool.</para>
        /// <para>JinxBot guarantees that all event handlers will be fired regardless of exceptions raised in previous event handlers.  However, 
        /// if a plugin repeatedly raises an exception, it may be forcefully unregistered from events.</para>
        /// </remarks>
        public event WarcraftProfileEventHandler WarcraftProfileReceived
        {
            add
            {
                lock (__WarcraftProfileReceived)
                {
                    if (!__WarcraftProfileReceived.ContainsKey(Priority.Normal))
                    {
                        __WarcraftProfileReceived.Add(Priority.Normal, new List<WarcraftProfileEventHandler>());
                    }
                }
                __WarcraftProfileReceived[Priority.Normal].Add(value);
            }
            remove
            {
                if (__WarcraftProfileReceived.ContainsKey(Priority.Normal))
                {
                    __WarcraftProfileReceived[Priority.Normal].Remove(value);
                }
            }
        }

        /// <summary>
        /// Registers for notification of the <see>WarcraftProfileReceived</see> event at the specified priority.
        /// </summary>
        /// <remarks>
        /// <para>The event system in the JinxBot API supports normal event registration and prioritized event registration.  You can use
        /// normal syntax to register for events at <see cref="Priority">Normal priority</see>, so no special registration is needed; this is 
        /// accessed through normal event handling syntax (the += syntax in C#, or the <see langword="Handles" lang="VB" /> in Visual Basic.</para>
        /// <para>Events in the JinxBot API are never guaranteed to be executed on the UI thread.  Events that affect the user interface should
        /// be marshaled back to the UI thread by the event handling code.  Generally, high-priority event handlers are
        /// raised on the thread that is parsing data from Battle.net, and lower-priority event handler are executed from the thread pool.</para>
        /// <para>JinxBot guarantees that all event handlers will be fired regardless of exceptions raised in previous event handlers.  However, 
        /// if a plugin repeatedly raises an exception, it may be forcefully unregistered from events.</para>
        ///	<para>To be well-behaved within JinxBot, plugins should always unregister themselves when they are being unloaded or when they 
        /// otherwise need to do so.  Plugins may opt-in to a Reflection-based event handling registration system which uses attributes to 
        /// mark methods that should be used as event handlers.</para>
        /// </remarks>
        /// <param name="p">The priority at which to register.</param>
        /// <param name="callback">The event handler that should be registered for this event.</param>
        /// <seealso cref="WarcraftProfileReceived" />
        /// <seealso cref="UnregisterWarcraftProfileReceivedNotification" />
        public void RegisterWarcraftProfileReceivedNotification(Priority p, WarcraftProfileEventHandler callback)
        {
            lock (__WarcraftProfileReceived)
            {
                if (!__WarcraftProfileReceived.ContainsKey(p))
                {
                    __WarcraftProfileReceived.Add(p, new List<WarcraftProfileEventHandler>());
                }
            }
            __WarcraftProfileReceived[p].Add(callback);
        }

        /// <summary>
        /// Unregisters for notification of the <see>WarcraftProfileReceived</see> event at the specified priority.
        /// </summary>
        /// <remarks>
        /// <para>The event system in the JinxBot API supports normal event registration and prioritized event registration.  You can use
        /// normal syntax to register for events at <see cref="Priority">Normal priority</see>, so no special registration is needed; this is 
        /// accessed through normal event handling syntax (the += syntax in C#, or the <see langword="Handles" lang="VB" /> in Visual Basic.</para>
        /// <para>Events in the JinxBot API are never guaranteed to be executed on the UI thread.  Events that affect the user interface should
        /// be marshaled back to the UI thread by the event handling code.  Generally, high-priority event handlers are
        /// raised on the thread that is parsing data from Battle.net, and lower-priority event handler are executed from the thread pool.</para>
        /// <para>JinxBot guarantees that all event handlers will be fired regardless of exceptions raised in previous event handlers.  However, 
        /// if a plugin repeatedly raises an exception, it may be forcefully unregistered from events.</para>
        ///	<para>To be well-behaved within JinxBot, plugins should always unregister themselves when they are being unloaded or when they 
        /// otherwise need to do so.  Plugins may opt-in to a Reflection-based event handling registration system which uses attributes to 
        /// mark methods that should be used as event handlers.</para>
        /// </remarks>
        /// <param name="p">The priority from which to unregister.</param>
        /// <param name="callback">The event handler that should be unregistered for this event.</param>
        /// <seealso cref="WarcraftProfileReceived" />
        /// <seealso cref="RegisterWarcraftProfileReceivedNotification" />
        public void UnregisterWarcraftProfileReceivedNotification(Priority p, WarcraftProfileEventHandler callback)
        {
            if (__WarcraftProfileReceived.ContainsKey(p))
            {
                __WarcraftProfileReceived[p].Remove(callback);
            }
        }

        /// <summary>
        /// Raises the WarcraftProfileReceived event.
        /// </summary>
        /// <remarks>
        /// <para>Only high-priority events are invoked immediately; others are deferred.  For more information, see <see>WarcraftProfileReceived</see>.</para>
        /// </remarks>
        /// <param name="e">The event arguments.</param>
        /// <seealso cref="WarcraftProfileReceived" />
        protected virtual void OnWarcraftProfileReceived(WarcraftProfileEventArgs e)
        {
            foreach (WarcraftProfileEventHandler eh in __WarcraftProfileReceived[Priority.High])
            {
                try
                {
                    eh(this, e);
                }
                catch (Exception ex)
                {
                    ReportException(
                        ex,
                        new KeyValuePair<string, object>("delegate", eh),
                        new KeyValuePair<string, object>("Event", "WarcraftProfileReceived"),
                        new KeyValuePair<string, object>("param: priority", Priority.High),
                        new KeyValuePair<string, object>("param: this", this),
                        new KeyValuePair<string, object>("param: e", e)
                        );
                }
            }

            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
            {
                foreach (WarcraftProfileEventHandler eh in __WarcraftProfileReceived[Priority.Normal])
                {
                    try
                    {
                        eh(this, e);
                    }
                    catch (Exception ex)
                    {
                        ReportException(
                            ex,
                            new KeyValuePair<string, object>("delegate", eh),
                            new KeyValuePair<string, object>("Event", "WarcraftProfileReceived"),
                            new KeyValuePair<string, object>("param: priority", Priority.Normal),
                            new KeyValuePair<string, object>("param: this", this),
                            new KeyValuePair<string, object>("param: e", e)
                            );
                    }
                }
                ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                {
                    foreach (WarcraftProfileEventHandler eh in __WarcraftProfileReceived[Priority.Low])
                    {
                        try
                        {
                            eh(this, e);
                        }
                        catch (Exception ex)
                        {
                            ReportException(
                                ex,
                                new KeyValuePair<string, object>("delegate", eh),
                                new KeyValuePair<string, object>("Event", "WarcraftProfileReceived"),
                                new KeyValuePair<string, object>("param: priority", Priority.Low),
                                new KeyValuePair<string, object>("param: this", this),
                                new KeyValuePair<string, object>("param: e", e)
                                );
                        }
                    }
                    FreeArgumentResources(e as BaseEventArgs);
                });
            });
        }
        #endregion
        #region ProfileLookupFailed event
        [NonSerialized]
        private Dictionary<Priority, List<ProfileLookupFailedEventHandler>> __ProfileLookupFailed = new Dictionary<Priority, List<ProfileLookupFailedEventHandler>>(3)
        {
            { Priority.High, new List<ProfileLookupFailedEventHandler>() },
            { Priority.Normal, new List<ProfileLookupFailedEventHandler>() },
            { Priority.Low, new List<ProfileLookupFailedEventHandler>() }
        };
        /// <summary>
        /// Informs listeners that the lookup of a Warcraft 3 profile failed.
        /// </summary>
        /// <remarks>
        /// <para>Registering for this event with this member will register with <see cref="Priority">Normal priority</see>.  To register for 
        /// <see cref="Priority">High</see> or <see cref="Priority">Low</see> priority, use the <see>RegisterProfileLookupFailedNotification</see> and
        /// <see>UnregisterProfileLookupFailedNotification</see> methods.</para>
        /// <para>Events in the JinxBot API are never guaranteed to be executed on the UI thread.  Events that affect the user interface should
        /// be marshaled back to the UI thread by the event handling code.  Generally, high-priority event handlers are
        /// raised on the thread that is parsing data from Battle.net, and lower-priority event handler are executed from the thread pool.</para>
        /// <para>JinxBot guarantees that all event handlers will be fired regardless of exceptions raised in previous event handlers.  However, 
        /// if a plugin repeatedly raises an exception, it may be forcefully unregistered from events.</para>
        /// </remarks>
        public event ProfileLookupFailedEventHandler ProfileLookupFailed
        {
            add
            {
                lock (__ProfileLookupFailed)
                {
                    if (!__ProfileLookupFailed.ContainsKey(Priority.Normal))
                    {
                        __ProfileLookupFailed.Add(Priority.Normal, new List<ProfileLookupFailedEventHandler>());
                    }
                }
                __ProfileLookupFailed[Priority.Normal].Add(value);
            }
            remove
            {
                if (__ProfileLookupFailed.ContainsKey(Priority.Normal))
                {
                    __ProfileLookupFailed[Priority.Normal].Remove(value);
                }
            }
        }

        /// <summary>
        /// Registers for notification of the <see>ProfileLookupFailed</see> event at the specified priority.
        /// </summary>
        /// <remarks>
        /// <para>The event system in the JinxBot API supports normal event registration and prioritized event registration.  You can use
        /// normal syntax to register for events at <see cref="Priority">Normal priority</see>, so no special registration is needed; this is 
        /// accessed through normal event handling syntax (the += syntax in C#, or the <see langword="Handles" lang="VB" /> in Visual Basic.</para>
        /// <para>Events in the JinxBot API are never guaranteed to be executed on the UI thread.  Events that affect the user interface should
        /// be marshaled back to the UI thread by the event handling code.  Generally, high-priority event handlers are
        /// raised on the thread that is parsing data from Battle.net, and lower-priority event handler are executed from the thread pool.</para>
        /// <para>JinxBot guarantees that all event handlers will be fired regardless of exceptions raised in previous event handlers.  However, 
        /// if a plugin repeatedly raises an exception, it may be forcefully unregistered from events.</para>
        ///	<para>To be well-behaved within JinxBot, plugins should always unregister themselves when they are being unloaded or when they 
        /// otherwise need to do so.  Plugins may opt-in to a Reflection-based event handling registration system which uses attributes to 
        /// mark methods that should be used as event handlers.</para>
        /// </remarks>
        /// <param name="p">The priority at which to register.</param>
        /// <param name="callback">The event handler that should be registered for this event.</param>
        /// <seealso cref="ProfileLookupFailed" />
        /// <seealso cref="UnregisterProfileLookupFailedNotification" />
        public void RegisterProfileLookupFailedNotification(Priority p, ProfileLookupFailedEventHandler callback)
        {
            lock (__ProfileLookupFailed)
            {
                if (!__ProfileLookupFailed.ContainsKey(p))
                {
                    __ProfileLookupFailed.Add(p, new List<ProfileLookupFailedEventHandler>());
                }
            }
            __ProfileLookupFailed[p].Add(callback);
        }

        /// <summary>
        /// Unregisters for notification of the <see>ProfileLookupFailed</see> event at the specified priority.
        /// </summary>
        /// <remarks>
        /// <para>The event system in the JinxBot API supports normal event registration and prioritized event registration.  You can use
        /// normal syntax to register for events at <see cref="Priority">Normal priority</see>, so no special registration is needed; this is 
        /// accessed through normal event handling syntax (the += syntax in C#, or the <see langword="Handles" lang="VB" /> in Visual Basic.</para>
        /// <para>Events in the JinxBot API are never guaranteed to be executed on the UI thread.  Events that affect the user interface should
        /// be marshaled back to the UI thread by the event handling code.  Generally, high-priority event handlers are
        /// raised on the thread that is parsing data from Battle.net, and lower-priority event handler are executed from the thread pool.</para>
        /// <para>JinxBot guarantees that all event handlers will be fired regardless of exceptions raised in previous event handlers.  However, 
        /// if a plugin repeatedly raises an exception, it may be forcefully unregistered from events.</para>
        ///	<para>To be well-behaved within JinxBot, plugins should always unregister themselves when they are being unloaded or when they 
        /// otherwise need to do so.  Plugins may opt-in to a Reflection-based event handling registration system which uses attributes to 
        /// mark methods that should be used as event handlers.</para>
        /// </remarks>
        /// <param name="p">The priority from which to unregister.</param>
        /// <param name="callback">The event handler that should be unregistered for this event.</param>
        /// <seealso cref="ProfileLookupFailed" />
        /// <seealso cref="RegisterProfileLookupFailedNotification" />
        public void UnregisterProfileLookupFailedNotification(Priority p, ProfileLookupFailedEventHandler callback)
        {
            if (__ProfileLookupFailed.ContainsKey(p))
            {
                __ProfileLookupFailed[p].Remove(callback);
            }
        }

        /// <summary>
        /// Raises the ProfileLookupFailed event.
        /// </summary>
        /// <remarks>
        /// <para>Only high-priority events are invoked immediately; others are deferred.  For more information, see <see>ProfileLookupFailed</see>.</para>
        /// </remarks>
        /// <param name="e">The event arguments.</param>
        /// <seealso cref="ProfileLookupFailed" />
        protected virtual void OnProfileLookupFailed(ProfileLookupFailedEventArgs e)
        {
            foreach (ProfileLookupFailedEventHandler eh in __ProfileLookupFailed[Priority.High])
            {
                try
                {
                    eh(this, e);
                }
                catch (Exception ex)
                {
                    ReportException(
                        ex,
                        new KeyValuePair<string, object>("delegate", eh),
                        new KeyValuePair<string, object>("Event", "ProfileLookupFailed"),
                        new KeyValuePair<string, object>("param: priority", Priority.High),
                        new KeyValuePair<string, object>("param: this", this),
                        new KeyValuePair<string, object>("param: e", e)
                        );
                }
            }

            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
            {
                foreach (ProfileLookupFailedEventHandler eh in __ProfileLookupFailed[Priority.Normal])
                {
                    try
                    {
                        eh(this, e);
                    }
                    catch (Exception ex)
                    {
                        ReportException(
                            ex,
                            new KeyValuePair<string, object>("delegate", eh),
                            new KeyValuePair<string, object>("Event", "ProfileLookupFailed"),
                            new KeyValuePair<string, object>("param: priority", Priority.Normal),
                            new KeyValuePair<string, object>("param: this", this),
                            new KeyValuePair<string, object>("param: e", e)
                            );
                    }
                }
                ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                {
                    foreach (ProfileLookupFailedEventHandler eh in __ProfileLookupFailed[Priority.Low])
                    {
                        try
                        {
                            eh(this, e);
                        }
                        catch (Exception ex)
                        {
                            ReportException(
                                ex,
                                new KeyValuePair<string, object>("delegate", eh),
                                new KeyValuePair<string, object>("Event", "ProfileLookupFailed"),
                                new KeyValuePair<string, object>("param: priority", Priority.Low),
                                new KeyValuePair<string, object>("param: this", this),
                                new KeyValuePair<string, object>("param: e", e)
                                );
                        }
                    }
                    FreeArgumentResources(e as BaseEventArgs);
                });
            });
        }
        #endregion
        #endregion
    }
}
