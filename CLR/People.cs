using System;

namespace ConsoleApp
{
    internal class People
    {
        public event EventHandler PropertyChanged;
        private string name;

        public People(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.OnPropertyChanged(new EventArgs());
                //每次改变Name值调用方法;                         
            }
        }

        private void OnPropertyChanged(EventArgs eventArgs)
        {
            if (this.PropertyChanged != null)
            //判断事件是否有处理函数              
            {
                this.PropertyChanged(this, eventArgs);
            }
        }
    }

    public partial class Program
    {
        static void MainPeopleTest(string[] args)
        {
            People p = new People("Name1");
            p.PropertyChanged += new EventHandler(p_PropertyChanged);
            //注册事件处理函数
            p.Name = "Name2";
            Console.ReadKey();
        }

        static void p_PropertyChanged(object sender, EventArgs e)
        //事件的处理函数                
        {
            Console.WriteLine("NamePropertyChanged:" + (sender as People).Name);
        } 
    }
}
