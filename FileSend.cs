using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2P_file_transfer
{
    internal class FileSend
    {
        public Socket socket;
        private string[] param;
        private Form2 fm;
        public FileSend(Form2 fm, params string[] param)
        {

            this.param = param;
            this.fm = fm;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse(param[0]); 
            IPEndPoint endpoint = new IPEndPoint(ipaddress, int.Parse(param[1]));

            socket.Connect(endpoint);
        }
        public void Send()
        {
            //0   1     2         3         4
            //ip, port, fileName, filePath, fileSize.ToString()
            try
            {
                
                string msg = param[0] + "|"+param[2] + "|" + param[4];
                byte[] m = Encoding.UTF8.GetBytes(msg);

                while (true)
                {
                    socket.Send(m,0,m.Length,0);
                    byte[] data = new byte[1024];
                    int len = socket.Receive(data);
                    msg = Encoding.UTF8.GetString(data, 0, len);
                    //对方要接收我发送的文件
                    if (msg.Equals("1"))
                    {

                        fm.SetState("正在发送：");
                        FileStream os = new FileStream(param[3], FileMode.OpenOrCreate);

                        data = new byte[1024];
                        //记录当前发送进度
                        long currentprogress = 0;
                        len = 0;
                        while ((len = os.Read(data, 0, data.Length)) > 0)
                        {
                            currentprogress += len;
                            //更新进度条
                            fm.UpDateProgress((int)(currentprogress * 100 / long.Parse(param[4])));
                            socket.Send(data, 0, len, 0);

                        }
                        
                        os.Close();
                        socket.Close();
                        MessageBox.Show("发送成功");
                        fm.Exit();
                        break;
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);

            }

        }
    }
}
