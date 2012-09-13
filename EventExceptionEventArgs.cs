using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Collections;

namespace BNSharp
{
    /// <summary>
    /// Contains information about when a BN# connection event handler callback raises an exception.
    /// </summary>
    /// <remarks>
    /// <para>Battle.net-related events driven by <see>BattleNetClient</see> are guaranteed to execute sequentially, in the order in which they
    /// were registered relative to other event handlers of their priority.</para>
    /// <example>
    /// <para>For example, consider a scenario in which there are three handlers registered for the <see cref="BNSharp.BattleNet.BattleNetClient.UserJoined">UserJoined event</see>; 
    /// one is registered at High <see>Priority</see> and two are registered at Low priority:</para>
    /// <list type="bullet">
    ///     <item>High: Moderation.ChannelModerationHandler.client_UserJoined(System.Object, BNSharp.UserEventArgs)</item>
    ///     <item>Low: Client.UI.ChatDisplay.client_UserJoined(System.Object, BNSharp.UserEventArgs)</item>
    ///     <item>Low: Client.UI.ChannelDisplay.client_UserJoined(System.Object, BNSharp.UserEventArgs)</item>
    /// </list>
    /// <para>Unlike normal .NET event handlers, every one of those handlers are guaranteed to be called back, regardless of whether a prior handler caused an 
    /// exception to be thrown.  However, if an exception is thrown, the <see cref="BNSharp.BattleNet.BattleNetClient.EventExceptionThrown">EventExceptionThrown event</see>
    /// will be raised on the client.  So, the sequence of external method calls that would happen in such an event, if the Moderation handler threw an exception, 
    /// would be something along these lines:</para>
    /// <list type="bullet">
    ///     <item>ChannelModerationHandler.client_UserJoined (raises exception)</item>
    ///     <item>BattleNetClient.EventExceptionThrown</item>
    ///     <item>ChatDisplay.client_UserJoined</item>
    ///     <item>ChannelDisplay.client_UserJoined</item>
    /// </list>
    /// </example>
    /// <para>Typically, all listeners of the EventExceptionThrown event are <b>not</b> guaranteed to be called and follow normal .NET event semantics.</para>
    /// </remarks>
    public class EventExceptionEventArgs : EventArgs
    {
        private Priority m_priority;
        private Exception m_ex;
        private Delegate m_del;
        private object m_sender;
        private string m_eventName;
        private EventArgs m_args;

        internal EventExceptionEventArgs(Exception ex, Delegate callee, object invokee, EventArgs arguments, Priority priority, string eventName)
        {
            m_ex = ex;
            m_del = callee;
            m_sender = invokee;
            m_args = arguments;
            m_priority = priority;
            m_eventName = eventName;
        }

        /// <summary>
        /// Gets the <see cref="BNSharp.Priority">Priority</see> at which the event was executing.
        /// </summary>
        public Priority Priority
        {
            get { return m_priority; }
        }

        /// <summary>
        /// Gets the <see cref="System.Exception">Exception</see> that is being reported.
        /// </summary>
        public Exception Exception
        {
            get { return m_ex; }
        }

        /// <summary>
        /// Gets the <see>Delegate</see> that was being invoked during the failure.
        /// </summary>
        public Delegate FaultingMethod
        {
            get { return m_del; }
        }

        /// <summary>
        /// Gets the name of the event that was being executed during the failure.
        /// </summary>
        public string EventName
        {
            get { return m_eventName; }
        }

        /// <summary>
        /// Gets the <c>sender</c> parameter of the method being invoked.
        /// </summary>
        /// <remarks>
        /// <para>This property typically returns the <see cref="BNSharp.BattleNet.BattleNetClient">BattleNetClient</see> that was executing the event.</para>
        /// </remarks>
        public object Sender
        {
            get { return m_sender; }
        }

        /// <summary>
        /// Gets the event arguments that were passed along to the method.
        /// </summary>
        public EventArgs Args
        {
            get { return m_args; }
        }

        /// <summary>
        /// Gets a string representation of this event data.
        /// </summary>
        /// <remarks>
        /// <para>The results of this method vary based on the type of build passed.  For builds with the DEBUG constant defined, 
        /// the result will be a verbose response.  For builds without it defined, it will limit itself to basic information.  For 
        /// more information, see <see cref="ToString(string)">ToString(string)</see>.</para>
        /// </remarks>
        /// <returns>A string representation of this event data.</returns>
        public override string ToString()
        {
#if DEBUG
            return ToString("v");
#else
            return ToString("b");
#endif
        }

        /// <summary>
        /// Gets a string representation of this event data.
        /// </summary>
        /// <param name="type">The type of representation to return.  <c>"v"</c> results in verbose information; <c>"b"</c> results in basic information.</param>
        /// <returns>
        /// A string representation of this event data.
        /// </returns>
        public string ToString(string type)
        {
            string result;
            switch (type)
            {
                case "v":
                    result = ToStringVerbose();
                    break;
                case "b":
                default:
                    result = ToStringBasic();
                    break;
            }
            return result;
        }

        private string ToStringBasic()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Event Exception Details: Faulting event '{0}' at Priority '{1}'", m_eventName, m_priority);
            sb.AppendLine();
            sb.AppendFormat("Target method name: {0}::{1}", m_del.Method.DeclaringType.FullName, m_del.Method.Name);
            sb.AppendLine();
            sb.AppendFormat("Target method assembly: {0}", m_del.Method.DeclaringType.Assembly.FullName);
            sb.AppendLine();
            sb.AppendLine(m_ex.Message);

            return sb.ToString();
        }

