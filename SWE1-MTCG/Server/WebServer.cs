using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Server
{
    public class WebServer
    {
        private TcpListener _server;
        private IPAddress _ipAddress= IPAddress.Loopback;
        private int _port = 6543;
        private byte[] _buffer;
        private IApiService _apiService;

        public WebServer(IApiService apiService)
        {
            _apiService = apiService;
            InitializeServer();
        }

        public WebServer(int port, IApiService apiService)
        {
            _apiService = apiService;
            _port = port;
            InitializeServer();
        }

        public WebServer(IPAddress ipAddress, int port, IApiService apiService)
        {
            _apiService = apiService;
            _ipAddress = ipAddress;
            _port = port;
            InitializeServer();
        }

        private void InitializeServer()
        {
            try
            {
                _server = new TcpListener(_ipAddress, _port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void StartServer()
        {
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            _server.Start();
            _buffer= new byte[1024];
            while (true)
            {
                object client = await _server.AcceptTcpClientAsync();
                ThreadPool.QueueUserWorkItem(InteractWithClient, client);
            }
        }

        private void InteractWithClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            using StreamReader reader= new StreamReader(client.GetStream());
            RequestContext request= new RequestContext(reader.ReadToEnd(), _apiService);
            IRestApi restApi = _apiService.GetRequestedApi(request.RequestedResource);
            ResponseContext response = request.HttpMethod switch
            {
                HttpMethod.Get => restApi.Get(request),
                HttpMethod.Post => restApi.Post(request),
                HttpMethod.Put => restApi.Put(request),
                HttpMethod.Delete => restApi.Delete(request)
            };

            using StreamWriter writer= new StreamWriter(client.GetStream()) {AutoFlush = true};
            writer.Write(response);
        }
    }
}