using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWare
{//Convension
    public class ExceptionMiddelware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddelware> _loggerFactory;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddelware(RequestDelegate next,HttpContext httpContextcontext, ILogger<ExceptionMiddelware>loggerFactory,IWebHostEnvironment env)
        {
            _next = next;
           _loggerFactory = loggerFactory;
            _env = env;
        }
        public async Task InvokAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);

            }
            catch (Exception ex)
            {
                _loggerFactory.LogError(ex.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var response = _env.IsDevelopment() ?
                    new APiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
                    new APiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await httpContext.Response.WriteAsync(json);
            }
        }
        ////factory Based
        //public  async Task InvokeAsync(HttpContext context, RequestDelegate _next)
        //{   try
        //    {
        //        await _next.Invoke(context);

        //    }
        //    catch (Exception ex)
        //    {
        //        _loggerFactory.LogError(ex.Message);
        //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        context.Response.ContentType = "application/json";
        //        var response = _env.IsDevelopment() ?
        //            new APiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
        //            new APiExceptionResponse((int)HttpStatusCode.InternalServerError);
        //        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        //        var json = JsonSerializer.Serialize(response, options);
        //        await context.Response.WriteAsync(json);
        //    }
        //}

    }
}

