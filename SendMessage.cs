using System.Net;
using System.Net.Sockets;
using System.Text;


namespace P2P_file_transfer
{
    internal class SendMessage
    {
        
 
        
        public static void sendToTracker(string data)
        {
            string trackerIP = "192.168.43.239";
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(trackerIP), 5000);
            sock.SendTo(Encoding.ASCII.GetBytes(data), iep);
            sock.Close();
        }

        public static void sendTo(string data,string ip,int port)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), port);
            sock.SendTo(Encoding.ASCII.GetBytes(data), iep);
            sock.Close();
        }

    }

}
