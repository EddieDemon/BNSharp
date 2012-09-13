using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using BNSharp.Plugins;
using System.Net;

namespace BNSharp.Net
{
    /// <summary>
    /// Enables proxy support to a connection by redirecting a normal <see>ConnectionBase</see> to a plugin (<see>IProxyConnector</see>), 
    /// which then can manipulate the underlying data connection as needed by the protocol.
    /// </summary>
    public class ThinProxiedConnectionBase : ConnectionBase, IProxiedRealConnection
    {
        private IProxyConnector m_proxy;

        [System.Diagnostics.DebuggerDisplay("(pass-through connector)")]
        private class NoProxyProxyConnector : IProxyConnector
        {
            private IProxiedRealConnection m_con;

            #region IProxyConnector Members
            public void Initialize(IProxiedRealConnection client)
            {
                m_con = client;
            }

            public bool Negotiate()
            {
                return true;
            }

            public IPEndPoint ResolveEndPoint(string host, int port)
            {
                return m_con.ResolveEndPoint(host, port);
            }

            public byte[] Receive(byte[] buffer, int index, int length)
            {
                return m_con.Receive(buffer, index, length);
            }

            public byte[] Receive()
            {
                return m_con.Receive();
            }

            public void Send(byte[] data, int index, int length)
            {
                m_con.Send(data, index, length);
            }

            #endregion
        }

        /// <summary>
        /// Creates a new instance of the <b>ConnectionBase</b> class.
        /// </summary>
        /// <param name="server">The URI of the server to connect to.</param>
        /// <param name="port">The port of the server to connect to.</param>
        public ThinProxiedConnectionBase(string server, int port)
            : base(server, port)
        {
            m_proxy = new NoProxyProxyConnector();
            m_proxy.Initialize(this);
        }

        /// <summary>
        /// Gets or sets an <see>IProxyConnector</see> that can be used to redirect this connection through a proxy.
        /// </summary>
        /// <remarks>
        /// <para>For more information on proxy support in BN#, please see the wiki article
        /// <a href="http://www.jinxbot.net/wiki/index.php?title=Proxy_support_in_BNSharp">Proxy support in BN#</a> or the 
        /// <see>IProxyConnector</see> interface.</para>
        /// <para>When this value is set to <see langword="null" />, a default proxy connector is instantiated.  Consequently this property
        /// will never return <see langword="null" />.</para>
        /// </remarks>
        public IProxyConnector ProxyConnector
        {
            get { return m_proxy; }
            set
            {
                if (IsConnected)
                    throw new InvalidOperationException("Cannot set a proxy connector once the connection has already been established.");

                if (value == null)
                    m_proxy = new NoProxyProxyConnector();
                else 
                    m_proxy = value;

                m_proxy.Initialize(this);
            }
        }

        /// <inheritdoc />
        public override bool Connect()
        {
            bool ok = base.Connect();
            if (ok)
                m_proxy.Negotiate();
            return ok;
        }

        /// <inheritdoc />
        protected override IPEndPoint ResolveEndpoint(string server, int port)
        {
            return m_proxy.ResolveEndPoint(server, port);
        }

        /// <inheritdoc />
        public override byte[] Receive(byte[] buffer, int index, int length)
        {
            return m_proxy.Receive(buffer, index, length);
        }

        /// <inheritdoc />
        protected override void Send(byte[] data, int index, int length)
        {
            m_proxy.Send(data, index, length);
        }

        /// <inheritdoc />
        public override byte[] Receive()
        {
            return m_proxy.Receive();
        }

        /// <inheritdoc />
        protected override bool AlwaysResolveRemoteHost
        {
            get
            {
                return !(m_proxy is NoProxyProxyConnector);
            }
        }

        #region IProxiedRealConnection Members

        byte[] IProxiedRealConnection.Receive(byte[] buffer, int index, int length)
        {
            return base.Receive(buffer, index, length);
        }

        byte[] IProxiedRealConnection.Receive()
        {
            return base.Receive();
        }

        void IProxiedRealConnection.Send(byte[] data, int index, int length)
        {
            base.Send(data, index, length);
        }

        IPEndPoint IProxiedRealConnection.ResolveEndPoint(string host, int port)
        {
            return base.ResolveEndpoint(host, port);
        }
        #endregion
    }
}
