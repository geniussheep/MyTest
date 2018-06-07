using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Pipes;
using System.IO;
using System.Security.Principal;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form2_Load);
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }

        NamedPipeClientStream pipeClient =
                    new NamedPipeClientStream("127.0.0.1", "testpipe",
                        PipeDirection.InOut, PipeOptions.Asynchronous,
                        TokenImpersonationLevel.None);
        StreamWriter sw = null;
        void Form2_Load(object sender, EventArgs e)
        {
            pipeClient.Connect();
            sw = new StreamWriter(pipeClient);
            sw.AutoFlush = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            sw.WriteLine(textBox1.Text);
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sw.WriteLine("bye");
            pipeClient.Close();
        }
    }
}
