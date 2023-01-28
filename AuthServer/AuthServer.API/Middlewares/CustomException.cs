using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using System.Text.Json;

namespace AuthServer.API.Middlewares
{
    public static class CustomException
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature != null)
                    {
                        var exception = errorFeature.Error;
                        ErrorDto errorDto = null;

                        if (typeof(CustomException) == exception.GetType())
                        {
                            errorDto = new ErrorDto(exception.Message, true);
                        }
                        else
                        {
                            errorDto = new ErrorDto(exception.Message, false);
                        }

                        var responseModel = ResponseModel<NoDataDto>.Fail(errorDto, 500);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(responseModel));
                    }
                });
            });
        }
    }
}
