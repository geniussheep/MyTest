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
using System.Threading;

namespace NamedPipeDemo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, 4, PipeTransmissionMode.Message, PipeOptions.Asynchronous);

        void Form1_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
               {
                   pipeServer.BeginWaitForConnection((o) =>
                   {
                       NamedPipeServerStream server = (NamedPipeServerStream)o.AsyncState;
                       server.EndWaitForConnection(o);
                       StreamReader sr = new StreamReader(server);
                       StreamWriter sw = new StreamWriter(server);
                       string result = null;
                       string clientName = server.GetImpersonationUserName();
                       while (true)
                       {
                           result = sr.ReadLine();
                           if (result == null || result == "bye")
                               break;
                           this.Invoke((MethodInvoker)delegate { lsbMsg.Items.Add(clientName+" : "+result); });
                       }
                   }, pipeServer);
               });
        }
    }

}
