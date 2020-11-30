using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Npgsql;
using SWE1_MTCG.Database;
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
            Console.CancelKeyPress += (sender, e) =>
            {
                PostgreSQLSingleton.GetInstance.Connection.Close();
                Environment.Exit(0);
            };

            _server.Start();
            //TODO execute script that starts database automatically
            try
            {
                PostgreSQLSingleton.GetInstance.Connection.Open();
            }
            catch (NpgsqlException)
            {
                Console.WriteLine("No connection to database, this session will not be stored");
            }
            
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
            ResponseContext response;
            try
            {
                try
                {
                    string token =
                        request.Headers.ContainsKey("Authorization") &&
                        request.Headers["Authorization"].Contains("Basic")
                            ? request.Headers["Authorization"][6..]
                            : string.Empty;

                    response = ClientMapSingleton.GetInstance.ClientMap.ContainsKey(token)
                        ? ProcessKnownClientRequest(ClientMapSingleton.GetInstance.ClientMap[token], request)
                        : ProcessAnonymousClientRequest(request);
                }
                catch (KeyNotFoundException)
                {
                    response = new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.NotFound,
                        $"The requested URL {request.RequestedApi} was not found on this server."));
                }
                catch (NotImplementedException)
                {
                    response = new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.NotImplemented,
                        $"The requested service {Enum.GetName(typeof(HttpMethod), request.HttpMethod)} {request.RequestedApi} has not yet been implemented on this server."));
                }
                client.Client.Send(Encoding.ASCII.GetBytes(response.ToString()));
            }
            catch (Exception ex)
            {
                response = new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest,
                    $"The request could not been processed by the server."));
                client.Client.Send(Encoding.ASCII.GetBytes(response.ToString()));
                Console.WriteLine(ex.Message);
            }
            client.Close();
        }

        private ResponseContext ProcessAnonymousClientRequest(RequestContext request)
        {
            IRestApi restApi = _apiService.GetRequestedApi(request.RequestedApi);
            if (!restApi.AllowAnonymous)
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(
                    StatusCode.Unauthorized,
                    "You have to log in to use this service."));

            Dictionary<string, object> param = new Dictionary<string, object>(new[]
            {
                new KeyValuePair<string, object>("request", request)
            });

            return request.HttpMethod switch
            {
                HttpMethod.Get => restApi.Get(param),
                HttpMethod.Post => restApi.Post(param),
                HttpMethod.Put => restApi.Put(param),
                HttpMethod.Delete => restApi.Delete(param),
                HttpMethod.Unrecognized => new ResponseContext(request, new KeyValuePair<StatusCode, object>(
                    StatusCode.NotImplemented,
                    "The Server either does not recognize the request method, or it lacks the ability to fulfill the request."))
            };
        }

        private ResponseContext ProcessKnownClientRequest(MtcgClient client, RequestContext request)
        {
            IRestApi restApi = _apiService.GetRequestedApi(request.RequestedApi);
            Dictionary<string, object> param = new Dictionary<string, object>(new []
            {
                new KeyValuePair<string, object>("request", request),
                new KeyValuePair<string, object>("client", client)
            });
            return request.HttpMethod switch
            {
                HttpMethod.Get => restApi.Get(param),
                HttpMethod.Post => restApi.Post(param),
                HttpMethod.Put => restApi.Put(param),
                HttpMethod.Delete => restApi.Delete(param),
                HttpMethod.Unrecognized => new ResponseContext(request, new KeyValuePair<StatusCode, object>(
                    StatusCode.NotImplemented,
                    "The Server either does not recognize the request method, or it lacks the ability to fulfill the request."))
            };
        }
    }
}