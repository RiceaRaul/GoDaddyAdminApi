using Models.ErrorHandling;
using Newtonsoft.Json;
using System.Net;

namespace Asp_NET_Core_6_Template.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(ApiErrorException ex)
            {
                await HandlerExceptionAsync(context, ex);
            }
            catch(Exception ex)
            {
                await HandlerExceptionAsync(context, ex);
            }
        }

        public static Task HandlerExceptionAsync(HttpContext context, ApiErrorException exception)
        {
            var code = exception.statusCode;

            var result = JsonConvert.SerializeObject(new
            {
                message = exception.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }

        public static Task HandlerExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new
            {
                message = exception.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
