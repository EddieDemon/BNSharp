using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MBNCSUtil.Data.Blp2
{
    internal class DDCaps
    {
        public int caps1;
        public int caps2;


        public static DDCaps Read(System.IO.Stream s)
        {
            using (BinaryReader br = new BinaryReader(s))
            {
                return Read(br);
            }
        }

        public static DDCaps Read(System.IO.BinaryReader r)
        {
            DDCaps caps = new DDCaps();
            caps.caps1 = r.ReadInt32();
            caps.caps2 = r.ReadInt32();
            r.ReadInt64(); // reserved[2]

            return caps;
        }
    }
}
