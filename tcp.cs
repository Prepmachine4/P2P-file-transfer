using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2P_file_transfer
{
    internal class Tcp
    {
        string ip = "192.168.43.239";
        Socket socket_server;
        
        public Tcp()
        {
            Thread thread1 = new Thread(new ThreadStart(handler));  //Thread1是你新线程的函数
            thread1.Start();
            
        }
        private void handler()
        {
            socket_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress addr = IPAddress.Parse(ip);
            IPEndPoint endp = new IPEndPoint(addr, 22222);
            socket_server.Connect(endp);
            while (true)
            {
                byte[] buffer=new byte[1024];
                socket_server.Receive(buffer);
                string res = Encoding.Default.GetString(buffer);
                if (res.StartsWith("host_list"))
                {
                    Console.Write(res);
                }
                else if (res.StartsWith("host_list"))
                {

                }
            }
        }
    }
}
