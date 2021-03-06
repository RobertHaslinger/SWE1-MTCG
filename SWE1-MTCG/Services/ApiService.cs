﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using SWE1_MTCG.Api;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public class ApiService : IApiService
    {

        private readonly Dictionary<string, Func<IRestApi>> _restApiMap = new Dictionary<string, Func<IRestApi>>()
        {
            {"/users", () => new UserApi()},
            {"/cards", () => new CardApi()},
            {"/deck", () => new DeckApi()},
            {"/battles", () => new BattleApi()},
            {"/transactions", () => new TransactionApi()},
            {"/transactions/packages", () => new TransactionPackagesApi()},
            {"/packages", () => new PackageApi()},
            {"/packages/open", () => new PackagesOpenApi()},
            {"/score", () => new RankingApi()},
            {"/stats", () => new StatisticApi()},
            {"/sessions", () => new SessionApi()},
            {"/messages", () => new MessageApi()},
            {"/tradings", () => new TradeApi()}
        };

        private readonly Dictionary<string, HttpMethod> _httpMethodMap = new Dictionary<string, HttpMethod>()
        {
            {"GET", HttpMethod.Get},
            {"POST", HttpMethod.Post},
            {"PUT", HttpMethod.Put},
            {"DELETE", HttpMethod.Delete}
        };

        public HttpMethod GetHttpMethod(string httpMethod)
        {
            if (_httpMethodMap.TryGetValue(httpMethod, out HttpMethod method)) return method;

            throw new KeyNotFoundException($"No Http Method found for key: {httpMethod}");

        }

        public IRestApi GetRequestedApi(string requestedApi)
        {
            requestedApi = requestedApi.TrimEnd('/');
            if (_restApiMap.ContainsKey(requestedApi))
            {
                return _restApiMap[requestedApi].Invoke();
            }

            throw new KeyNotFoundException($"No REST Api found for key: {requestedApi}");
        }
    }
}
