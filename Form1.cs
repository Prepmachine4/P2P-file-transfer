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
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            ReceiveMessage.run();
            labelHostName.Text = Dns.GetHostName();
            SendMessage.send("login");
            Thread.Sleep(500);
            List<string> hosts = ReceiveMessage.getList();
            initListView(hosts);



        }

        
    }
    
}
