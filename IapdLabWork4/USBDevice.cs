using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IapdLabWork4
{
    class USBDevice
    {
        public string Volume { get; set; }
        public string VolumeName { get; set; }
        public ulong Size { get; set; }
        public ulong FreeSpace { get; set; }
        public ulong OccupiedSpace { get; set; }
        public string FileSystem { get; set; }
    }
}
