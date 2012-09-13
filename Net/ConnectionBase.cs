using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Globalization;
using System.Threading;

namespace BNSharp.Net
{
    /// <summary>
    /// Represents a TCP/IP connection.
    /// </summary>
    public class ConnectionBase : MarshalByRefObject, IDisposable
    {
        private TcpClient m_client;
        private IPEndPoint m_ipep;
        private string m_server;
        private int m_port;

        /// <summary>
        /// Creates a new instance of the <b>ConnectionBase</b> class.
        /// </summary>
        /// <param name="server">The URI of the server to connect to.</param>
        /// <param name="port">The port of the server to connect to.</param>
        public ConnectionBase(string server, int port)
        {
            m_server = server;
            m_port = port;
        }

        /// <summary>
        /// Resolves an IP end point for the specified server and port.
        /// </summary>
        /// <param name="server">The DNS name or IP address (as a string) of the server to look up.</param>
        /// <param name="port">The port number to which to connect.</param>
        /// <returns>An <see>IPEndPoint</see> representing the server and port; or, if resolution failed, <see langword="null" />.</returns>
        protected virtual IPEndPoint ResolveEndpoint(string server, int port)
        {
            IPAddress[] addresses = Dns.GetHostEntry(server).AddressList;
            foreach (IPAddress addr in addresses)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                    return new IPEndPoint(addr, port);
            }
            return null;
        }

        /// <summary>
        /// Connects the connection to a remote host other than the one for which the connection was initialized.
        /// </summary>
        /// <param name="server">The URI of the server to which to connect.</param>
        /// <param name="port">The port of the server to which to connect.</param>
        /// <returns><see langword="true" /> if the connection completed successfully; otherwise <see langword="false" />.</returns>
        /// <remarks>
        /// <para>The <see cref="OnError">OnError</see> method is called when an error takes place, but no
        /// behavior is defined by default.  Inheriting classes should provide an implementation 
        /// to handle errors.</para>
        /// <para>This method may return false if the connection is already established.  To check whether the 
        /// connection is established, check the <see>IsConnected</see> property.</para>
        /// </remarks>
        public bool Connect(string server, int port)
        {
            m_server = server;
            m_port = port;

            return Connect();
        }

