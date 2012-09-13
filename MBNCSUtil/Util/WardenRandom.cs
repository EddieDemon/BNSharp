using System.Security.Cryptography;

namespace BNSharp.MBNCSUtil.Util
{
    internal class WardenRandom
    {
        private int Position;
        private byte[] Data1;
        private byte[] Data2;
        private byte[] Data3;

        public WardenRandom(byte[] seed)
        {
            Data1 = new byte[0x14];
            Data2 = new byte[0x14];
            Data3 = new byte[0x14];

            int length1 = (int)seed.Length >> 1;
            int length2 = seed.Length - length1;

            byte[] seed1 = new byte[length1];
            byte[] seed2 = new byte[length2];

            for (int i = 0; i < length1; i++)
                seed1[i] = seed[i];
            for (int i = 0; i < length2; i++)
                seed2[i] = seed[i + length1];

            SHA1 sha = new SHA1Managed();
            Data2 = sha.ComputeHash(seed1);
            Data3 = sha.ComputeHash(seed2);

            sha.Initialize();
            sha.TransformBlock(Data2, 0, Data2.Length, Data2, 0);
            sha.TransformBlock(Data1, 0, Data1.Length, Data1, 0);
            sha.TransformFinalBlock(Data3, 0, Data3.Length);

            Data1 = sha.Hash;

            sha.Initialize();
        }

        private void Update()
        {
            SHA1Managed sha = new SHA1Managed();

            sha.TransformBlock(Data2, 0, Data2.Length, Data2, 0);
            sha.TransformBlock(Data1, 0, Data1.Length, Data1, 0);
            sha.TransformFinalBlock(Data3, 0, Data3.Length);

            Data1 = sha.Hash;
        }

        public byte[] GetBytes(int count)
        {
            byte[] m_bBytes = new byte[count];
            for (int i = 0; i < count; i++)
                m_bBytes[i] = GetByte();
            return m_bBytes;
        }

        private byte GetByte()
        {
            int m_iPos = Position;
            byte m_bVal = Data1[m_iPos];
            m_iPos++;
            if (m_iPos >= 0x14)
            {
                m_iPos = 0;
                Update();
            }
            Position = m_iPos;
            return m_bVal;
        }
    }
}
