using System;
using System.Collections.Generic;
using System.Text;

namespace MBNCSUtil.Data.Blp2
{
    [Flags]
    internal enum DdsCapabilityFlags
    {
        Alpha = 2,
        Complex = 8,
        Texture = 0x000010000,
        Mipmap = 0x00400000,

        Cubemap = 0x200,
        CubemapPositiveX = 0x400,
        CubemapNegativeX = 0x800,
        CubemapPositiveY = 0x1000,
        CubemapNegativeY = 0x2000,
        CubemapPositiveZ = 0x4000,
        CubemapNegativeZ = 0x8000,
        Volume = 0x00200000,
    }
}
