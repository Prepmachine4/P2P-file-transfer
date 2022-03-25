using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Threading;


namespace P2P_file_transfer
{
    
    public partial class mainForm : Form
    {
        public FileRecive fileRecive;
        public mainForm()
        {
            InitializeComponent();



            ListView.CheckForIllegalCrossThreadCalls = false;


            GlobalData.form1 = this;
            ReceiveMessage.run();
            labelHostName.Text = Dns.GetHostName();
            labelIP.Text = GetLocalIp();
            SendMessage.sendToTracker("login");

            

        }

      
        
        // 选择ip地址
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listView1.SelectedItems.Count;
            GlobalData.selectIP = ReceiveMessage.hosts[index - 1].ToString();
            GlobalData.form2 = new Form2();
            SendMessage.sendTo("file", GlobalData.selectIP, 5000);
            GlobalData.form2.Show();
        }
        public void initListView(List<string> hosts)
        {
            this.listView1.Clear();
            this.listView1.BeginUpdate();
            for (int i = 0; i < hosts.Count; i++)
            {
                if (hosts[i] == GetLocalIp())
                {
                    hosts[i] = "my:" + hosts[i];
                }
                this.listView1.Items.Add(hosts[i]);
            }

            this.listView1.EndUpdate();
        }
        public static string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SendMessage.sendToTracker("logout");

            // close
            ReceiveMessage.threadReceive.Abort();
            ReceiveMessage.sock.Close();




        }
        private void mainForm_Load(object sender, EventArgs e)
        {


        }

        
    }

    public class GlobalData
    {
        public static string selectIP;
        public static Form2 form2;
        public static mainForm form1;
    }

}
