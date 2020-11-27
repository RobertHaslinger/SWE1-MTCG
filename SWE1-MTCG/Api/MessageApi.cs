using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class MessageApi : IRestApi
    {
        private MessageController _messageController;

        public bool AllowAnonymous => true;

        public MessageApi()
        {
            IFileService fileService = new FileService();
            _messageController = new MessageController(fileService);
        }

        public ResponseContext Get(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext) param["request"];

            var responsePair = string.IsNullOrWhiteSpace(request.RequestedResource)
                ? _messageController.ReadAllFiles()
                : _messageController.ReadExistingFile(Convert.ToInt32(request.RequestedResource));

            return new ResponseContext(request, responsePair);
        }

        public ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            var responsePair = _messageController.AddNewFile(request.Payload);

            return new ResponseContext(request, responsePair);
        }

        public ResponseContext Put(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            var responsePair = _messageController.EditFile(request.Payload, Convert.ToInt32(request.RequestedResource));

            return new ResponseContext(request, responsePair);
        }

        public ResponseContext Delete(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            var responsePair = _messageController.DeleteFile(Convert.ToInt32(request.RequestedResource));

            return new ResponseContext(request, responsePair);
        }
    }
}
