using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SKYPE4COMLib;
using System.Diagnostics;
namespace SkypeCheckUserApp
{

    public partial class Form1 : Form
    {

        public SKYPE4COMLib.Skype skypeMain;
        List<String> emailids = new List<String>();
        int totalemailids = 0;
        public Form1()
        {
            InitializeComponent();
            textBoxoutputFIleName.Text = "D:\\out.txt";
            textBoxinput.Text = "D:\\1.txt";
        }

        private void inputBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(openFileDialog1.FileName))
                {
                    textBoxinput.Text = openFileDialog1.FileName;
                }
                else
                {
                    LogError("File Not Present");
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (buttonStart.Text == "Start")
            {
                if (textBoxoutputFIleName.Text.Length == 0)
                {
                    LogError("Output File Name  Is Empty ");

                    return;
                }

                if (textBoxinput.Text.Length == 0)
                {
                    LogError("Input File Name  Is Empty ");
                    return;
                }
                if (File.Exists(textBoxoutputFIleName.Text))
                {
                    LogError("File Already Present ");
                    //return;
                }
                if (IsFileInUse(textBoxinput.Text))
                {
                    LogError("Input File is Not Acessible");
                    return;
                }
                skypeMain = new Skype();
                if (skypeMain.Client.IsRunning)
                {
                    LogError("Skype is running",false);
                    //skype.Client.Start(true, true);
                }
                else
                {
                    LogError("Skype is not running. Please login to Skype");
                    return;
                }
                //skype.Attach(6, true);
                skypeMain.Attach(skypeMain.Protocol, false);
                ////skype.SilentMode.
                ////
                LogError("Skype Version" + skypeMain.Version, false);
                totalemailids = 0;
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(textBoxinput.Text);
                while ((line = file.ReadLine()) != null)
                {
                    emailids.Add(line);
                    totalemailids++;
                }
                file.Close();

                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Maximum = totalemailids;
                if (backgroundWorker1.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    backgroundWorker1.RunWorkerAsync();
                }
                // buttonStart.Text = "Stop";
                
            }
            else
            {
                if (backgroundWorker1.WorkerSupportsCancellation == true)
                {
                    // Cancel the asynchronous operation.
                    backgroundWorker1.CancelAsync();
                    backgroundWorker1.Dispose();
                    LogError("Operation Cancelled");

                    buttonStart.Text = "Start";
                }

                
            }


        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Dictionary<string, string> datalist = new Dictionary<string, string>();
                int index = 0;
                foreach (String emailid in emailids)
                {
                    System.IO.StreamWriter outputfile = new System.IO.StreamWriter(textBoxoutputFIleName.Text, true);
                    foreach (User user in skypeMain.SearchForUsers(emailid))
                    {
                        //outputfile.WriteLine(emailid + " : " + user.Aliases +" : " + user.DisplayName+ " : " + user.FullName);
                        outputfile.WriteLine(emailid + " : " + user.FullName + " : " + user.Handle);
                    }
                    LogError(emailid+" - ("+ index + "/" + totalemailids+")", false);
                    outputfile.Close();
                    index++;
                    backgroundWorker1.ReportProgress(index);

                }


            }
            catch (Exception ex)
            {
                LogError("Excelption " + ex.Message);
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // LogError("ProgressChanged",false);
            toolStripProgressBar1.Value = e.ProgressPercentage;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            if (e.Cancelled == true)
            {
                LogError("Canceled");
            }
            else if (e.Error != null)
            {
                LogError("Error: " + e.Error.Message);
            }
            else
            {
                LogError("Done!");
            }


        }
       
        public bool IsFileInUse(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) { }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
        public void LogError(String str, Boolean showMessageBox = true)
        {
            if (showMessageBox)
            {
                MessageBox.Show(str);
            }
            toolStripStatusLabel1.Text = str;

        }
    }
}
