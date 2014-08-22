using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MP3TagUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
#if DEBUG
            this.chShowLog.Checked = true;
#else
            this.chShowLog.Checked = false;
#endif
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = MP3TagUpdater.Properties.Settings.Default.LastDirectory + "";
            dlg.Description = "Select source directory where files will be taken for conversion from";
            if (dlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            txtSource.Text = dlg.SelectedPath;
            MP3TagUpdater.Properties.Settings.Default.LastDirectory = dlg.SelectedPath;
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = MP3TagUpdater.Properties.Settings.Default.LastDirectory + "";
            dlg.Description = "Select target directory where converted files will be copied to";
            if (dlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            txtTarget.Text = dlg.SelectedPath;
            MP3TagUpdater.Properties.Settings.Default.LastDirectory = dlg.SelectedPath;
        }

        Task _convert;

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!Validate())
                return;
            try
            {
                if (_convert == null || _convert.Status == TaskStatus.Running)
                {
                    return;
                }
                btnRun.Enabled = false;
                btnClose.Enabled = false;
                progressBar1.Value = 0;
                //FolderConverter converter = new FolderConverter(chShowLog.Checked, chUpdateTag.Checked);
                //converter.LogEvent += new LogMsgHandler(converter_LogEvent);
                //converter.ProgressEvent += new ProgressHandler(converter_ProgressEvent);
                _convert = new Task(() =>
                {
                    FolderConverter converter = new FolderConverter(chShowLog.Checked, chUpdateTag.Checked);
                    converter.LogEvent += new LogMsgHandler(converter_LogEvent);
                    converter.ProgressEvent += new ProgressHandler(converter_ProgressEvent);
                    converter.Convert(txtSource.Text, txtTarget.Text);
                });
                _convert.Start();
                //converter.Convert(txtSource.Text, txtTarget.Text);   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRun.Enabled = true;
                btnClose.Enabled = true;
            }
        }

        void converter_ProgressEvent(double current, double total)
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(() => { converter_ProgressEvent(current, total); }));
            else
            {
                progressBar1.Value = total == 0 ? 0 : (int)(current / total * 100);
                if (progressBar1.Value == 100)
                {
                    btnRun.Enabled = true;
                    btnClose.Enabled = true;
                    MessageBox.Show("Conversion is done");
                }
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
        }

        void converter_LogEvent(string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(() => { converter_LogEvent(msg); }));
            else
            {
                txtLog.Text += msg + "\r\n";
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
        }

        private bool Validate()
        {
            if (!System.IO.Directory.Exists(txtSource.Text))
            {
                MessageBox.Show("Directory '" + txtSource.Text + "' doesn't exist", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (txtTarget.Text.Length == 0)
            {
                MessageBox.Show("Target Directory should be specified", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
