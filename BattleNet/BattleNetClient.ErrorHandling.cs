using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BNSharp.BattleNet
{
    partial class BattleNetClient
    {
        partial void ReportException(Exception ex, params KeyValuePair<string, object>[] notes)
        {
            //Exception ex, Delegate callee, object invokee, EventArgs arguments, Priority priority, string eventName
            /*
             *          new KeyValuePair<string, object>("delegate", eh),
                        new KeyValuePair<string, object>("Event", "ClanMemberListReceived"),
                        new KeyValuePair<string, object>("param: priority", p),
                        new KeyValuePair<string, object>("param: this", this),
                        new KeyValuePair<string, object>("param: e", e)
             */
            Delegate callee = null;
            object invokee = null;
            EventArgs args = null;
            Priority p = default(Priority);
            string name = null;

            foreach (KeyValuePair<string, object> kvp in notes)
            {
                switch (kvp.Key)
                {
                    case "delegate":
                        callee = kvp.Value as Delegate;
                        break;
                    case "Event":
                        name = kvp.Value as string;
                        break;
                    case "param: priority":
                        p = (Priority)kvp.Value;
                        break;
                    case "param: this":
                        invokee = kvp.Value;
                        break;
                    case "param: e":
                        args = kvp.Value as EventArgs;
                        break;
                }
            }

            EventExceptionEventArgs e3args = new EventExceptionEventArgs(ex, callee, invokee, args, p, name);
            OnEventExceptionThrown(e3args);
        }

        /// <summary>
        /// Informs listeners that a client-oriented event handler has thrown an exception.
        /// </summary>
        public event EventExceptionEventHandler EventExceptionThrown;
        /// <summary>
        /// Raises the <see>EventExceptionThrown</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnEventExceptionThrown(EventExceptionEventArgs e)
        {
            if (EventExceptionThrown != null)
                EventExceptionThrown(this, e);
        }
    }
}
