using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp
{
    /// <summary>
    /// Specifies the reasons for which an account may fail to be created.
    /// </summary>
    public enum CreationFailureReason
    {
        /// <summary>
        /// Specifies that an unknown cause was responsible for the account creation failure.
        /// </summary>
        Unknown, 
        /// <summary>
        /// Specifies that invalid characters (such as those in illies) was used.
        /// </summary>
        InvalidCharacters,
        /// <summary>
        /// Specifies that an invalid or banned word, such as a curse word, were in the name.
        /// </summary>
        InvalidWord,
        /// <summary>
        /// Specifies that the account name already exists.
        /// </summary>
        AccountAlreadyExists,
        /// <summary>
        /// Specifies that not enough alphanumeric characters were in the name.
        /// </summary>
        NotEnoughAlphanumerics,
        /// <summary>
        /// Specifies that the name was too short or blank.
        /// </summary>
        NameTooShort, 
        /// <summary>
        /// Specifies that too many punctuation characters were next to each other in the name.
        /// </summary>
        AdjacentPunctuation,
        /// <summary>
        /// Specifies that there were too many punctuation characters in the name.
        /// </summary>
        TooMuchPunctuation,
    }
}
