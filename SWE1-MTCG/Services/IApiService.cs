using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public interface IApiService
    {
        HttpMethod GetHttpMethod(string httpMethod);
        IRestApi GetRequestedApi(string requestedApi);
    }
}