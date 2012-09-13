using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BNSharp
{
    /// <summary>
    /// Contains error information raised when a new <see>BattleNetClient</see> is constructed with 
    /// invalid settings.
    /// </summary>
    [Serializable]
    public sealed class BattleNetSettingsErrorsException : Exception
    {
        private BattleNetSettingsErrors m_errors;

        internal BattleNetSettingsErrorsException(BattleNetSettingsErrors errors)
            : base()
        {
            m_errors = errors;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        private BattleNetSettingsErrorsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_errors = (BattleNetSettingsErrors)info.GetValue("m_errors", typeof(BattleNetSettingsErrors));
        }

        /// <summary>
        /// Gets the bitwise combination of errors that were associated with this exception.
        /// </summary>
        public BattleNetSettingsErrors Errors
        {
            get { return m_errors; }
        }

        /// <inheritdoc />
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("m_errors", m_errors);
        }
    }

    /// <summary>
    /// Specifies one or more errors that were found with an initialized <see>BattleNetClient</see>.
    /// </summary>
    [Flags]
    public enum BattleNetSettingsErrors
    {
        /// <summary>
        /// Indicates no errors were found.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that the game executable file (<see cref="IBattleNetSettings.GameFile3">the GameExe
        /// property</see>) was not specified or did not exist.
        /// </summary>
        GameExeMissingOrNotFound = 1,
        /// <summary>
        /// Indicates that the second game file (<see cref="IBattleNetSettings.GameFile3">the GameFile2
        /// property</see>) was not specified or did not exist.
        /// </summary>
        GameFile2MissingOrNotFound = 2 ,
        /// <summary>
        /// Indicates that the third game file (<see cref="IBattleNetSettings.GameFile3">the GameFile3
        /// property</see>) was not specified or did not exist.
        /// </summary>
        GameFile3MissingOrNotFound = 4,
        /// <summary>
        /// Indicates that the username (<see cref="IBattleNetSettings.Username">the Username
        /// property</see>) was null or empty.
        /// </summary>
        UserNameNull = 8,
        /// <summary>
        /// Indicates that the emulated ping response (<see cref="IBattleNetSettings.PingMethod">the
        /// PingMethod property</see>) was not one of the known values of the <see>PingType</see> 
        /// enumeration.
        /// </summary>
        InvalidPingType = 16,
        /// <summary>
        /// Indicates that the client specified (<see cref="IBattleNetSettings.Client">the Client
        /// property</see>) was not valid for emulation; the only valid values are presently
        /// <c>STAR</c>, <c>SEXP</c>, <c>D2DV</c>, <c>D2XP</c>, <c>W2BN</c>, <c>WAR3</c>, and <c>W3XP</c>.
        /// </summary>
        InvalidEmulationClient = 32,
        /// <summary>
        /// Indicates that the primary CD key (<see cref="IBattleNetSettings.CdKey1">the CdKey1
        /// property</see>) was not specified or was invalid.
        /// </summary>
        PrimaryCdKeyMissingOrInvalid = 64,
        /// <summary>
        /// Indicates that the secondary CD key (<see cref="IBattleNetSettings.CdKey2">the CdKey2 
        /// property</see>) was not specified or was invalid, but was required for the selected client.
        /// </summary>
        SecondaryCdKeyMissingOrInvalid = 128,
        /// <summary>
        /// Indicates that the lockdown file (<see cref="IBattleNetSettings.ImageFile">the ImageFile 
        /// property</see> was not specified or did not exist.
        /// </summary>
        LockdownFileMissingOrNotFound = 256,
        /// <summary>
        /// Indicates that the gateway's server name (<see cref="IBattleNetSettings.Gateway">the 
        /// Gateway property</see>, then the <see cref="BNSharp.BattleNet.Gateway.ServerHost">ServerHost
        /// property</see>) was null or empty.
        /// </summary>
        InvalidGatewayServer = 512,
    }
}