        private string ToStringVerbose()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Event Exception Details: Faulting event '{0}' at Priority '{1}'", m_eventName, m_priority);
            sb.AppendLine();
            sb.AppendFormat("Target method name: {0}::{1}", m_del.Method.DeclaringType.FullName, m_del.Method.Name);
            sb.AppendLine();
            sb.AppendFormat("Target method assembly: {0}", m_del.Method.DeclaringType.Assembly.FullName);
            sb.AppendLine();

            if (m_args == null)
            {
                sb.AppendLine("Invocation argument 'e' was -nil-");
            }
            else
            {
                Type argumentsType = m_args.GetType();
                sb.AppendFormat("Argument 'e' was of type {0}", argumentsType.FullName);
                sb.AppendLine();
                DumpObjectToBuilder(4, m_args, argumentsType, sb);
            }

            sb.AppendLine(m_ex.ToString());


            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <para>Assumes all parameters are not null.</para>
        /// </remarks>
        /// <param name="indent"></param>
        /// <param name="obj"></param>
        /// <param name="t"></param>
        /// <param name="sb"></param>
        private static void DumpObjectToBuilder(int indent, object obj, Type t, StringBuilder sb)
        {
            StringBuilder ident = new StringBuilder(indent);
            for (int i = 0; i < indent; i++)
                ident.Append(' ');
            string textIndent = ident.ToString();

            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanRead)
                {
                    if (IsIndexedProperty(pi))
                    {
                        sb.AppendFormat("{0}(Cannot display indexed property '{1}'", textIndent, pi.Name);
                        sb.AppendLine();
                    }
                    else
                    {
                        FormatProperty(indent, obj, sb, textIndent, pi);
                    }
                }
                else
                {
                    sb.AppendFormat("{0}(Cannot read property '{1}'", textIndent, pi.Name);
                    sb.AppendLine();
                }
                //sb.AppendLine();
            }
        }

        private static void FormatProperty(int indent, object obj, StringBuilder sb, string textIndent, PropertyInfo pi)
        {
            if (IsFormattableType(pi.PropertyType))
            {
                sb.AppendFormat("{0}{1}: {2}", textIndent, pi.Name, pi.GetValue(obj, null));
                sb.AppendLine();
            }
            else if (IsArrayOrCollection(pi.PropertyType))
            {
                ICollection col = pi.GetValue(obj, null) as ICollection;
                int current = 0;
                if (col != null)
                {
                    sb.AppendFormat("{0}{1} is of type '{2}':", textIndent, pi.Name, col.GetType().FullName);
                    sb.AppendLine();
                    foreach (object o in col)
                    {
                        DumpKvpToBuilder(current++, o, indent + 2, sb);
                    }
                }
                else
                {
                    sb.AppendFormat("{0}{1} is of type '{2}' (-nil-).", textIndent, pi.Name, pi.PropertyType.FullName);
                }
            }
            else if (IsDictionary(pi.PropertyType))
            {
                IDictionary dict = pi.GetValue(obj, null) as IDictionary;
                foreach (object key in dict.Keys)
                {
                    DumpKvpToBuilder(key, dict[key], indent + 2, sb);
                }
            }
            else
            {
                object val = pi.GetValue(obj, null);
                if (val == null)
                {
                    sb.AppendFormat("{0}{1} is of type '{2}' (-nil-)", textIndent, pi.Name, pi.PropertyType.FullName);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendFormat("{0}{1} is of type '{2}':", textIndent, pi.Name, val.GetType().FullName);
                    sb.AppendLine();
                    DumpObjectToBuilder(indent + 4, pi.GetValue(obj, null), val.GetType(), sb);
                }
            }
        }

        private static void DumpKvpToBuilder(object key, object value, int indent, StringBuilder sb)
        {
            StringBuilder ident = new StringBuilder(indent);
            for (int i = 0; i < indent; i++)
                ident.Append(' ');
            string textIndent = ident.ToString();

            if (value == null)
            {
                if (key == null)
                {
                    sb.AppendFormat("{0}[-nil-]: -nil-", textIndent);
                }
                else
                {
                    sb.AppendFormat("{0}[{1}]: -nil-", textIndent, key);
                }
            }
            else
            {
                if (key == null)
                {
                    if (IsFormattableType(value.GetType()))
                    {
                        sb.AppendFormat("{0}[-nil-]: {1}", textIndent, value);
                    }
                    else
                    {
                        sb.AppendFormat("{0}[-nil-] is of type '{1}':", textIndent, value.GetType().FullName);
                        sb.AppendLine();
                        DumpObjectToBuilder(indent + 8, value, value.GetType(), sb);
                    }
                }
                else
                {
                    if (IsFormattableType(value.GetType()))
                    {
                        sb.AppendFormat("{0}[{1}]: {2}", textIndent, key, value);
                    }
                    else
                    {
                        sb.AppendFormat("{0}[{1}] is of type '{2}':", textIndent, key, value.GetType().FullName);
                        sb.AppendLine();
                        DumpObjectToBuilder(indent + 2 + key.ToString().Length, value, value.GetType(), sb);
                    }
                }
            }
        }

        private static bool IsDictionary(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type);
        }

        private static bool IsArrayOrCollection(Type type)
        {
            return typeof(ICollection).IsAssignableFrom(type);
        }

        private static bool IsIndexedProperty(PropertyInfo pi)
        {
            MethodInfo mi = pi.GetGetMethod(false);
            return mi.GetParameters().Length > 0;
        }

        private static bool IsFormattableType(Type type)
        {
            return typeof(IFormattable).IsAssignableFrom(type) || type == typeof(string);
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for event exception events.
    /// </summary>
    /// <param name="sender">The object that originated the event (typically a <see cref="BNSharp.BattleNet.BattleNetClient">BattleNetClient</see>).</param>
    /// <param name="e">The event arguments.</param>
    public delegate void EventExceptionEventHandler(object sender, EventExceptionEventArgs e);
}
