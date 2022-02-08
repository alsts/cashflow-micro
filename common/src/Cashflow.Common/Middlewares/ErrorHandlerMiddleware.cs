using System;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Cashflow.Common.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                // set status code:
                if (exception is HttpStatusException statusException) {
                    context.Response.StatusCode = (int) statusException.Status;
                } else {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }

                await context.Response.WriteAsJsonAsync(new { error = exception.Message });
            }
        }
    }
}
