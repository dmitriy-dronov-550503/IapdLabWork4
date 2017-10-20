using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Management;

namespace IapdLabWork4
{
    class USBDevices
    {
        private List<DeviceInformation> devices;

        public USBDevices()
        {
            devices = new List<DeviceInformation>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, DeviceID, Description, Manufacturer, Service FROM Win32_PnPEntity WHERE DeviceID LIKE '%USB\\\\%' AND (Service LIKE '%HidUsb%' OR Service LIKE '%USBSTOR%')");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                var device = new DeviceInformation();
                device.setName((string)queryObj["Name"]);
                device.setDescription((string)queryObj["Description"]);
                device.setManufacturer((string)queryObj["Manufacturer"]);
                device.setParameters(parseQueryResult((string)queryObj["DeviceID"]));
                devices.Add(device);
            }
        }

        private Dictionary<string, string> parseQueryResult(string queryResult)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            info["deviceType"] = new Regex("(.*?)(?=\\\\)").Match(queryResult).Value;
            info["vendorId"] = new Regex("(?<=VID_)(.{0,4})(?=&)").Match(queryResult).Value;
            info["deviceId"] = new Regex("(?<=PID_)(.{0,4})(?=&)").Match(queryResult).Value;
            info["multiplyInterface"] = new Regex("(?<=MI_)(.{0,2})(?=&)").Match(queryResult).Value;
            info["revisionId"] = new Regex("(?<=REV_)(.{0,2})(?=\\\\)").Match(queryResult).Value;
            info["otherId"] = new Regex("(?<=\\\\)(?!VEN_)(.*)").Match(queryResult).Value;
            return info;
        }

        public List<DeviceInformation> getDevices()
        {
            return devices;
        }
    }
}
