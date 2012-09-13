using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.MBNCSUtil;
using System.Globalization;
using System.Net;
using System.Collections.ObjectModel;
using System.Threading;
using BNSharp.Plugins;
using BNSharp.Net;
using System.IO;
using System.Configuration;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Implements a client connection to Battle.net.
    /// </summary>
    /// <remarks>
    /// <para>This is the primary class that should be used when implementing a Battle.net client.  To implement one, you only need to implement
    /// the <see>IBattleNetSettings</see> interface, which provides information about a connection to Battle.net.  Once this interface is implemented
    /// and this object is created, the client should register for events and that's it.</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public partial class BattleNetClient : ThinProxiedConnectionBase
    {
        const string PLATFORM_TYPE = "IX86";
        const string EMOTE_1 = "/me ";
        const string EMOTE_2 = "/emote ";
        const string COMMAND_START = "/";

        #region partial methods that exist in the other partial files
        partial void InitializeListenState();
        partial void InitializeParseDictionaries();
        #endregion

        #region fields
        private IBattleNetSettings m_settings;
        private bool m_closing;
        private Dictionary<string, ChatUser> m_namesToUsers = new Dictionary<string, ChatUser>();
        private Dictionary<int, UserProfileRequest> m_profileRequests = new Dictionary<int, UserProfileRequest>();
        private int m_currentProfileRequestID;
        private string m_channelName;
        private ICommandQueue m_queue;
        private QueuedMessageReadyCallback m_messageReadyCallback;
        #endregion

        #region .ctor
        /// <summary>
        /// Creates a new <see>BattleNetClient</see> with the specified settings.
        /// </summary>
        /// <param name="settings">An object containing the settings for a Battle.net connection.</param>
        /// <exception cref="BattleNetSettingsErrorsException">Thrown if required parameters of the 
        /// <paramref name="settings"/> object are invalid.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public BattleNetClient(IBattleNetSettings settings)
            : base(settings.Gateway.ServerHost, settings.Gateway.ServerPort)
        {
            ValidateSettings(settings);

            m_settings = settings;
            m_priorityProvider = new CombinedPacketPriorityProvider();

            InitializeListenState();

            InitializeParseDictionaries();

            m_queue = new DefaultCommandQueue();
            m_messageReadyCallback = SendCallbackImpl;
            m_queue.MessageReady += m_messageReadyCallback;
        }

        private static void ValidateSettings(IBattleNetSettings settings)
        {
            BattleNetSettingsErrors errors = BattleNetSettingsErrors.None;

            if (!File.Exists(settings.GameExe))
                errors |= BattleNetSettingsErrors.GameExeMissingOrNotFound;
            if (!File.Exists(settings.GameFile2))
                errors |= BattleNetSettingsErrors.GameFile2MissingOrNotFound;
            if (!File.Exists(settings.GameFile3))
                errors |= BattleNetSettingsErrors.GameFile3MissingOrNotFound;
            if (string.IsNullOrEmpty(settings.Username))
                errors |= BattleNetSettingsErrors.UserNameNull;
            if (!Enum.IsDefined(typeof(PingType), settings.PingMethod))
                errors |= BattleNetSettingsErrors.InvalidPingType;
            if (string.IsNullOrEmpty(settings.Gateway.ServerHost))
                errors |= BattleNetSettingsErrors.InvalidGatewayServer;

            if (settings.CdKeyOwner == null)
                settings.CdKeyOwner = string.Empty;
            if (settings.Password == null)
                settings.Password = string.Empty;

            Product productToUse = Product.GetByProductCode(settings.Client);
            if (productToUse == null)
                errors |= BattleNetSettingsErrors.InvalidEmulationClient;
            else
            {
                if (!productToUse.CanConnect)
                    errors |= BattleNetSettingsErrors.InvalidEmulationClient;
                if (string.IsNullOrEmpty(settings.CdKey1))
                    errors |= BattleNetSettingsErrors.PrimaryCdKeyMissingOrInvalid;
                else
                {
                    try
                    {
                        CdKey test = new CdKey(settings.CdKey1);
                        if (!test.IsValid)
                            errors |= BattleNetSettingsErrors.PrimaryCdKeyMissingOrInvalid;
                    }
                    catch
                    {
                        errors |= BattleNetSettingsErrors.PrimaryCdKeyMissingOrInvalid;
                    }
                }
                if (productToUse.NeedsTwoKeys && string.IsNullOrEmpty(settings.CdKey2))
                    errors |= BattleNetSettingsErrors.SecondaryCdKeyMissingOrInvalid;
                else
                {
                    if (productToUse.NeedsTwoKeys)
                    {
                        try
                        {
                            CdKey test2 = new CdKey(settings.CdKey2);
                            if (!test2.IsValid)
                                errors |= BattleNetSettingsErrors.SecondaryCdKeyMissingOrInvalid;
                        }
                        catch
                        {
                            errors |= BattleNetSettingsErrors.SecondaryCdKeyMissingOrInvalid;
                        }
                    }
                }
                if (productToUse.NeedsLockdown)
                {
                    if (string.IsNullOrEmpty(settings.ImageFile) || !File.Exists(settings.ImageFile))
                        errors |= BattleNetSettingsErrors.LockdownFileMissingOrNotFound;
                }
            }

            if (errors != BattleNetSettingsErrors.None)
                throw new BattleNetSettingsErrorsException(errors);
        }
        #endregion

        #region virtual methods
        /// <summary>
        /// Sends a data buffer to the server.
        /// </summary>
        /// <param name="packet">The buffer to send.</param>
        /// <remarks>
        /// <para>Use of this method is preferred when sending binary messages because, after sending the buffer, it frees the buffer from the outgoing buffer pool.  If you
        /// are only sending a text command, you should use <see>Send(string)</see>; it not only automatically creates the packet, but uses the speed delay provider, 
        /// if any, assigned to the <see>CommandQueueProvider</see> property.</para>
        /// </remarks>
        public virtual void Send(DataBuffer packet)
        {
            Send(packet.UnderlyingBuffer, 0, packet.Count);
            BattleNetClientResources.OutgoingBufferPool.FreeBuffer(packet.UnderlyingBuffer);
        }

        /// <summary>
        /// Begins the connection to Battle.net.
        /// </summary>
        /// <returns><see langword="true" /> if the connection succeeded; otherwise <see langword="false" />.</returns>
        public override bool Connect()
        {
            BattleNetClientResources.RegisterClient(this);

            bool ok = base.Connect();
            if (ok)
            {
                InitializeListenState();

                CultureInfo ci = CultureInfo.CurrentCulture;
                RegionInfo ri = RegionInfo.CurrentRegion;
                TimeSpan ts = DateTime.UtcNow - DateTime.Now;

                OnConnected(BaseEventArgs.GetEmpty(null));

                Send(new byte[] { 1 });

                BncsPacket pck = new BncsPacket((byte)BncsPacketId.AuthInfo);
                pck.Insert(0);
                pck.InsertDwordString(PLATFORM_TYPE); // platform
                pck.InsertDwordString(m_settings.Client); // product
                pck.InsertInt32(m_settings.VersionByte); // verbyte
                pck.InsertDwordString(string.Concat(ci.TwoLetterISOLanguageName, ri.TwoLetterISORegionName));
                pck.InsertByteArray(LocalEP.Address.GetAddressBytes());
                pck.InsertInt32((int)ts.TotalMinutes);
                pck.InsertInt32(ci.LCID);
                pck.InsertInt32(ci.LCID);
                pck.InsertCString(ri.ThreeLetterWindowsRegionName);
                pck.InsertCString(ri.DisplayName);

                Send(pck);

                if (Settings.PingMethod == PingType.ZeroMs)
                {
                    pck = new BncsPacket((byte)BncsPacketId.Ping);
                    pck.InsertInt32(new Random().Next());
                    Send(pck);
                }

                StartParsing();

                StartListening();
            }

            return ok;
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public override void Close()
        {
            base.Close();
            if (!m_closing)
            {
                m_closing = true;

                BattleNetClientResources.UnregisterClient(this);

                OnDisconnected(BaseEventArgs.GetEmpty(null));

                m_queue.Clear(); // clear the outgoing command queue

                StopParsingAndListening();

                ResetConnectionState();
            }
            else 
                m_closing = false;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (IsConnected)
                    Close();

                if (m_parseWait != null)
                {
                    m_parseWait.Close();
                    m_parseWait = null;
                }

                if (m_tmr != null)
                {
                    m_tmr.Dispose();
                    m_tmr = null;
                }

                if (m_adTmr != null)
                {
                    m_adTmr.Dispose();
                    m_adTmr = null;
                }
            }
        }

        // Implements the actual sending of the text.  This method is called as the callback referenced by m_messageReadyCallback, hooked into 
        // the queue's MessageReady event.
        private void SendCallbackImpl(string text)
        {
            if (IsConnected)
            {
                BncsPacket pck = new BncsPacket((byte)BncsPacketId.ChatCommand);
                pck.InsertCString(text, Encoding.UTF8);
                Send(pck);
                if (text.StartsWith(EMOTE_1, StringComparison.OrdinalIgnoreCase) || text.StartsWith(EMOTE_2, StringComparison.OrdinalIgnoreCase))
                {
                    // do nothing, but we need this case first so that command sent doesn't fire for emotes.
                }
                else if (text.StartsWith(COMMAND_START, StringComparison.Ordinal))
                {
                    OnCommandSent(new InformationEventArgs(text));
                }
                else
                {
                    ChatMessageEventArgs cme = new ChatMessageEventArgs(ChatEventType.Talk, UserFlags.None, this.m_uniqueUN, text);
                    OnMessageSent(cme);
                }
            }
        }

        /// <summary>
        /// Sends a textual message to the server.
        /// </summary>
        /// <param name="text">The message to send.</param>
        /// <param name="priority">The priority at which to send the message.</param>
        /// <exception cref="InvalidOperationException">Thrown if the client is not connected.</exception>
        /// <exception cref="ProtocolViolationException">Thrown if <paramref name="text"/> is longer than 223 characters.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is <see langword="null" /> or zero-length.</exception>
        public virtual void Send(string text, Priority priority)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(Strings.param_text);

            if (text.Length > 223)
                throw new ProtocolViolationException(Strings.BnetClient_Send_TooLong);

            if (IsConnected)
            {
                m_queue.EnqueueMessage(text, priority);
            }
            else
            {
                throw new InvalidOperationException(Strings.BnetClient_Send_NotConnected);
            }
        }

        /// <summary>
        /// Sends a textual message to the server at normal priority.
        /// </summary>
        /// <param name="text">The message to send.</param>
        /// <exception cref="InvalidOperationException">Thrown if the client is not connected.</exception>
        /// <exception cref="ProtocolViolationException">Thrown if <paramref name="text"/> is longer than 223 characters.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is <see langword="null" />.</exception>
        public virtual void SendMessage(string text)
        {
            Send(text, Priority.Normal);
        }

        /// <summary>
        /// Creates a new account, attempting to use the login information provided in the settings.
        /// </summary>
        public virtual void CreateAccount()
        {
            bool isClientWar3 = (m_settings.Client.Equals(Product.Warcraft3Retail.ProductCode, StringComparison.Ordinal) || m_settings.Client.Equals(Product.Warcraft3Expansion.ProductCode, StringComparison.Ordinal));
            if (isClientWar3)
            {
                CreateAccountNLS();
            }
            else
            {
                CreateAccountOld();
            }
        }

        /// <summary>
        /// Allows the client to continue logging in if the login has stopped due to a non-existent username or password.
        /// </summary>
        /// <remarks>
        /// <para>If a <see>LoginFailed</see> event occurs, the client is not automatically disconnected.  The UI can then present an interface
        /// by which the user may modify the client's <see>Settings</see> instance with proper login information.  Once this has been done, the 
        /// user may then call this method to attempt to log in again.</para>
        /// <para>This method does not need to be called after the <see>AccountCreated</see> event.</para>
        /// </remarks>
        public virtual void ContinueLogin()
        {
            bool isClientWar3 = (m_settings.Client.Equals(Product.Warcraft3Retail.ProductCode, StringComparison.Ordinal) || m_settings.Client.Equals(Product.Warcraft3Expansion.ProductCode, StringComparison.Ordinal));
            if (isClientWar3)
            {
                LoginAccountNLS();
            }
            else
            {
                LoginAccountOld();
            }
        }

        /// <summary>
        /// Informs the server that an ad has been displayed.  This should be sent whenever an ad 
        /// is updated on the client.
        /// </summary>
        /// <param name="adID">The ID of the ad assigned by the server.</param>
        public virtual void DisplayAd(int adID)
        {
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.DisplayAd);
            pck.InsertDwordString(PLATFORM_TYPE);
            pck.InsertDwordString(Settings.Client);
            pck.InsertInt32(adID);
            pck.InsertInt16(0); // NULL strings for filename and URL.

            Send(pck);
        }

        /// <summary>
        /// Informs the server that an ad has been clicked.
        /// </summary>
        /// <param name="adID">The ID of the ad assigned by the server.</param>
        public virtual void ClickAd(int adID)
        {
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClickAd);
            pck.InsertInt32(adID);
            pck.InsertInt32(1); // non-SID_QUERYADURL request

            Send(pck);
        }

        /// <summary>
        /// Sends a binary channel join command.
        /// </summary>
        /// <param name="channelName">The name of the channel to join.</param>
        /// <param name="method">The specific way by which to join.  This should typically be 
        /// set to <see cref="JoinMethod">JoinMethod.NoCreate</see>.</param>
        public virtual void JoinChannel(string channelName, JoinMethod method)
        {
            if (string.IsNullOrEmpty(channelName))
                throw new ArgumentNullException(Strings.param_channelName);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.JoinChannel);
            pck.InsertInt32((int)method);
            pck.InsertCString(channelName);

            Send(pck);
        }

        #region helpers
        private void LoginAccountOld()
        {
            switch (m_settings.Client)
            {
                case "W2BN":
                    BncsPacket pck0x29 = new BncsPacket((byte)BncsPacketId.LogonResponse);
                    pck0x29.Insert(m_clientToken);
                    pck0x29.Insert(m_srvToken);
                    pck0x29.InsertByteArray(OldAuth.DoubleHashPassword(m_settings.Password, m_clientToken, m_srvToken));
                    pck0x29.InsertCString(m_settings.Username);

                    Send(pck0x29);
                    break;
                case "STAR":
                case "SEXP":
                case "D2DV":
                case "D2XP":
                    BncsPacket pck0x3a = new BncsPacket((byte)BncsPacketId.LogonResponse2);
                    pck0x3a.Insert(m_clientToken);
                    pck0x3a.Insert(m_srvToken);
                    pck0x3a.InsertByteArray(OldAuth.DoubleHashPassword(
                        m_settings.Password,
                        m_clientToken, m_srvToken));
                    pck0x3a.InsertCString(m_settings.Username);

                    Send(pck0x3a);
                    break;

                default:
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Strings.BnetClient_LoginAccountOld_ClientNotSupported_fmt, m_settings.Client));
            }
        }

        private void LoginAccountNLS()
        {
            m_nls = new NLS(m_settings.Username, m_settings.Password);

            BncsPacket pck0x53 = new BncsPacket((byte)BncsPacketId.AuthAccountLogon);
            m_nls.LoginAccount(pck0x53);
            Send(pck0x53);
        }


        private void CreateAccountOld()
        {
            byte[] passwordHash = OldAuth.HashPassword(m_settings.Password);
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.CreateAccount2);
            pck.InsertByteArray(passwordHash);
            pck.InsertCString(m_settings.Username);

            Send(pck);
        }

        private void CreateAccountNLS()
        {
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.AuthAccountCreate);
            m_nls = new NLS(m_settings.Username, m_settings.Password);
            m_nls.CreateAccount(pck);

            Send(pck);
        }
        #endregion
        #endregion

        #region properties
        /// <summary>
        /// Gets the <see cref="IBattleNetSettings">settings</see> associated with this connection.
        /// </summary>
        public IBattleNetSettings Settings
        {
            get { return m_settings; }
        }

        /// <summary>
        /// Gets or sets a command queue implementation to be used by the Battle.net client.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this property is set while the 
        /// client is connected.</exception>
        public ICommandQueue CommandQueue
        {
            get { return m_queue; }
            set
            {
                if (IsConnected)
                    throw new InvalidOperationException(Strings.BnetClient_setCommandQueue_Connected);

                m_queue.Clear();
                if (value == null)
                {
                    m_queue.MessageReady -= m_messageReadyCallback;
                    m_queue = new DefaultCommandQueue();
                    m_queue.MessageReady += m_messageReadyCallback;
                }
                else
                {
                    m_queue.MessageReady -= m_messageReadyCallback;
                    m_queue = value;
                    m_queue.MessageReady += m_messageReadyCallback;
                }
            }
        }

        /// <summary>
        /// Gets a read-only list of all of the users in the current channel.
        /// </summary>
        public ReadOnlyCollection<ChatUser> Channel
        {
            get
            {
                List<ChatUser> users = new List<ChatUser>(m_namesToUsers.Values);
                return new ReadOnlyCollection<ChatUser>(users);
            }
        }

        /// <summary>
        /// Requests a user's profile.
        /// </summary>
        /// <param name="accountName">The name of the user for whom to request information.</param>
        /// <param name="profile">The profile request, which should contain the keys to request.</param>
        public virtual void RequestUserProfile(string accountName, UserProfileRequest profile)
        {
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ReadUserData);
            pck.InsertInt32(1);
            pck.InsertInt32(profile.Count);
            int currentRequest = Interlocked.Increment(ref m_currentProfileRequestID);
            pck.InsertInt32(currentRequest);
            pck.InsertCString(accountName);
            foreach (UserProfileKey key in profile)
            {
                pck.InsertCString(key.Key);
            }

            m_profileRequests.Add(currentRequest, profile);

            Send(pck);
        }

        /// <summary>
        /// Gets the name of the current channel.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if this property is set to a null or empty string.</exception>
        public string ChannelName
        {
            get { return m_channelName; }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                m_channelName = value;
            }
        }

        /// <summary>
        /// Gets the unique username of the current user.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if this property is set to a null or empty string.</exception>
        public string UniqueUsername
        {
            get { return m_uniqueUN; }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                m_uniqueUN = value;
            }
        }
        #endregion
    }
}
