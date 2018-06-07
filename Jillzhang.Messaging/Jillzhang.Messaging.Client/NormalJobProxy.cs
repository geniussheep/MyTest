using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jillzhang.Messaging.Contract;

namespace Jillzhang.Messaging.Client
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class NormalJobClient : System.ServiceModel.ClientBase<INormalJob>, INormalJob
    {
        public NormalJobClient()
        {

        }
        public NormalJobClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public NormalJobClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public string Do(string jobName)
        {
            return base.Channel.Do(jobName);
        }
    }
}
