﻿using System.Net;

using TestProject.Domain.Responses;

namespace Tabloid.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorResponse()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StackTrace = exception.StackTrace,
                Message = exception.Message
            }, typeof(ErrorResponse));
        }
    }
}
