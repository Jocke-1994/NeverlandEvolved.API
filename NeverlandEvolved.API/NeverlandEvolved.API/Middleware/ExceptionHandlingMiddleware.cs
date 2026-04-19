using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace NeverlandEvolved.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // Standard 500
            var result = string.Empty;

            if (exception is ValidationException validationException)
            {
                code = HttpStatusCode.BadRequest; // 400 om det är valideringsfel

                var errors = validationException.Errors
                    .Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage });

                result = JsonSerializer.Serialize(new { message = "Valideringen misslyckades", errors });
            }
            else
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}