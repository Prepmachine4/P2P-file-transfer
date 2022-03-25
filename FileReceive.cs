using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2P_file_transfer
{

    /// <summary>
    /// 文件接收类
    /// </summary>
    public class FileRecive
    {

        public Socket socket;
        private Form2 fm;
        public Thread thread;
        public FileRecive()
        {
            
            try
            {
                fm = GlobalData.form2;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipaddress = IPAddress.Parse(mainForm.GetLocalIp()); 
                IPEndPoint endpoint = new IPEndPoint(ipaddress, 4000);
                socket.Bind(endpoint);
                socket.Listen(1);
            }
            catch (Exception e)
            {

                fm.Tip(e.Message);

            }
            thread=new Thread(run);
            thread.Start();
        }

        /// <summary>
        /// 接收文件
        /// </summary>
        public void run()
        {

            while (true)
            {
                try
                {
                    Socket conn = socket.Accept();
                    byte[] msgs = new byte[1024];
                    int len = conn.Receive(msgs, 0, msgs.Length,0);
                    string msg = Encoding.UTF8.GetString(msgs, 0, len);
                    string[] tip = msg.Split('|');
                    if (DialogResult.Yes == MessageBox.Show(tip[0] + "给您发了一个文件:" + tip[1] + "大小为:" + (long.Parse(tip[2]) / 1024) + "kb ,确定要接收吗?", "接收提醒", MessageBoxButtons.YesNo))
                    {

                        //将接收信息反馈给发送方
                        msg = "1";
                        msgs = Encoding.UTF8.GetBytes(msg);
                        conn.Send(msgs, 0, msgs.Length,0);
                        //开始接收文件
                        string path = @"c:\" + tip[1];//接收文件的存储路径
                        FileStream os = new FileStream(path, FileMode.OpenOrCreate);

                        byte[] data = new byte[1024];
                        int length = 0;
                        while ((length = conn.Receive(data, 0, data.Length,0)) > 0)
                        {
                            os.Write(data, 0, length);

                        }
                        os.Flush();
                        os.Close();
                        conn.Close();
                        socket.Close();
                        fm.Tip("成功接收文件并存入了" + path + "中!");

                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());

                }
            }
        }
    }
}
