using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Management;
using System.IO;

namespace IapdLabWork4
{
    class USBDevices
    {
        private List<USBDevice> devices = new List<USBDevice>();

        public USBDevices()
        {
            foreach(var drive in DriveInfo.GetDrives())
            {
                var device = new USBDevice();
                if ((drive.DriveType != DriveType.Fixed) && drive.IsReady)
                {
                    try
                    {
                        device.Volume = drive.Name.Replace("\\", "");
                        device.VolumeName = drive.VolumeLabel;
                        device.Size = (ulong)drive.TotalSize;
                        device.FreeSpace = (ulong)drive.TotalFreeSpace;
                        device.OccupiedSpace = device.Size - device.FreeSpace;
                        device.FileSystem = drive.DriveFormat;
                    }
                    catch (Exception e)
                    {
                        //System.Windows.Forms.MessageBox.Show("Not all data was readed");
                    }
                    devices.Add(device);
                }
            }
        }

        public List<USBDevice> getDevices()
        {
            return devices;
        }
    }
}
