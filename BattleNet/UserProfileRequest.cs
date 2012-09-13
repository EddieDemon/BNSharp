using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Specifies a key-value-based user profile.  This class cannot be inherited.
    /// </summary>
    //[DataContract]
    public sealed class UserProfileRequest : IEnumerable<UserProfileKey>
    {
        [DataMember(Name = "ValuePairs")]
        private Dictionary<UserProfileKey, string> m_profileValues;
        [DataMember(Name = "UserName")]
        private string m_un;

        /// <summary>
        /// Creates a new empty <see>UserProfileRequest</see>.
        /// </summary>
        /// <param name="userName">The name of the user being requested.</param>
        public UserProfileRequest(string userName)
        {
            m_un = userName;
            m_profileValues = new Dictionary<UserProfileKey, string>();
        }

        /// <summary>
        /// Creates a new <see>UserProfileRequest</see> with a number of keys.
        /// </summary>
        /// <param name="userName">The name of the user being requested.</param>
        /// <param name="keys">The keys to initially add.</param>
        public UserProfileRequest(string userName, params UserProfileKey[] keys)
            : this(userName)
        {
            AddRange(keys);
        }

        /// <summary>
        /// Adds a key to a profile request.
        /// </summary>
        /// <param name="key">The key to retrieve.</param>
        public void Add(UserProfileKey key)
        {
            if (!m_profileValues.ContainsKey(key))
                m_profileValues.Add(key, null);
        }

        /// <summary>
        /// Adds a list of keys to a profile request.
        /// </summary>
        /// <param name="keys">The keys to retrieve.</param>
        public void AddRange(UserProfileKey[] keys) 
        {
            foreach (UserProfileKey key in keys)
            {
                if (!m_profileValues.ContainsKey(key))
                    m_profileValues.Add(key, null);
            }
        }

        internal IEnumerable<UserProfileKey> Keys
        {
            get
            {
                List<UserProfileKey> keys = new List<UserProfileKey>(m_profileValues.Keys);
                return keys;
            }
        }

        /// <summary>
        /// Gets the number of keys in this request.
        /// </summary>
        public int Count
        {
            get { return m_profileValues.Count; }
        }

        /// <summary>
        /// Gets the name of the user whose profile this represents.
        /// </summary>
        public string UserName
        {
            get { return m_un; }
        }

        /// <summary>
        /// Gets the value returned from Battle.net for the specified key.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <returns>The value returned from Battle.net.</returns>
        /// <remarks>
        /// <para>You can obtain the keys by creating and maintaining references to them, or by treating this object as enumerable
        /// (which means it can be used in a <see langword="foreach" /> loop).</para>
        /// </remarks>
        public string this[UserProfileKey key]
        {
            get { return m_profileValues[key]; }
            internal set { m_profileValues[key] = value; } 
        }

        #region IEnumerable<UserProfileKey> Members

        /// <inheritdoc />
        public IEnumerator<UserProfileKey> GetEnumerator()
        {
            return m_profileValues.Keys.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
