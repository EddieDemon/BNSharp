using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace MBNCSUtil.Data.Blp2
{
    internal class DxtColor565
    {
        public byte b, g, r;

        public static DxtColor565 Read(Stream s)
        {
            using (BinaryReader br = new BinaryReader(s))
            {
                return Read(br);
            }
        }

        public static DxtColor565 Read(BinaryReader br)
        {
            short val = br.ReadInt16();
            DxtColor565 color = new DxtColor565();
            color.b = unchecked((byte)((byte)((byte)(val >> 11)) & (byte)0x1f));
            color.g = unchecked((byte)((byte)((byte)(val >> 5)) & (byte)0x3f));
            color.r = (byte)((byte)val & (byte)0x1f);
            return color;
        }

        public Color ToColor()
        {
            return Color.FromArgb(From5To8(r), From6To8(g), From5To8(b));
        }

        public DxtColor565 FromColor(Color c)
        {
            DxtColor565 d = new DxtColor565();
            d.r = From8To5(c.R);
            d.g = From8To6(c.G);
            d.b = From8To5(c.B);
            return d;
        }

        private static byte From5To8(byte fiveBit)
        {
            byte tmp = unchecked((byte)(fiveBit * 8));
            if (fiveBit == 32)
                tmp = 255;
            return tmp;
        }

        private static byte From6To8(byte sixBit)
        {
            byte tmp = unchecked((byte)(sixBit * 4));
            if (sixBit == 64)
                tmp = 255;
            return tmp;
        }

        private static byte From8To6(byte eightBit)
        {
            byte tmp = unchecked((byte)(eightBit / 4));
            if (eightBit == 255)
                tmp = 64;
            return tmp;
        }

        private static byte From8To5(byte eightBit)
        {
            byte tmp = unchecked((byte)(eightBit / 8));
            if (eightBit == 255)
                tmp = 32;
            return tmp;
        }
    }
}
