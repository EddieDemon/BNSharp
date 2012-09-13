namespace BNSharp.MBNCSUtil.Util
{
    internal class WardenCrypt
    {
        private byte[] m_key;

        public WardenCrypt(byte[] baseData)
        {
            char val = (char)0;
            int i;
            int position = 0;

            this.m_key = new byte[0x102];

            for (i = 0; i < 0x100; i++)
                m_key[i] = (byte)i;

            this.m_key[0x100] = 0;
            this.m_key[0x101] = 0;

            for (i = 1; i <= 0x40; i++)
            {
                val += (char)(this.m_key[(i * 4) - 4] + baseData[position++ % baseData.Length]);
                byte temp = this.m_key[(i * 4) - 4];
                this.m_key[(i * 4) - 4] = this.m_key[val & 0xff];
                this.m_key[val & 0xff] = temp;

                val += (char)(this.m_key[(i * 4) - 3] + baseData[position++ % baseData.Length]);
                temp = this.m_key[(i * 4) - 3];
                this.m_key[(i * 4) - 3] = this.m_key[val & 0xff];
                this.m_key[val & 0xff] = temp;

                val += (char)(this.m_key[(i * 4) - 2] + baseData[position++ % baseData.Length]);
                temp = this.m_key[(i * 4) - 2];
                this.m_key[(i * 4) - 2] = this.m_key[val & 0xff];
                this.m_key[val & 0xff] = temp;

                val += (char)(m_key[(i * 4) - 1] + baseData[position++ % baseData.Length]);
                temp = m_key[(i * 4) - 1];
                this.m_key[(i * 4) - 1] = this.m_key[val & 0xff];
                this.m_key[val & 0xff] = temp;
            }
        }

        public void Crypt(byte[] data)
        {
            int i;

            unchecked
            {
                for (i = 0; i < data.Length; i++)
                {
                    m_key[0x100]++;
                    m_key[0x101] += m_key[m_key[0x100]];
                    byte temp = m_key[m_key[0x101]];
                    m_key[m_key[0x101]] = m_key[m_key[0x100]];
                    m_key[m_key[0x100]] = temp;

                    data[i] = (byte)(data[i] ^ (m_key[(m_key[m_key[0x101] & 0x0FF] + m_key[m_key[0x100] & 0x0FF]) & 0x0FF]));
                }
            }
        }

    }
}
