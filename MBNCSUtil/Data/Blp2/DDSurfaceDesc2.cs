using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MBNCSUtil.Data.Blp2
{
    internal class DDSurfaceDesc2
    {
        public int size, flags, height, width, pitchOrLinearSize, depth, mipCount;
        int reserved1; // [11]
        public DDPixelFormat ddpfPixelFormat;
        public DDCaps ddsCaps;
        int reserved2;

        public static DDSurfaceDesc2 Read(Stream s)
        {
            using (BinaryReader reader = new BinaryReader(s))
            {
                return Read(reader);
            }
        }

        public static DDSurfaceDesc2 Read(BinaryReader reader)
        {
            DDSurfaceDesc2 s = new DDSurfaceDesc2();
            s.size = reader.ReadInt32();
            s.flags = reader.ReadInt32();
            s.height = reader.ReadInt32();
            s.width = reader.ReadInt32();
            s.pitchOrLinearSize = reader.ReadInt32();
            s.depth = reader.ReadInt32();
            s.mipCount = reader.ReadInt32();
            for (int i = 0; i < 11; i++) // reserved1
                reader.ReadInt32();
            s.ddpfPixelFormat = DDPixelFormat.Read(reader);
            s.ddsCaps = DDCaps.Read(reader);
            reader.ReadInt32(); // reserved2

            return s;
        }
    }
}
