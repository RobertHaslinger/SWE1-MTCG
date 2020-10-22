using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG.Enums
{
    /// <summary>
    /// More info on http://webconcepts.info/concepts/http-status-code/
    /// </summary>
    public enum StatusCode
    {
        Continue = 100,
        OK = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        MultipleChoices = 300,
        Found = 301,
        Unused = 306,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        RequestTimeout = 408,
        InternalServerError = 500,
        NotImplemented = 501,
        ServiceUnavailable = 503
    }
}
