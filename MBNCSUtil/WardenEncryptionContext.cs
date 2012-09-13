using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.MBNCSUtil.Util;

namespace BNSharp.MBNCSUtil
{
    /// <summary>
    /// Handles the encryption and decryption of Warden modules for a given connection.
    /// </summary>
    public class WardenEncryptionContext
    {
        private WardenCrypt m_out, m_in;

        /// <summary>
        /// Creates a new <see>WardenEncryptionContext</see> with the specified seed.
        /// </summary>
        /// <param name="keyHashPart">The value with which to seed the encryption keys.</param>
        public WardenEncryptionContext(int keyHashPart)
        {
            WardenRandom rnd = new WardenRandom(BitConverter.GetBytes(keyHashPart));
            m_out = new WardenCrypt(rnd.GetBytes(16));
            m_in = new WardenCrypt(rnd.GetBytes(16));
        }

        /// <summary>
        /// Encrypts an entire array.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <see langword="null" />.</exception>
        /// <returns>A new, encrypted array.</returns>
        public byte[] Encrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            return Encrypt(data, 0, data.Length);
        }

        /// <summary>
        /// Encrypts a block of data using the current encryption key.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <param name="start">The starting 0-based index to begin encryption.</param>
        /// <param name="length">The amount of data to encrypt.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="start"/> is negative, or if <paramref name="start"/> and <paramref name="length"/>
        /// sum to greater than the length of <paramref name="data"/>.</exception>
        /// <returns>A new, encrypted array.</returns>
        public byte[] Encrypt(byte[] data, int start, int length)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (start < 0)
                throw new ArgumentOutOfRangeException("start", start, "Starting index must be non-negative.");

            if (start + length > data.Length)
                throw new ArgumentOutOfRangeException("length", length,
                    "Starting index and length must be no longer than the total length of the array.");

            byte[] result = new byte[length];
            Buffer.BlockCopy(data, start, result, 0, length);
            m_out.Crypt(result);

            return result;
        }

        /// <summary>
        /// Decrypts an entire array.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <see langword="null" />.</exception>
        /// <returns>A new, decrypted array.</returns>
        public byte[] Decrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            return Decrypt(data, 0, data.Length);
        }

        /// <summary>
        /// Decrypts a block of data using the current decryption key.
        /// </summary>
        /// <param name="data">The data to decrypt.</param>
        /// <param name="start">The starting 0-based index to begin decryption.</param>
        /// <param name="length">The amount of data to decrypt.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="start"/> is negative, or if <paramref name="start"/> and <paramref name="length"/>
        /// sum to greater than the length of <paramref name="data"/>.</exception>
        /// <returns>A new, decrypted array.</returns>
        public byte[] Decrypt(byte[] data, int start, int length)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (start < 0)
                throw new ArgumentOutOfRangeException("start", start, "Starting index must be non-negative.");

            if (start + length > data.Length)
                throw new ArgumentOutOfRangeException("length", length, "Starting index and length must be no longer than the total length of the array.");
            
            byte[] result = new byte[length];
            Buffer.BlockCopy(data, start, result, 0, length);
            m_in.Crypt(result);

            return result;
        }
    }
}
