﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Assets.Networking.Messaging.ConnectionHandlers
{
    public class ConnectionInitializationListener
    {
        private readonly TcpListener _tcpListener;
        public event EventHandler<Socket> NewSocketEvent;

        public ConnectionInitializationListener()
        {
            // Use any for the local address vs calling DNS GetHost Name
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=netframework-4.8
            _tcpListener = new TcpListener(IPAddress.Any, ServerClientInfo.PortNum);
            _tcpListener.Start();
            new Thread(ListenOnSeparateThread).Start();
        }

        private void ListenOnSeparateThread()
        {
            var socket = _tcpListener.AcceptSocket();
            // Note: The subscriptions to events will be ran on this listener thread
            // Hypothetically if something were to take a really long time we could be not listening for some time
            // And, this also allows a subscriber to cause race conditions
            NewSocketEvent?.Invoke(this, socket);
        }
    }
}
