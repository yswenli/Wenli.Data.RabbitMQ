using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wenli.Data.RabbitMQ.Handler
{
    public delegate void ConnectionShutdown(ShutdownEventArgs args);

    public delegate void ConnectionBlocked(ConnectionBlockedEventArgs args);

    public delegate void ConnectionUnblocked(EventArgs args);

    public delegate void CallbackException(CallbackExceptionEventArgs args);

}
