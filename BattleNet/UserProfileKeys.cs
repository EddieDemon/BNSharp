using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Represents a user profile request key.  This class cannot be inherited.
    /// </summary>
    //[DataContract]
    public sealed class UserProfileKey
    {
        #region predefined strings
        private const string SEX = @"profile\sex";
        private const string AGE = @"profile\age";
        private const string LOCATION = @"profile\location";
        private const string DESCRIPTION = @"profile\description";
        private const string ACCOUNTCREATED = @"System\Account Created";
        private const string LASTLOGON = @"System\Last Logon";
        private const string LASTLOGOFF = @"System\Last Logoff";
        private const string TIMELOGGED = @"System\Time Logged";
        private const string NORMAL_WINS = @"record\{0}\0\wins";
        private const string NORMAL_LOSSES = @"record\{0}\0\losses";
        private const string NORMAL_DISCONNECTS = @"record\{0}\0\disconnects";
        private const string NORMAL_LAST_GAME = @"record\{0}\0\last {0}";
        private const string NORMAL_LAST_RESULT = @"record\{0}\0\last {0} result";
        private const string LADDER_WINS = @"record\{0}\1\wins";
        private const string LADDER_LOSSES = @"record\{0}\1\losses";
        private const string LADDER_DISCONNECTS = @"record\{0}\1\disconnects";
        private const string LADDER_LAST_GAME = @"record\{0}\1\last {0}";
        private const string LADDER_LAST_RESULT = @"record\{0}\1\last {0} result";
        private const string LADDER_RATING = @"record\{0}\1\rating";
        private const string LADDER_HIGH_RATING = @"record\{0}\1\high rating";
        private const string LADDER_RANK = @"DynKey\{0}\1\rank";
        private const string LADDER_HIGH_RANK = @"record\{0}\1\high rank";
        private const string IRONMAN_WINS = @"record\{0}\3\wins";
        private const string IRONMAN_LOSSES = @"record\{0}\3\losses";
        private const string IRONMAN_DISCONNECTS = @"record\{0}\3\disconnects";
        private const string IRONMAN_LAST_GAME = @"record\{0}\3\last {0}";
        private const string IRONMAN_LAST_RESULT = @"record\{0}\3\last {0} result";
        private const string IRONMAN_RATING = @"record\{0}\3\rating";
        private const string IRONMAN_HIGH_RATING = @"record\{0}\3\high rating";
        private const string IRONMAN_RANK = @"DynKey\{0}\3\rank";
        private const string IRONMAN_HIGH_RANK = @"record\{0}\3\high rank";
        #endregion

        #region fields
        [DataMember(Name = "Key")]
        private string m_keyValue;
        [DataMember(Name = "Writeable")]
        private bool m_writeable;
        #endregion
        #region constructors
        private UserProfileKey(string request)
        {
            Debug.Assert(!string.IsNullOrEmpty(request));

            m_keyValue = request;
        }
        private UserProfileKey(string request, bool writeable)
            : this(request)
        {
            m_writeable = writeable;
        }
        #endregion

        #region settable fields
        /// <summary>
        /// Gets the profile key corresponding to the user's Sex entry.
        /// </summary>
        /// <remarks>
        /// <para>This field is defunct in Starcraft and Warcraft III.</para>
        /// </remarks>
        public static UserProfileKey Sex { get { return new UserProfileKey(SEX); } }
        /// <summary>
        /// Gets the profile key corresponding to the user's Age entry.
        /// </summary>
        /// <remarks>
        /// <para>This field is defunct on all Battle.net clients.</para>
        /// </remarks>
        [Obsolete("The Age profile key is defunct on all Battle.net clients.", false)]
        public static UserProfileKey Age { get { return new UserProfileKey(AGE); } }
        /// <summary>
        /// Gets the profile key corresponding to the user's Location entry.
        /// </summary>
        /// <remarks>
        /// <para>This key may be used when writing the user profile.</para>
        /// </remarks>
        public static UserProfileKey Location { get { return new UserProfileKey(LOCATION, true); } }
        /// <summary>
        /// Gets the profile key corresponding to the user's Description entry.
        /// </summary>
        /// <remarks>
        /// <para>This key may be used when writing the user profile.</para>
        /// </remarks>
        public static UserProfileKey Description { get { return new UserProfileKey(DESCRIPTION, true); } }
        #endregion

        #region time fields
        /// <summary>
        /// Gets the profile key corresponding to the time of the last logon.
        /// </summary>
        public static UserProfileKey LastLogon
        {
            get { return new UserProfileKey(LASTLOGON); }
        }

        /// <summary>
        /// Gets the profile key corresponding to the time of the last logoff.
        /// </summary>
        public static UserProfileKey LastLogoff
        {
            get { return new UserProfileKey(LASTLOGOFF); }
        }

        /// <summary>
        /// Gets the profile key corresponding to the total time logged in.
        /// </summary>
        public static UserProfileKey TotalTimeLogged
        {
            get { return new UserProfileKey(TIMELOGGED); }
        }

        /// <summary>
        /// Gets the profile key corresponding to the account creation time.
        /// </summary>
        public static UserProfileKey AccountCreated
        {
            get { return new UserProfileKey(ACCOUNTCREATED); }
        }
        #endregion

        #region statistics fields
        /// <summary>
        /// Gets a user profile key for the specified type of statistic.
        /// </summary>
        /// <param name="profileType">The type of statistic (ladder, non-ladder).</param>
        /// <param name="recordType">The type of record (wins, losses).</param>
        /// <param name="client">The client for which to request.</param>
        /// <returns>A non-writeable UserProfileKey.</returns>
        /// <exception cref="InvalidEnumArgumentException">Thrown if <paramref name="profileType"/> or <paramref name="recordType"/>
        /// are values that are not defined by their enumerations.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the product specified by <paramref name="client"/> was invalid, or if the 
        /// type of profile or record was not appropriate for the client (for example, IronMan Ladder is only valid for Warcraft II: 
        /// Battle.net Edition).</exception>
        public static UserProfileKey GetRecordKey(ProfileRecordKeyType profileType, RecordKeyType recordType, Product client)
        {
            if (profileType == ProfileRecordKeyType.IronManLadder && client != Product.Warcraft2BNE)
                throw new InvalidEnumArgumentException("profileType", (int)profileType, typeof(ProfileRecordKeyType));

            switch (profileType)
            {
                case ProfileRecordKeyType.Normal:
                    return GetRecordKey(recordType, client);
                case ProfileRecordKeyType.Ladder:
                    return GetLadderRecordKey(recordType, client);
                case ProfileRecordKeyType.IronManLadder:
                    return GetIronManLadderRecordKey(recordType, client);
                default:
                    throw new InvalidEnumArgumentException("profileType", (int)profileType, typeof(ProfileRecordKeyType));
            }
        }

        private static UserProfileKey GetIronManLadderRecordKey(RecordKeyType recordType, Product client)
        {
            if (!Enum.IsDefined(typeof(RecordKeyType), recordType))
                throw new InvalidEnumArgumentException("recordType", (int)recordType, typeof(RecordKeyType));

            string key = null;
            switch (recordType)
            {
                case RecordKeyType.Wins:
                    key = IRONMAN_WINS;
                    break;
                case RecordKeyType.Losses:
                    key = IRONMAN_LOSSES;
                    break;
                case RecordKeyType.Disconnects:
                    key = IRONMAN_DISCONNECTS;
                    break;
                case RecordKeyType.LastGame:
                    key = IRONMAN_LAST_GAME;
                    break;
                case RecordKeyType.LastGameResult:
                    key = IRONMAN_LAST_RESULT;
                    break;
                case RecordKeyType.Rating:
                    key = IRONMAN_RATING;
                    break;
                case RecordKeyType.HighRating:
                    key = IRONMAN_HIGH_RATING;
                    break;
                case RecordKeyType.Rank:
                    key = IRONMAN_RANK;
                    break;
                case RecordKeyType.HighRank:
                    key = IRONMAN_HIGH_RANK;
                    break;
            }
            key = string.Format(CultureInfo.InvariantCulture, key, client.ProductCode);
            return new UserProfileKey(key);
        }

        private static UserProfileKey GetRecordKey(RecordKeyType recordType, Product client)
        {
            if (!Enum.IsDefined(typeof(RecordKeyType), recordType))
                throw new InvalidEnumArgumentException("recordType", (int)recordType, typeof(RecordKeyType));

            if (recordType >= RecordKeyType.Rating)
                throw new ArgumentOutOfRangeException("recordType", "Non-ladder keys may not be requested for the Normal profile record types.");

            string key = null;
            switch (recordType)
            {
                case RecordKeyType.Wins:
                    key = NORMAL_WINS;
                    break;
                case RecordKeyType.Losses:
                    key = NORMAL_LOSSES;
                    break;
                case RecordKeyType.Disconnects:
                    key = NORMAL_DISCONNECTS;
                    break;
                case RecordKeyType.LastGame:
                    key = NORMAL_LAST_GAME;
                    break;
                case RecordKeyType.LastGameResult:
                    key = NORMAL_LAST_RESULT;
                    break;
            }
            key = string.Format(CultureInfo.InvariantCulture, key, client.ProductCode);
            return new UserProfileKey(key);
        }

        private static UserProfileKey GetLadderRecordKey(RecordKeyType recordType, Product client)
        {
            if (!Enum.IsDefined(typeof(RecordKeyType), recordType))
                throw new InvalidEnumArgumentException("recordType", (int)recordType, typeof(RecordKeyType));

            string key = null;
            switch (recordType)
            {
                case RecordKeyType.Wins:
                    key = LADDER_WINS;
                    break;
                case RecordKeyType.Losses:
                    key = LADDER_LOSSES;
                    break;
                case RecordKeyType.Disconnects:
                    key = LADDER_DISCONNECTS;
                    break;
                case RecordKeyType.LastGame:
                    key = LADDER_LAST_GAME;
                    break;
                case RecordKeyType.LastGameResult:
                    key = LADDER_LAST_RESULT;
                    break;
                case RecordKeyType.Rating:
                    key = LADDER_RATING;
                    break;
                case RecordKeyType.HighRating:
                    key = LADDER_HIGH_RATING;
                    break;
                case RecordKeyType.Rank:
                    key = LADDER_RANK;
                    break;
                case RecordKeyType.HighRank:
                    key = LADDER_HIGH_RANK;
                    break;
            }
            key = string.Format(CultureInfo.InvariantCulture, key, client.ProductCode);
            return new UserProfileKey(key);
        }
        #endregion

        /// <summary>
        /// Gets whether this key may be used when writing a profile update.
        /// </summary>
        public bool IsWriteable
        {
            get { return m_writeable; }
        }

        internal string Key
        {
            get { return m_keyValue; }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return m_keyValue;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return m_keyValue.GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            UserProfileKey key = obj as UserProfileKey;
            if (!object.ReferenceEquals(key, null))
            {
                return key.Key.Equals(m_keyValue, StringComparison.Ordinal);
            }

            return false;
        }
    }
}
