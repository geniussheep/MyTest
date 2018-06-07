using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Jillzhang.Messaging.Contract
{
    [ServiceContract(CallbackContract=typeof(ICallback))]
    public interface IJob
    {
        [OperationContract]
        string Do(string jobName);
    }
}
