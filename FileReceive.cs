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
    class FileRecive
    {

        public TcpListener server;
        private Form2 fm;
        public NetworkStream stream;
        public FileRecive()
        {
            this.fm = GlobalData.form2;
            try
            {
                this.server = new TcpListener(IPAddress.Parse(mainForm.GetLocalIp()), 4000);

                server.Start();
            }
            catch (Exception e)
            {

                fm.Tip(e.Message);

            }
            new Thread(run).Start();
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
                    TcpClient client = server.AcceptTcpClient();
                    this.stream = client.GetStream();
                    byte[] msgs = new byte[1024];

                    int len = this.stream.Read(msgs, 0, msgs.Length);

                    string msg = Encoding.UTF8.GetString(msgs, 0, len);
                    string[] tip = msg.Split('|');
                    if (DialogResult.Yes == MessageBox.Show(tip[0] + "给您发了一个文件:" + tip[1] + "大小为:" + (long.Parse(tip[2]) / 1024) + "kb ,确定要接收吗?", "接收提醒", MessageBoxButtons.YesNo))
                    {

                        //将接收信息反馈给发送方
                        msg = "1";
                        msgs = Encoding.UTF8.GetBytes(msg);
                        this.stream.Write(msgs, 0, msgs.Length);
                        this.stream.Flush();
                        fm.SetState("正在接收：");
                        //开始接收文件
                        string path = @"d:\p2p\" + tip[0];//接收文件的存储路径
                        FileStream os = new FileStream(path, FileMode.OpenOrCreate);

                        byte[] data = new byte[1024];
                        long currentprogress = 0;
                        int length = 0;
                        while ((length = this.stream.Read(data, 0, data.Length)) > 0)
                        {
                            currentprogress += length;
                            //更新进度条
                            fm.UpDateProgress((int)(currentprogress * 100 / long.Parse(tip[1])));
                            os.Write(data, 0, length);

                        }
                        os.Flush();
                        this.stream.Flush();
                        os.Close();
                        this.stream.Close();
                        fm.Tip("成功接收文件并存入了" + path + "中!");
                        fm.Exit();

                    }

                }
                catch (Exception e)
                {
                    fm.Tip(e.Message);

                }
            }
        }
    }
}
