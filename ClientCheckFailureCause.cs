using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp
{
    /// <summary>
    /// Specifies the causes of client versioning failure reported by Battle.net.
    /// </summary>
    [DataContract]
    public enum ClientCheckFailureCause
    {
        /// <summary>
        /// Indicates that client checks passed.
        /// </summary>
        [EnumMember]
        Passed = 0,
        /// <summary>
        /// Indicates that the client should upgrade.
        /// </summary>
        [EnumMember]
        OldVersion = 0x100,
        /// <summary>
        /// Indicates that the version checksum was invalid.
        /// </summary>
        [EnumMember]
        InvalidVersion = 0x101,
        /// <summary>
        /// Indicates that the client is using a newer version than is currently supported.
        /// </summary>
        [EnumMember]
        NewerVersion = 0x102,
        /// <summary>
        /// The CD key was invalid.
        /// </summary>
        [EnumMember]
        InvalidCdKey = 0x200,
        /// <summary>
        /// The CD key is already in use.
        /// </summary>
        [EnumMember]
        CdKeyInUse = 0x201,
        /// <summary>
        /// The CD key has been banned.
        /// </summary>
        [EnumMember]
        BannedCdKey = 0x202,
        /// <summary>
        /// The CD key was for the wrong product.
        /// </summary>
        [EnumMember]
        WrongProduct = 0x203,
        /// <summary>
        /// Indicates that the expansion CD key was invalid.
        /// </summary>
        [EnumMember]
        InvalidExpCdKey = 0x210,
        /// <summary>
        /// Indicates that the expansion CD key is already in use.
        /// </summary>
        [EnumMember]
        ExpCdKeyInUse = 0x211,
        /// <summary>
        /// Indicates that the expansion CD key was banned.
        /// </summary>
        [EnumMember]
        BannedExpCdKey = 0x212,
        /// <summary>
        /// Indicates that the expansion CD key was for the wrong product.
        /// </summary>
        [EnumMember]
        WrongExpProduct = 0x213,
    }
}
