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
        

        private const int WM_DEVICECHANGE = 0x219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x00000002;

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            RefreshData();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                RefreshData();
            }
            base.WndProc(ref m);
        }

        /*
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
                    checkBox1.Checked = !checkBox1.Checked;
                    break;
            }

        }*/

        private void RefreshData()
        {
            USBDevices usbDevices = new USBDevices();
            dataGridView1.Rows.Clear();
            foreach (var device in usbDevices.getDevices())
            {
                try
                {
                    dataGridView1.Rows.Add(device.Volume, device.VolumeName,
                        String.Format("{0:F2}", device.Size / 1024.0 / 1024.0 / 1024.0) + " GBytes",
                        String.Format("{0:F2}", device.FreeSpace / 1024.0 / 1024.0 / 1024.0) + " GBytes",
                        String.Format("{0:F2}", device.OccupiedSpace / 1024.0 / 1024.0 / 1024.0) + " GBytes",
                        device.FileSystem);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Not all data is");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
