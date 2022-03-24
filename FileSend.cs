using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2P_file_transfer
{
    internal class FileSend
    {
        public TcpClient client;
        public NetworkStream stream;
        private string[] param;
        private Form2 fm;
        public FileSend(Form2 fm, params string[] param)
        {

            this.param = param;
            this.fm = fm;
        }
        public void Send()
        {
            //0   1     2         3         4
            //ip, port, fileName, filePath, fileSize.ToString()
            try
            {
                //连接接收端
                this.client = new TcpClient(param[0], int.Parse(param[1]));
                string msg = param[0] + "|"+param[2] + "|" + param[4];
                byte[] m = Encoding.UTF8.GetBytes(msg);

                while (true)
                {
                    this.stream = this.client.GetStream();
                    this.stream.Write(m, 0, m.Length);
                    this.stream.Flush();
                    byte[] data = new byte[1024];
                    int len = this.stream.Read(data, 0, data.Length);
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
                            this.stream.Write(data, 0, len);

                        }
                        os.Flush();
                        this.stream.Flush();
                        os.Close();
                        this.stream.Close();
                        fm.Tip("发送成功!");
                        fm.Exit();
                    }
                }
            }
            catch (Exception e)
            {

                fm.Tip(e.Message);

            }

        }
    }
}
