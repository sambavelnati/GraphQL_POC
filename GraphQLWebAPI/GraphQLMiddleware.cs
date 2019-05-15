using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using GraphQL.Types;

namespace GraphQLWebAPI
{
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDocumentWriter _writer;
        private readonly IDocumentExecuter _executor;
        private readonly ISchema _schema;

        public GraphQLMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor, ISchema schema)
        {
            _next = next;
            _writer = writer;
            _executor = executor;
            _schema = schema;
        }

        [Obsolete]
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments("/api/graphql") && string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                string body;
                using (var streamReader = new StreamReader(httpContext.Request.Body))
                {
                    body = await streamReader.ReadToEndAsync();

                    var request = JsonConvert.DeserializeObject<GraphQLRequest>(body);

                    var result = await _executor.ExecuteAsync(doc =>
                    {
                        doc.Schema = _schema;
                        doc.Query = request.Query;
                    }).ConfigureAwait(false);

                    var json = _writer.WriteToStringAsync(result).Result;
                    await httpContext.Response.WriteAsync(json);
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
