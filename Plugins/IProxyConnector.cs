using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.Net;
using System.Net;

namespace BNSharp.Plugins
{
    /// <summary>
    /// Enables a client connection to be connected through a proxy server that may decorate any part of the protocol with additional data.
    /// </summary>
    public interface IProxyConnector
    {
        /// <summary>
        /// Initializes the proxy connector for the specified client.
        /// </summary>
        /// <param name="client">Provides the real connection services to the proxy connector.</param>
        void Initialize(IProxiedRealConnection client);

        /// <summary>
        /// Allows a presentation-level proxy protocol to negotiate immediately after connecting to the proxy host.
        /// </summary>
        /// <returns><see langword="true" /> if negotiation succeeded; otherwsie <see langword="false" />.</returns>
        bool Negotiate();

        /// <summary>
        /// Resolves an IP end point for the specified server and port.
        /// </summary>
        /// <param name="host">The DNS name or IP address (as a string) of the server to look up.</param>
        /// <param name="port">The port number to which to connect.</param>
        /// <remarks>
        /// <para>Proxy implementations should use this method to resolve the real and proxy IP addresses for connection and return 
        /// the IP end point of the proxy host.  This allows the base class to transparently connect to the proxy server.</para>
        /// </remarks>
        /// <returns>An <see>IPEndPoint</see> representing the server and port.</returns>
        IPEndPoint ResolveEndPoint(string host, int port);

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
        byte[] Receive(byte[] buffer, int index, int length);

        /// <summary>
        /// Retrieves an arbitrarily-sized byte array of data.
        /// </summary>
        /// <returns>An array of bytes of data that have been received from the server.</returns>
        byte[] Receive();

        /// <summary>
        /// Sends part of the specified binary data to the server.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="index">The start index of the data.</param>
        /// <param name="length">The amount of data to send.</param>
        void Send(byte[] data, int index, int length);
    }
}
