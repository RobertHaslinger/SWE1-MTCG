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

        public void StartServer()
        {
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            _server.Start();
            Console.WriteLine("Server is running on {0}:{1}", _ipAddress, _port);
            _buffer= new byte[1024];
            while (true)
            {
                try
                {
                    object client = _server.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(InteractWithClient, client);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private void InteractWithClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream networkStream = client.GetStream();
            RequestContext request;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                do
                {
                    int readCount = networkStream.Read(_buffer, 0, _buffer.Length);
                    memoryStream.Write(_buffer, 0, readCount);
                } while (networkStream.DataAvailable);
                request= new RequestContext(Encoding.ASCII.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length), _apiService);
            }
            IRestApi restApi = _apiService.GetRequestedApi(request.RequestedApi);
            ResponseContext response = request.HttpMethod switch
            {
                HttpMethod.Get => restApi.Get(request),
                HttpMethod.Post => restApi.Post(request),
                HttpMethod.Put => restApi.Put(request),
                HttpMethod.Delete => restApi.Delete(request)
            };

            //TODO if response is null, return 404

            using StreamWriter writer= new StreamWriter(networkStream) {AutoFlush = true};
            writer.Write(response);
        }
    }
}