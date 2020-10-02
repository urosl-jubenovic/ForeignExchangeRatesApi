using System;
using System.Net;
using ForeignExchangeRates.Service.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ForeignExchangeRates.WebAPI.Extensions
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.UseCors("Default");
                appError.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    var causalException = GetException(exception);
                    var statusCode = GetStatusCode(causalException);

                    context.Response.StatusCode = (int)statusCode;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = causalException.Message
                        }
                            .ToString());
                    }
                });
            });
        }

        private static Exception GetException(Exception exception)
        {
            return exception.InnerException == null ? exception : GetException(exception.InnerException);
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception is InvalidClientRequestException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;
        }
    }
}