using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MBNCSUtil.Data.Blp2
{
    internal class DDPixelFormat
    {
        public int size;
        public DxtPixelFormat flags;
        public int fourCC;
        public int rgbBitCount;
        public int rBitMask;
        public int gBitMask;
        public int bBitMask;
        public int rgbAlphaBitMask;

        public static DDPixelFormat Read(Stream s)
        {
            using (BinaryReader reader = new BinaryReader(s))
            {
                return Read(reader);
            }
        }

        public static DDPixelFormat Read(BinaryReader br)
        {
            DDPixelFormat fmt = new DDPixelFormat();
            fmt.size = br.ReadInt32();
            fmt.flags = (DxtPixelFormat)br.ReadInt32();
            fmt.fourCC = br.ReadInt32();
            fmt.rgbBitCount = br.ReadInt32();
            fmt.rBitMask = br.ReadInt32();
            fmt.gBitMask = br.ReadInt32();
            fmt.bBitMask = br.ReadInt32();
            fmt.rgbAlphaBitMask = br.ReadInt32();

            return fmt;
        }
    }
}
