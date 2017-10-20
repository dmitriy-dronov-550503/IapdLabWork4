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
using USBEjectLibrary;

namespace IapdLabWork4
{
    public partial class Form1 : Form
    {

        USBDevices usbDevices;
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

        private void RefreshData()
        {
            List<int> checkedRows = new List<int>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells[6].Value)
                {
                    checkedRows.Add(row.Index);
                }
            }

            usbDevices = new USBDevices();
            dataGridView1.Rows.Clear();
            foreach (var device in usbDevices.GetDevices())
            {
                try
                {
                    dataGridView1.Rows.Add(device.Volume, device.VolumeName,
                        String.Format("{0:F2}", device.Size / 1024.0 / 1024.0 / 1024.0) + " GBytes",
                        String.Format("{0:F2}", device.OccupiedSpace / 1024.0 / 1024.0 / 1024.0) + " GBytes",
                        String.Format("{0:F2}", device.FreeSpace / 1024.0 / 1024.0 / 1024.0) + " GBytes",
                        device.FileSystem, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not all data was loaded");
                }
            }

            foreach (var i in checkedRows)
            {
                dataGridView1.Rows[i].Cells[6].Value = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if((bool)row.Cells[6].Value)
                {
                    MessageBox.Show(EjectClass.EjectUSBDrive(((string)row.Cells[0].Value).First()));
                    dataGridView1.Rows.Remove(row);
                    progressBar1.Value = 0;
                }
            }
            timer1.Start();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            USBDevice device = usbDevices.GetDevices()[dataGridView1.CurrentRow.Index];
            progressBar1.Value = (int)(device.FreeSpace*100/device.Size);
            if (dataGridView1.CurrentCell.ColumnIndex == 6)
                dataGridView1.CurrentRow.Cells[Column7.Name].Value = !(bool)dataGridView1.CurrentRow.Cells[Column7.Name].Value;
        }
    }
}
