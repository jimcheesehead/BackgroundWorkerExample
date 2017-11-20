using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BackgroundWorkerExample
{
    // From: YouTube BackgroundWorker Class example in windows forms application
    // https://youtu.be/TwlO5XYeeMo

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int sum = 0;
            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(100);
                sum = sum + i;
                backgroundWorker1.ReportProgress(i); // Raises ProgressChanged event

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    backgroundWorker1.ReportProgress(0);
                    return;
                }
            }

            e.Result = sum;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                label1.Text = "Process cancelled";
            }
            else if (e.Error != null)
            {
                label1.Text = e.Error.Message;
            }
            else
            {
                label1.Text = "Sum = " + e.Result.ToString();
            }

            label2.Text = "";
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            // Check if backgroundWorker is already busy running the asynchronous operation
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(); // No argument for this example
            }
            else
            {
                label2.Text = "Busy processing, please wait";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                // Cancel the asynchronous operation if running
                backgroundWorker1.CancelAsync();
            }
            else
            {
                label1.Text = "No operation in progress to cancel";
            }
        }
    }
}
