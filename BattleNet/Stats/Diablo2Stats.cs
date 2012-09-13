using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BNSharp.MBNCSUtil;
using System.IO;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Represents statistics logged onto a Diablo II Realm character.
    /// </summary>
    /// <remarks>
    /// <para>This class cannot be instantiated directly.  To obtain an instance of this class, use 
    /// <see cref="UserStats.Parse">UserStats.Parse</see>, and cast the result to this class.</para>
    /// <para>This class is only meaningful if the user is logged on as a realm character.  To determine 
    /// whether the user is logged on with a Realm character, check the <see>IsRealmCharacter</see> property.</para>
    /// </remarks>
    [DataContract]
    public class Diablo2Stats : UserStats
    {
        #region fields
        private static readonly Regex RealmCharacterTest = new Regex(@"(?<Character>\S+@\w+)\*(?<Username>\S+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        [DataMember(Name = "IsRealmCharacter")]
        private bool m_isRealm;
        [DataMember(Name = "Product")]
        private Product m_prod;
        [DataMember(Name = "LiteralText")]
        private string m_literal;

        [DataMember(Name = "CharacterName")]
        private string m_charName;
        [DataMember(Name = "Realm")]
        private string m_realm;
        [DataMember(Name = "AccountName")]
        private string m_userName;
        [DataMember(Name = "IsExpansionCharacter")]
        private bool m_isExpCharacter;
        [DataMember(Name = "IsHardcoreCharacter")]
        private bool m_isHardcore;
        [DataMember(Name = "IsDead")]
        private bool m_isDead;
        [DataMember(Name = "IsLadder")]
        private bool m_isLadder;
        [DataMember(Name = "IsMale")]
        private bool m_isMale;
        [DataMember(Name = "HasCompletedGame")]
        private bool m_hasCompletedGame;
        [DataMember(Name = "Difficulty")]
        private Diablo2DifficultyLevel m_difficulty;
        [DataMember(Name = "Class")]
        private Diablo2CharacterClass m_class;
        [DataMember(Name = "Level")]
        private int m_level;
        [DataMember(Name = "ActsCompleted")]
        private int m_numActsCompleted;
        #endregion

        #region constructor
        internal Diablo2Stats(string userName, byte[] statstring)
        {
            m_literal = Encoding.ASCII.GetString(statstring);

            DataReader dr = new DataReader(statstring);
            string productCode = dr.ReadDwordString(0);
            m_prod = Product.GetByProductCode(productCode);
            if (m_prod == null)
                m_prod = Product.UnknownProduct;

            Match m = RealmCharacterTest.Match(userName);
            if (m.Success)
            {
                m_userName = string.Concat("*", m.Groups["Username"].Value);
                try
                {
                    m_isRealm = true;
                    m_realm = dr.ReadTerminatedString(',', Encoding.ASCII);
                    m_charName = dr.ReadTerminatedString(',', Encoding.ASCII);

                    byte[] characterData = dr.ReadByteArray(33);
                    /*
    0000   ff 0f 74 00 01 00 00 00 00 00 00 00 8c 00 00 00  ..t.............
    0010   00 00 00 00 0d f0 ad ba 0d f0 ad ba 53 63 72 65  ............Scre
    0020   65 6e 53 68 6f 6f 74 40 55 53 45 61 73 74 2a 44  enShoot@USEast*D
    0030   72 2e 4d 61 72 73 68 61 6c 6c 00 50 58 32 44 55  r.Marshall.PX2DU
    0040   53 45 61 73 74 2c 53 63 72 65 65 6e 53 68 6f 6f  SEast,ScreenShoo
    0050   74 2c>84 80 39 ff ff ff ff 0f ff 5d ff ff ff*04  t,..9......]....
    0060   4d ff ff ff ff ff ff ff ff ff ff 56*a8*9a ff ff  M..........V....
    0070   ff ff ff<00                                      ....
                     * */
                    m_class = (Diablo2CharacterClass)characterData[13];
                    if (m_class < Diablo2CharacterClass.Amazon || m_class > Diablo2CharacterClass.Assassin)
                        m_class = Diablo2CharacterClass.Unknown;

                    m_isMale = !(m_class == Diablo2CharacterClass.Amazon || m_class == Diablo2CharacterClass.Assassin ||
                                    m_class == Diablo2CharacterClass.Sorceress);

                    m_level = characterData[25];

                    byte flags = characterData[26];
                    m_isHardcore = ((flags & 4) == 4);
                    m_isDead = ((flags & 8) == 8);
                    m_isExpCharacter = ((flags & 32) == 32);
                    m_isLadder = ((flags & 64) == 64);

                    byte completedActs = (byte)((characterData[27] & 0x3e) >> 2);
                    if (m_isExpCharacter)
                    {
                        m_difficulty = (Diablo2DifficultyLevel)(completedActs / 5);
                        m_numActsCompleted = (completedActs % 5);
                        m_hasCompletedGame = (m_numActsCompleted == 5);
                    }
                    else
                    {
                        m_difficulty = (Diablo2DifficultyLevel)(completedActs / 4);
                        m_numActsCompleted = (completedActs % 4);
                        m_hasCompletedGame = (m_numActsCompleted == 4);
                    }
                }
                catch (ArgumentOutOfRangeException) { }
            }
            else
            {
                m_userName = userName;
            }
        }
        #endregion

        #region properties

        /// <summary>
        /// Gets the name of the user's account.  On Diablo II, this will always be prefixed with the asterisk (*), as this is the account
        /// name to which whispering may be done.
        /// </summary>
        public string UserName
        {
            get { return m_userName; }
        }

        /// <summary>
        /// Gets the number of acts the character has completed on the current difficulty level.
        /// </summary>
        public int ActsCompleted
        {
            get { return m_numActsCompleted; }
        }

        /// <summary>
        /// Gets the user's level.
        /// </summary>
        public int Level
        {
            get { return m_level; }
        }

        /// <summary>
        /// Gets the character class for the user's character.
        /// </summary>
        public Diablo2CharacterClass CharacterClass
        {
            get { return m_class; }
        }

        /// <summary>
        /// Gets the most recent difficulty level for the character.
        /// </summary>
        public Diablo2DifficultyLevel Difficulty
        {
            get { return m_difficulty; }
        }

        /// <summary>
        /// Gets whether the character has completed the entire game.
        /// </summary>
        /// <remarks>
        /// <para>This property is a utility provided as a shortcut, since you can determine whether the character has 
        /// completed the entire game based on the number of <see>ActsCompleted</see>, <see>Difficulty</see>, and 
        /// whether the character <see cref="IsExpansionCharacter">is a Lord of Destruction character</see>.</para>
        /// </remarks>
        public bool HasCompletedGame
        {
            get { return m_hasCompletedGame; }
        }

        /// <summary>
        /// Gets whether the character is male.
        /// </summary>
        /// <remarks>
        /// <para>This property is a utility property provided as a shortcut, since you can determine the gender
        /// of the character based on the <see>CharacterClass</see> property.</para>
        /// </remarks>
        public bool IsMaleCharacter
        {
            get { return m_isMale; }
        }

        /// <summary>
        /// Gets whether the user's character is a ladder character.
        /// </summary>
        public bool IsLadderCharacter
        {
            get { return m_isLadder; }
        }

        /// <summary>
        /// Gets whether the user's character is currently dead.
        /// </summary>
        public bool IsCharacterDead
        {
            get { return m_isDead; }
        }

        /// <summary>
        /// Gets whether the user's character is hardcore.
        /// </summary>
        public bool IsHardcoreCharacter
        {
            get { return m_isHardcore; }
        }

        /// <summary>
        /// Gets whether the user's character was created with the Lord of Destruction expansion.
        /// </summary>
        public bool IsExpansionCharacter
        {
            get { return m_isExpCharacter; }
        }

        /// <summary>
        /// Gets the user's Realm.
        /// </summary>
        public string Realm
        {
            get { return m_realm; }
        }

        /// <summary>
        /// Gets the user's current character name.
        /// </summary>
        public string CharacterName
        {
            get { return m_charName; }
        }

        /// <summary>
        /// Gets whether the user is logged on with a Realm character.
        /// </summary>
        public bool IsRealmCharacter
        {
            get { return m_isRealm; }
        }

        /// <summary>
        /// Gets the product currently in use by the user.
        /// </summary>
        public override Product Product
        {
            get { return m_prod; }
        }

        /// <summary>
        /// Gets the literal text of the statstring.
        /// </summary>
        public override string LiteralText
        {
            get { return m_literal; }
        }
        #endregion
    }
}
