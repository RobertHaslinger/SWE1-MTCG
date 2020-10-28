using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class MessageController
    {
        private IFileService _fileService;

        public MessageController(IFileService fileService)
        {
            _fileService = fileService;
        }

        private KeyValuePair<StatusCode, object> HandleFileException(Exception ex)
        {
            Console.WriteLine(ex.Message);
            switch (ex)
            {
                case UnauthorizedAccessException _:
                    return new KeyValuePair<StatusCode, object>(StatusCode.Unauthorized, "");
                case NotSupportedException _:
                    return new KeyValuePair<StatusCode, object>(StatusCode.NotImplemented, "");
                case ArgumentException _:
                    return new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "");
                case PathTooLongException _:
                case IOException _:
                    return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, "");
                default:
                    return new KeyValuePair<StatusCode, object>(StatusCode.NotFound, "");
            }
        }

        public KeyValuePair<StatusCode, object> ReadExistingFile(int id)
        {
            KeyValuePair<StatusCode, object> response;
            try
            {
                string content = _fileService.GetFileContent(id);
                if (string.IsNullOrWhiteSpace(content))
                {
                    response = new KeyValuePair<StatusCode, object>(StatusCode.NoContent, "");
                }
                else
                {
                    response = new KeyValuePair<StatusCode, object>(StatusCode.OK, content);
                }
            }
            catch (Exception ex)
            {
                response = HandleFileException(ex);
            }

            return response;
        }

        public KeyValuePair<StatusCode, object> ReadAllFiles()
        {
            KeyValuePair<StatusCode, object> response;
            try
            {
                List<string> contents = _fileService.GetAllFileContents();
                response= new KeyValuePair<StatusCode, object>(StatusCode.OK, string.Join("\r\n", contents.ToArray()));
            }
            catch (Exception ex)
            {
                response = HandleFileException(ex);
            }

            return response;
        }

        public KeyValuePair<StatusCode, object> AddNewFile(string content)
        {
            KeyValuePair<StatusCode, object> response;
            try
            {
                int id= _fileService.SaveInFile(content);
                response= new KeyValuePair<StatusCode, object>(StatusCode.Created, id);
            }
            catch (Exception ex)
            {
                response = HandleFileException(ex);
            }

            return response;
        }

        public KeyValuePair<StatusCode, object> EditFile(string content, int id)
        {
            KeyValuePair<StatusCode, object> response;
            try
            {
                _fileService.SaveInFile(content, id);
                response= new KeyValuePair<StatusCode, object>(StatusCode.Created, "");
            }
            catch (Exception ex)
            {
                response = HandleFileException(ex);
            }

            return response;
        }

        public KeyValuePair<StatusCode, object> DeleteFile(int id)
        {
            KeyValuePair<StatusCode, object> response;
            try
            {
                _fileService.DeleteFile(id);
                response = new KeyValuePair<StatusCode, object>(StatusCode.OK, "");
            }
            catch (Exception ex)
            {
                response = HandleFileException(ex);
            }

            return response;
        }
    }
}
