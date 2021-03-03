using System;
using System.Net;
using System.Threading.Tasks;
using hobbie.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace hobbie.Middlewares
{
    public class JsonResponseMiddleware
    {
        private readonly RequestDelegate _next;
        public JsonResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);
            if (httpContext.Response.StatusCode == 404)
            {
                await HandleExceptionNotFound(httpContext);
                return;
            }

        }

        private async Task HandleExceptionNotFound(HttpContext context)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var status = (int)HttpStatusCode.NotFound;
            response.StatusCode = status;
            await response.WriteAsync(JsonConvert.SerializeObject(JsonResponse.failed(message: "Request not found")));
        }

    }

    public static class JsonResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseJsonResponse(this IApplicationBuilder builder)
        {
            builder.UseWhen((context) => context.Request.Path.StartsWithSegments("/api"), (b) => b.UseMiddleware<JsonResponseMiddleware>());
            return builder;
        }
    }
}
