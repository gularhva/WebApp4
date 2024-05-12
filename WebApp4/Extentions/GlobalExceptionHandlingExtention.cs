using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Text.Json;
using WebApp4.Models;

namespace WebApp4.Extentions;

public static class GlobalExceptionHandlingExtention
{
    public static void ConfigureExtentionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(error =>
        {
            error.Run(async context =>
            {
                Console.WriteLine("Ishe dushdu!!!");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if(contextFeature != null)
                {
                    Log.Error($"Something went wrong:{contextFeature.Error}");
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = $"Internal server error:{contextFeature.Error}"
                    }));
                }
            });
        });
    }
}