        /// <summary>
        /// Connects the connection to the remote host.
        /// </summary>
        /// <returns><b>True</b> if the connection completed successfully; otherwise <b>false</b>.</returns>
        /// <remarks>
        /// <para>The <see cref="OnError">OnError</see> method is called when an error takes place, but no
        /// behavior is defined by default.  Inheriting classes should provide an implementation 
        /// to handle errors.</para>
        /// <para>This method may return false if the connection is already established.  To check whether the 
        /// connection is established, check the <see>IsConnected</see> property.</para>
        /// </remarks>
        public virtual bool Connect()
        {
            // sanity check
            if (m_open)
                return false;

            if (m_ipep == null || AlwaysResolveRemoteHost)
            {
                try
                {
                    m_ipep = ResolveEndpoint(m_server, m_port);
                    if (m_ipep == null)
                        throw new SocketException();
                }
                catch (SocketException se)
                {
                    OnError(string.Format(CultureInfo.CurrentCulture, Strings.ConnectionBase_Connect_ResolveFailed_fmt,
                        m_server), se);
                    return false;
                }
            }
            try
            {
                m_open = true;
                m_client = new TcpClient();
                m_client.NoDelay = true;
                m_client.Connect(m_ipep);
            }
            catch (SocketException se)
            {
                m_open = false;
                OnError(string.Format(CultureInfo.CurrentCulture,
                    Strings.ConnectionBase_Connect_ConnectFailed_fmt,
                    m_server, m_port, m_ipep.Address.ToString()), se);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Begins a connection asynchronously.
        /// </summary>
        /// <param name="state">Any user object that is desired to be passed back to the connection completed callback.  This will be represented
        /// through the <see>ConnectCompletedResult.State</see> property.</param>
        /// <param name="callback">The method to call when the connection has completed.</param>
        public virtual void ConnectAsync(object state, ConnectCompletedCallback callback)
        {
            WaitCallback ts = delegate
                             {
                                 bool ok = Connect();
                                 ConnectCompletedResult result = new ConnectCompletedResult { State = state, Succeeded = ok };
                                 callback(result);
                             };
            ThreadPool.QueueUserWorkItem(ts);
        }

        /// <summary>
        /// When overridden by a derived class, provides error information from the current connection.
        /// </summary>
        /// <param name="message">Human-readable information about the error.</param>
        /// <param name="ex">An internal exception containing the error details.</param>
        protected virtual void OnError(string message, Exception ex)
        {

        }

        /// <summary>
        /// Gets whether the connection is alive or not.
        /// </summary>
        public bool IsConnected { get { return m_open; } }
        private bool m_open;

        /// <summary>
        /// Closes the connection and prepares for a new connection.
        /// </summary>
        public virtual void Close()
        {
            if (m_open)
            {
                m_open = false;
                m_client.Client.Disconnect(true);
            }
        }

        /// <summary>
        /// Once the connection is established, gets the local endpoint to which the client is bound.
        /// </summary>
        public IPEndPoint LocalEP
        {
            get
            {
                return (IPEndPoint)m_client.Client.LocalEndPoint;
            }
        }

        /// <summary>
        /// Once the connection is established, gets the remote endpoint to which the client is bound.
        /// </summary>
        public IPEndPoint RemoteEP
        {
            get
            {
                return (IPEndPoint)m_client.Client.RemoteEndPoint;
            }
        }

        /// <summary>
        /// Allows derived classes to always require the connection to re-resolve the remote host during the <see cref="Connect()"/> method.
        /// </summary>
        protected virtual bool AlwaysResolveRemoteHost
        {
            get { return false; }
        }

        /// <summary>
        /// For derived classes, sends the specified binary data to the server.
        /// </summary>
        /// <param name="data">The data to send.</param>
        protected virtual void Send(byte[] data)
        {
            Send(data, 0, data.Length);
        }

        /// <summary>
        /// For derived classes, sends part of the specified binary data to the server.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="index">The start index of the data.</param>
        /// <param name="length">The amount of data to send.</param>
        protected virtual void Send(byte[] data, int index, int length)
        {
            m_client.GetStream().Write(data, index, length);
        }

        /// <summary>
        /// Receives the specified number of bytes.
        /// </summary>
        /// <remarks>
        /// <para>This method blocks in a loop until all of the data comes through.  For that reason, it 
        /// is recommended that this method is only used in a background thread.</para>
        /// </remarks>
        /// <param name="len">The amount of data to receive.</param>
        /// <returns>A byte array containing the specified data.</returns>
        public virtual byte[] Receive(int len)
        {
            byte[] result = new byte[len];
            return Receive(result, 0, len);
        }

        /// <summary>
        /// Receives the specified number of bytes into the provided buffer.
        /// </summary>
        /// <param name="buffer">The buffer to receive the data.</param>
        /// <param name="index">The starting index to place the data.</param>
        /// <param name="length">The amount of data to receive.</param>
        /// <returns>A reference to <paramref name="buffer"/> if the operation completed successfully, or else 
        /// <see langword="null" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="buffer"/> is <see langword="null" /></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the combination of <paramref name="length"/> and 
        /// <paramref name="index"/> point to invalid positions in the buffer.</exception>
        public virtual byte[] Receive(byte[] buffer, int index, int length)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (index + length > buffer.Length)
                throw new ArgumentOutOfRangeException("length", length, "Index and length must be valid positions within the buffer.");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "Index must be nonnegative.");
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", length, "Length must be nonnegative and nonzero.");
            if (!m_open)
                return null;

            int totRecv = 0;
            NetworkStream localNS = m_client.GetStream();
            while (m_open && m_client.Connected && totRecv < length)
            {
                try
                {
                    totRecv += localNS.Read(buffer, totRecv + index, length - totRecv);
                }
                catch (IOException se)
                {
                    if (IsConnected)
                    {
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            Close();
                            OnError("A read error occurred on the connection.", se);
                        });
                    }
                    return null;
                }
            }

            return buffer;
        }

        /// <summary>
        /// Retrieves an arbitrarily-sized byte array of data.
        /// </summary>
        /// <returns>An array of bytes of data that have been received from the server.</returns>
        public virtual byte[] Receive()
        {
            byte[] incBuffer = new byte[2048];
            int recvsize = m_client.GetStream().Read(incBuffer, 0, 2048);
            byte[] result = new byte[recvsize];
            Buffer.BlockCopy(incBuffer, 0, result, 0, recvsize);
            return result;
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object, freeing unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">Whether to free managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_client != null && m_client.Connected)
                {
                    m_client.Close();
                }
                m_client = null;
                m_ipep = null;
                m_server = null;
                m_open = false;
            }
        }

        #endregion
    }
}
