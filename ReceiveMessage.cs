using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace P2P_file_transfer
{
    

    internal class ReceiveMessage
    {
        public static List<string> hosts;
        static byte[] data;
        static int length;
        public static Socket sock;
        static string str;
        public static Thread threadReceive;
        static ReceiveMessage()
        {

            data = new byte[1024];
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Bind(new IPEndPoint(IPAddress.Any, 5000));
            
        }
        public static void run()
        {
            threadReceive = new Thread(new ThreadStart(thread));
            threadReceive.Start();
        }
        public static void thread()
        {
            while (true)
            {
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                if (sock.Available <= 0) continue;
                length = sock.ReceiveFrom(data, ref remoteEndPoint);//此方法把数据来源ip、port放到第二个参数中
                str= Encoding.UTF8.GetString(data,0,length);
                if(str.StartsWith("hosts")){
                    str = str.Substring(5);

                    hosts = getList();
                    GlobalData.form1.initListView(hosts);
                }
                // clear
                data = new byte[1024];
            }
        }

        
        public static List<string> getList()
        {
            while (str.Length == 0) ;
            return JsonConvert.DeserializeObject<List<string>>(str);
        }
    }
   
}

