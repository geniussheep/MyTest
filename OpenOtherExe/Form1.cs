using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenOtherExe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            IEnumerable<string> list = new List<string>();

            //list.For();
            Process pro = Process.Start(@"D:\TestProjects\MyTest\Receive\bin\Debug\Receive.exe");
            //Thread.Sleep(1000);
            pro?.WaitForExit();
            MessageBox.Show("pro.WaitForExit()");

            pro?.Close();
            MessageBox.Show("pro.close()");
            pro?.Dispose();
            MessageBox.Show("pro.Dispose()");
        }
    }
}
