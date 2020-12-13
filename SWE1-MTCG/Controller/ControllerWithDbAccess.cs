using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Controller
{
    public abstract class ControllerWithDbAccess
    {
        /// <summary>
        /// PostgreSQL Error Codes: https://www.postgresql.org/docs/current/errcodes-appendix.html
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected KeyValuePair<StatusCode, object> HandleException(Exception ex)
        {
            Console.WriteLine(ex.Message);

            if (ex is NpgsqlException)
            {
                return ((NpgsqlException)ex).SqlState switch
                {
                    { } state when state.StartsWith("02") => new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, null),
                    { } state when state.StartsWith("08") => new KeyValuePair<StatusCode, object>(StatusCode.NotFound, null),
                    { } state when state.StartsWith("0A") => new KeyValuePair<StatusCode, object>(StatusCode.NotImplemented, null),
                    { } state when state.StartsWith("23") => new KeyValuePair<StatusCode, object>(StatusCode.Conflict, "This item may exist already"),
                    _ => new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, null)
                };
            }

            if (ex is JsonException)
            {
                return new KeyValuePair<StatusCode, object>(StatusCode.NoContent, "There is nothing here...");
            }

            return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, null);
        }
    }
}
