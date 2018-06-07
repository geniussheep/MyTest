using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Jillzhang.Messaging.Contract
{
    [ServiceContract]
    public interface INormalJob
    {
        [OperationContract]
        string Do(string jobName);
    }
}
