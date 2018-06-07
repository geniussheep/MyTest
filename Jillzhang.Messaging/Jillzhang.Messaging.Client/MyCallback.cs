using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jillzhang.Messaging.Client
{
    public class MyCallback:Contract.ICallback
    {
        public void Done(int usedTime)
        {
            Console.WriteLine("服务端程序已经完成，用时"+usedTime+"毫秒，并且成功调用了回调函数");
        }
    }
}
