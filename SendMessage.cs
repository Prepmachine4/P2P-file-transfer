using System.Net;
using System.Net.Sockets;
using System.Text;


namespace P2P_file_transfer
{
    internal class SendMessage
    {
        static string ip = "192.168.43.239";
        public static Socket sock;
        static IPEndPoint iep;
        

        static SendMessage()
        {
            sock = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
            iep = new IPEndPoint(IPAddress.Parse(ip), 5000);
        }
        public static void send(string data)
        {
            sock.SendTo(Encoding.ASCII.GetBytes(data), iep);
        }
        
    }

}
