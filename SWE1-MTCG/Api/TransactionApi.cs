using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class TransactionApi : IRestApi
    {
        public bool AllowAnonymous => false;
        public virtual ResponseContext Get(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public virtual ResponseContext Post(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public virtual ResponseContext Put(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public virtual ResponseContext Delete(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }
    }
}
