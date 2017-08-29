using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wenli.Data.RabbitMQ.Handler
{
    public delegate void ModelCallbackException(CallbackExceptionEventArgs args);

    public delegate void BasicReturn(BasicReturnEventArgs args);

}
