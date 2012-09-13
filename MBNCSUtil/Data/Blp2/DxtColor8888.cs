using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace MBNCSUtil.Data.Blp2
{
    internal class DxtColor8888
    {
        byte b, g, r, a;
        public static DxtColor8888 Read(Stream s)
        {
            using (BinaryReader br = new BinaryReader(s))
            {
                return Read(br);
            }
        }

        public static DxtColor8888 Read(BinaryReader br)
        {
            DxtColor8888 color = new DxtColor8888();
            color.b = br.ReadByte();
            color.g = br.ReadByte();
            color.r = br.ReadByte();
            color.a = br.ReadByte();

            return color;
        }

        public Color ToColor()
        {
            return Color.FromArgb(a, r, g, b);
        }

        public static DxtColor8888 FromColor(Color c)
        {
            DxtColor8888 d = new DxtColor8888();
            d.a = c.A;
            d.r = c.R;
            d.g = c.G;
            d.b = c.B;
            return d;
        }
    }
}
