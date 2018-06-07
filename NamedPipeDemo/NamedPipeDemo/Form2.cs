using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Security.Principal;
using System.IO;

namespace NamedPipeDemo
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form2_Load);
            this.FormClosed += new FormClosedEventHandler(Form2_FormClosed);
        }

        void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            pipeClient.Close();
        }
        NamedPipeClientStream pipeClient =
                    new NamedPipeClientStream("127.0.0.1", "testpipe",
                        PipeDirection.InOut, PipeOptions.None,
                        TokenImpersonationLevel.Impersonation);
        StreamWriter sw = null;
        void Form2_Load(object sender, EventArgs e)
        {
            pipeClient.Connect();
            sw = new StreamWriter(pipeClient);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sw.AutoFlush = true;
            sw.WriteLine(textBox1.Text);
            
        }
    }
}
