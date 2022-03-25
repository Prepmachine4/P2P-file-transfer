using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2P_file_transfer
{
    public partial class Form2 : Form
    {
        private FileSend fileSend;
        private Thread sendThread;
        private string fileName;
        private string filePath;
        private long fileSize;
        public Form2()
        {
            InitializeComponent();
            textBox2.Text = GlobalData.selectIP;
            textBox3.Text = "4000";
        }

        /// <summary>
        /// 信息提示框
        /// </summary>
        /// <param name="msg"></param>
        public void Tip(string msg)
        {

            MessageBox.Show(msg, "温馨提示");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dig = new OpenFileDialog();

            dig.ShowDialog();

            //获取文件名
            this.fileName = dig.SafeFileName;

            //获取文件路径
            this.filePath = dig.FileName;

            FileInfo f = new FileInfo(this.filePath);

            //获取文件大小
            this.fileSize = f.Length;
            textBox1.Text = filePath;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string ip = textBox2.Text;
            string port = textBox3.Text;

            if (fileName.Length == 0)
            {

                Tip("请选择文件");
                return;
            }
            if (ip.Length == 0 || port.ToString().Length == 0)
            {

                Tip("端口和ip地址是必须的!");
                return;
            }

            fileSend = new FileSend(this, new string[] { ip, port, fileName, filePath, fileSize.ToString() });
            sendThread=new Thread(fileSend.Send);
            sendThread.Start();
        }



        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="value"></param>
        public void UpDateProgress(int value)
        {


            this.progressBar1.Value = value;
            this.label4.Text = value + "%";
            Application.DoEvents();
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="state"></param>
        public void SetState(string state)
        {

            label3.Text = state;
        }
        /// <summary>
        /// 退出程序
        /// </summary>
        public void Exit()
        {

            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sendThread != null)
            {
                fileSend.socket.Close();
                sendThread.Abort ();
            }
            
        }
    }
}
