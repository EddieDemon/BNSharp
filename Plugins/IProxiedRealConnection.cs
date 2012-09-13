using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BNSharp.Plugins
{
    /// <summary>
    /// Provides real communication services to a proxy connector.  This interface does not need to be implemented by a user wishing to 
    /// add proxy support to BN#; rather, it is provided via the <see cref="IProxyConnector.Initialize">IProxyConnector.Initialize</see> method.
    /// </summary>
    public interface IProxiedRealConnection
    {
        /// <summary>
        /// Receives the specified number of bytes into the provided buffer from the real underlying connection.
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
        /// Retrieves an arbitrarily-sized byte array of data from the real underlying connection.
        /// </summary>
        /// <returns>An array of bytes of data that have been received from the server.</returns>
        byte[] Receive();

        /// <summary>
        /// Sends part of the specified binary data to the server through the real underlying connection.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="index">The start index of the data.</param>
        /// <param name="length">The amount of data to send.</param>
        void Send(byte[] data, int index, int length);

        /// <summary>
        /// Provides a real implementation of endpoint resolution for the specified host and port.
        /// </summary>
        /// <param name="host">The DNS name or IP address (as a string) of the server to look up.</param>
        /// <param name="port">The port number to which to connect.</param>
        /// <returns>An <see>IPEndPoint</see> representing the server and port.</returns>
        IPEndPoint ResolveEndPoint(string host, int port);
    }
}
