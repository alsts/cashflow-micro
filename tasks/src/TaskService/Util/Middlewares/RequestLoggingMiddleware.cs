using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace AccountService.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestLoggingMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        
        /**
         * Use LogRequest to get request details
         * LogResponse is minified by default
         * To get response body pass second parameter as true
         */
        public async Task Invoke(HttpContext context)
        {
            // await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var requestDetails = $"[{context.Request.Method}] - {context.Request.Host}{context.Request.Path} - [{context.Response.StatusCode}]";

            var requestBody = ReadStreamInChunks(requestStream);
            if (requestBody.Length > 0) {
              requestDetails += $"{Environment.NewLine}[Request Body] - {requestBody}";
            }

            _logger.LogInformation(requestDetails);
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                    0,
                    readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context, bool displayResponseBody = false)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseDetails = $"[{context.Request.Method}] - {context.Request.Host}{context.Request.Path} - [{context.Response.StatusCode}] ";
            if (displayResponseBody && text.Length > 0) {
                responseDetails += $"{Environment.NewLine}[Response Body] - {text}";
            }
            
            _logger.LogInformation(responseDetails);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
