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
        bool backgroundWorkerResult = false;
        private const int WM_DEVICECHANGE = 0x219;

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
                try
                {
                    dataGridView1.Rows[i].Cells[6].Value = true;
                }
                catch (Exception ex) { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            progressBar2.Visible = true;
            backgroundWorker1.RunWorkerAsync();
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells[6].Value)
                {
                    string res = EjectClass.EjectUSBDrive(((string)row.Cells[0].Value).First());
                    MessageBox.Show(res);
                    backgroundWorkerResult = res.Contains("OK");
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorkerResult)
            {
                RefreshData();
                progressBar1.Value = 0;
            }
            progressBar2.Visible = false;
            timer1.Start();
        }
    }
}
