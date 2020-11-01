using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.Network.Client
{
    public delegate void ClientConnectedEventHandler(object Sender, ConnectionStatusEventArgs e);
    public delegate void ClientConnectingEventHandler(object Sender, ConnectionStatusEventArgs e);
    public delegate void ClientDisconnectedEventHandler(object Sender, ConnectionStatusEventArgs e);
    public delegate void DataReceivedEventHandler(object Sender, DataReceivedEventArgs e);
}

namespace RMS.Network.Server
{
    public delegate void DataReceivedEventHandler(object Sender, DataReceivedEventArgs e);
    public delegate void ClientConnectedEventHandler(object Sender, ClientConnectedEventArgs e);
    public delegate void ClientDisconnectedEventHandler(object Sender, ClientDisconnectedEventArgs e);
    public delegate void ServerErrorEventHandler(object sender, ServerErrorEventArgs e);
}

