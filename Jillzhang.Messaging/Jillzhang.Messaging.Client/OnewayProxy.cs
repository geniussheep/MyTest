using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jillzhang.Messaging.Contract;

namespace Jillzhang.Messaging.Client
{   
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class OneWayJobClient : System.ServiceModel.ClientBase<IOneWayJob>, IOneWayJob
    {

        public OneWayJobClient()
        {
        }

        public OneWayJobClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public OneWayJobClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public OneWayJobClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public OneWayJobClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public void Do(string jobName)
        {
            base.Channel.Do(jobName);
        }
    }   
}
