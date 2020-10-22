using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SWE1_MTCG.Server
{
    public class WebServer
    {
        private TcpListener _server;
        private IPAddress _ipAddress;
        private int _port;
        private byte[] _buffer;

        public WebServer()
        {
            throw new System.NotImplementedException();
        }

        public WebServer(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public WebServer(IPAddress ipAddress, int port)
        {
            throw new System.NotImplementedException();
        }

        private void InitializeServer()
        {
            throw new System.NotImplementedException();
        }

        public void StartServer()
        {
            throw new System.NotImplementedException();
        }

        private void InteractWithClient(object client)
        {
            throw new System.NotImplementedException();
        }
    }
}