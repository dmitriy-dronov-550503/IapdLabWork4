using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace IapdLabWork4
{
    public partial class Form1 : Form
    {
        USBDevices usbDevices = new USBDevices();

        private const int WM_DEVICECHANGE = 0x219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x00000002;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch ((int)m.WParam)
                    {
                        case DBT_DEVICEARRIVAL:
                            dataGridView1.Rows.Add("New Device Arrived");

                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                DevBroadcastVolume vol;
                                vol = (DevBroadcastVolume)
                                   Marshal.PtrToStructure(m.LParam,
                                   typeof(DevBroadcastVolume));
                                dataGridView1.Rows.Add("Mask is " + vol.Mask);
                            }

                            break;

                        case DBT_DEVICEREMOVECOMPLETE:
                            dataGridView1.Rows.Add("Device Removed");
                            break;

                    }
                    break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach(var device in usbDevices.getDevices())
            {
                dataGridView1.Rows.Add(device.getName(), device.getManufacturer(), device.getParameters()["deviceId"], device.getDesription());
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
