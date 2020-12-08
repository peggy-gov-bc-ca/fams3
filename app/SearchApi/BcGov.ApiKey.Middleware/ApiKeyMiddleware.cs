﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BcGov.ApiKey.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        public static string CONFIG_APIKEY = "ApiKey";
        public static string HEADER_APIKEYNAME = "X-ApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (IsApiKeyRequired(context))
            {
                if (!context.Request.Headers.TryGetValue(HEADER_APIKEYNAME, out var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Api Key was not provided.");
                    return;
                }

                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

                var apiKey = appSettings.GetValue<string>(CONFIG_APIKEY);

                if (!apiKey.Equals(extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized client. ");
                    return;
                }
            }
            await _next(context);
        }

        private bool IsApiKeyRequired(HttpContext context)
        {
            if (context.Request.Path != null)
                return !context.Request.Path.Value.Contains("/swagger/");
            return true;
        }
    }
}

