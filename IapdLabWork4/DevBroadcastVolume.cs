﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IapdLabWork4
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DevBroadcastVolume
    {
        public int Size;
        public int DeviceType;
        public int Reserved;
        public int Mask;
        public Int16 Flags;
    }
}
