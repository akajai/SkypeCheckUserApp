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


        public Form1()
        {
            InitializeComponent();
            textBoxoutputFIleName.Text = "D:\\out.txt";
            textBoxinput.Text = "D:\\1.txt";
            textBoxskypeusername.Text = "check_user1";
            textBoxskypepassword.Text = "check_user1";
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
                    MessageBox.Show("File Not Present");
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxoutputFIleName.Text.Length == 0)
            {
                MessageBox.Show("Output File Name  Is Empty ");
            }
            
            if (textBoxinput.Text.Length == 0)
            {
                MessageBox.Show("Input File Name  Is Empty ");
            }
            if (File.Exists(textBoxoutputFIleName.Text))
            {
                MessageBox.Show("File Already Present ");
                //return;
            }
            try
            {
                System.Diagnostics.Process process = new Process();
                
                Skype skype = new Skype();

                
                if (skype.Client.IsRunning)
                {
                    Console.WriteLine("Skype is running");
                    //skype.Client.Start(true, true);
                }
                else
                {
//                    Console.WriteLine("Skype is not running so starting skype and login in ");
//                    //process.WaitForInputIdle("skype.exe /username:" + textBoxskypeusername + "/password:" + textBoxskypepassword);

//                    //System.Diagnostics.Process.Start("C:/Program Files/Skype/Phone/Skype.exe"+" /username:" + textBoxskypeusername + "/password:" + textBoxskypepassword);
//                    process.StartInfo = new ProcessStartInfo();

//                    process.StartInfo.FileName = @"C:\Program Files\Skype\Phone\Skype.exe";
//                    process.StartInfo.Verb = "Skype.exe";
//                   process.StartInfo.CreateNoWindow = true;
//                    process.StartInfo.Domain = "Domain";
//                    process.EnableRaisingEvents = true;
//                    process.StartInfo.UseShellExecute = false;
//                    //process.StartInfo.LoadUserProfile = true;
//                    //process.StartInfo.Arguments=" /username:" + textBoxskypeusername + "/password:" + textBoxskypepassword;
//                    //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
//                    //process.StartInfo.RedirectStandardOutput = true;
//                    process.StartInfo.UserName = textBoxskypeusername.Text;
                    
//                   //// process.StartInfo.Password.Clear();
//                   // Console.WriteLine("Skype Username");

//                   // //process.StartInfo.Password.AppendChar('c');
//                   // //process.StartInfo.Password.AppendChar('h');
//                   // //process.StartInfo.Password.AppendChar('e');
//                   // //process.StartInfo.Password.AppendChar('c');
//                   // //process.StartInfo.Password.AppendChar('k');
//                   // //process.StartInfo.Password.AppendChar('_');
//                   // //process.StartInfo.Password.AppendChar('u');
//                   // //process.StartInfo.Password.AppendChar('s');
//                   // //process.StartInfo.Password.AppendChar('e');
//                   // //process.StartInfo.Password.AppendChar('r');
//                   // //process.StartInfo.Password.AppendChar('1');

//                    for (int i = 0; i < textBoxskypepassword.Text.Length; i++)
//                    {
//                        process.StartInfo.Password.AppendChar(textBoxskypepassword.Text.ToString()[i]);
//                    }
////                    Console.WriteLine("Password " + process.StartInfo.Password.ToString());
//                    //process.WaitForInputIdle();
//                    //process.StartInfo.WorkingDirectory = @"C:\Program Files\Skype\Phone";
//                    Console.WriteLine("Skype Started");
                }
                // wait for the client to be connected and ready
                //skype.Timeout = 1000;
                
                
                //skype.Attach(6, true);
                skype.Attach(skype.Protocol, false);
                ////skype.SilentMode.
                ////
                Console.WriteLine("Skype Version" + skype.Version);
                //Console.WriteLine("Enter a skype name to search for: ");
                //string username = Console.ReadLine();
                 //Read the file and display it line by line.
                int counter = 0;
                string line;
                List<String> emailids= new List<String>();
                
                System.IO.StreamReader file =new System.IO.StreamReader(textBoxinput.Text);
                while ((line = file.ReadLine()) != null)
                {
                 //   Console.WriteLine(line);
                    emailids.Add(line);
                    counter++;
                }
                file.Close();
                Console.WriteLine("emailids Count " + emailids.Count);
                 Dictionary<string, string> datalist = new Dictionary<string, string>();
                List<String> validemailids= new List<String>();
                

                foreach (String emailid in emailids)
                {
                    System.IO.StreamWriter outputfile = new System.IO.StreamWriter(textBoxoutputFIleName.Text, true);
                    foreach (User user in skype.SearchForUsers(emailid))
                    {

                        Console.WriteLine(emailid + " : " + user.FullName);
                        validemailids.Add(emailid);
                        outputfile.WriteLine(emailid + " : " + user.Aliases +" : " + user.DisplayName+ " : " + user.FullName);
                    }
                    outputfile.Close();
                }
                
                
                //System.IO.StreamWriter outputfile = new System.IO.StreamWriter(textBoxoutputFIleName.Text);
                //for (int j = 0; j < validemailids.Count; j++)
                //{
                //    outputfile.WriteLine(validemailids[j].ToString());
                //}
                //outputfile.Close();

                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excelption " + ex.Message);
            }

            
        }
    }
}
