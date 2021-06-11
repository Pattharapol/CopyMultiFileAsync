using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyMultipleFileWithProgressBar
{
    public partial class Form1 : Form
    {

        BackgroundWorker worker = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = false;

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
        }

        private void CopyFIle(string source, string destination)
        {
            FileStream fsOut = new FileStream(destination, FileMode.Create);
            FileStream fsIn = new FileStream(source, FileMode.Open);
            byte[] bt = new byte[1048756];
            int readByte;
            while ((readByte = fsIn.Read(bt, 0, bt.Length)) > 0)
            {
                fsOut.Write(bt, 0, readByte);
                worker.ReportProgress((int)(fsIn.Position * 100 / fsIn.Length));
            }
            fsIn.Close();
            fsOut.Close();
        }


        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lblPercentage.Text = progressBar.Value.ToString() + " %";
            if (progressBar.Value == 100)
            {
                progressBar.Value = 0;
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                CopyFIle(listBox.Items[i].ToString(), txtDestination.Text+"\\"+Path.GetFileName(listBox.Items[i].ToString()));
            }
            MessageBox.Show("OK");
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.Multiselect = true;
            if(opd.ShowDialog() == DialogResult.OK)
            {
                listBox.Items.AddRange(opd.FileNames);
            }
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtDestination.Text = fbd.SelectedPath;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
        }
    }
}
