
using Microsoft.AspNetCore.Http;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.Core.Dto.Common;
using WebAPI.Core.Interfaces;
using WebAPI.Infrastructure.Common;

namespace WebAPI.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;
        public ExceptionMiddleware(RequestDelegate next, ILoggerService logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                switch (ex)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        if (ex.Message.Contains("IDX10223"))
                        {
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            // unhandled error
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        }
                        break;
                }
                /*Add user details to sentry error log*/
                SentrySdk.ConfigureScope(scope =>
                {
                    scope.SetExtra("userdata", httpContext.Request.Headers.ToArray()[3].Value[0].ToString().Substring(7));
                    if (((System.Security.Claims.ClaimsIdentity)httpContext.User.Identity).Claims.ToArray().Length > 0)
                    {
                        scope.User = new User
                        {
                            Email = ((System.Security.Claims.ClaimsIdentity)httpContext.User.Identity).Claims.ToArray()[0].Value
                        };
                    }
                });
                SentrySdk.CaptureException(ex);

                var responseModel = new CommonResponse() { success = false, responseMessage = ex?.Message, responseCode = response.StatusCode };
                var result = JsonSerializer.Serialize(responseModel);
                _logger.LogError($"Something went wrong: {ex}");

                await response.WriteAsync(result);
            }
        }
    }
}
