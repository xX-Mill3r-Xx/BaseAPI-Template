using BaseTemplate.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace BaseTemplate.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/problem+json";

            var (statusCode, title, errors) = ex switch
            {
                NotFoundException notFoundEx => (
                    HttpStatusCode.NotFound,
                    notFoundEx.Message,
                    (IDictionary<string, string[]>?)null
                ),
                ValidationException validationEx => (
                    HttpStatusCode.BadRequest,
                    "Erro de validação.",
                    validationEx.Errors
                ),
                BusinessRuleException businessEx => (
                    HttpStatusCode.Conflict,
                    businessEx.Message,
                    (IDictionary<string, string[]>?)null
                ),
                ArgumentException argEx => (
                    HttpStatusCode.BadRequest,
                    argEx.Message,
                    null
                ),
                _ => (
                    HttpStatusCode.InternalServerError,
                    "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                    null
                )
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            else
                _logger.LogWarning(ex, "Erro de negócio: {Message}", ex.Message);

            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new
            {
                title,
                status = (int)statusCode,
                errors,
                traceId = context.TraceIdentifier
            };

            var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            await context.Response.WriteAsync(json);
        }
    }
}
