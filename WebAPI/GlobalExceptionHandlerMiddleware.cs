using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System;
using System.Threading.Tasks;

namespace LearnWebAPI.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException nEx)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(nEx.Message);
            }
            catch (Exception ex)
            {

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(ex.Message);

            }
        }
    }
}
