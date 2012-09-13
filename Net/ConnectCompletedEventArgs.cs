using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp.Net
{
    /// <summary>
    /// Represents the result of a connection attempt made asynchronously.
    /// </summary>
    [Serializable]
    public class ConnectCompletedResult
    {
        internal ConnectCompletedResult() { }

        /// <summary>
        /// Creates a new <see>ConnectCompletedResult</see>.
        /// </summary>
        public ConnectCompletedResult(object state, bool succeeded)
        {
            State = state;
            Succeeded = succeeded;
        }

        /// <summary>
        /// Gets user-provided state associated with the connection attempt.
        /// </summary>
        /// <remarks>
        /// <para>This property returns the object provided via the <c>state</c> parameter of <see>ConnectionBase.ConnectAsync</see> method.</para>
        /// </remarks>
        public object State
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets whether the connection succeeded.
        /// </summary>
        public bool Succeeded
        {
            get;
            internal set;
        }
    }

    /// <summary>
    /// Specifies the signature of methods that should be implemented to handle an asynchronous connect method call.
    /// </summary>
    /// <param name="result">The result of the connection attempt.</param>
    public delegate void ConnectCompletedCallback(ConnectCompletedResult result);
}
