using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2P_file_transfer
{
    partial class mainForm
    {
        public void initListView(List<string> hosts)
        {
            this.listView1.Clear();
            this.listView1.BeginUpdate();
            for (int i = 0; i < hosts.Count; i++)
            {
                if(hosts[i] == GetLocalIp())
                {
                    hosts[i] = "my:"+hosts[i];
;                }
                this.listView1.Items.Add(hosts[i]);
            }

            this.listView1.EndUpdate();
        }
        public string GetLocalIp()
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
    }
}
