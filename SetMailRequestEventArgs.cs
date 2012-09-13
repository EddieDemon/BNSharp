using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp /* NOTE: SetMailRequest event arguments*/
{
    /// <summary>
    /// Contains information about a to-set e-mail address.
    /// </summary>
    [DataContract]
    public class SetMailRequestEventArgs : BaseEventArgs
    {
        public SetMailRequestEventArgs()
        {
        }
    }
    /// <summary>
    /// Specifies the contract for handlers wishing to listen for set mail events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void SetMailRequestEventHandler(object sender, SetMailRequestEventArgs e);
}
