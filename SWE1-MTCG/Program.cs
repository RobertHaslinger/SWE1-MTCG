using System;
using System.Threading.Tasks;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiService apiService= new ApiService();
            WebServer server= new WebServer(10001, apiService);
            server.StartServer();
        }
    }
}
