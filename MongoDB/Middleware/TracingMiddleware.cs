using System;
using System.Threading.Tasks;

namespace EventosApi.Middleware
{
    public class TracingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TracingMiddleware> _logger;

        public TracingMiddleware(RequestDelegate next, ILogger<TracingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Tenta obter um TraceId existente (se vier de outro serviço)
            var traceId = context.Request.Headers["X-Trace-Id"].FirstOrDefault()
                          ?? Guid.NewGuid().ToString();

            // Adiciona o TraceId ao log para esta requisição
            using (_logger.BeginScope("TraceId: {TraceId}", traceId))
            {
                // Loga a requisição (CP06 - Parte 1)
                _logger.LogInformation("Iniciando requisição: {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                // Adiciona o TraceId à resposta para o cliente poder ver
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Append("X-Trace-Id", traceId);
                    return Task.CompletedTask;
                });

                // Continua o pipeline para o próximo middleware/controller
                await _next(context);

                // Loga a finalização da requisição
                _logger.LogInformation("Finalizando requisição: {Method} {Path} com Status {StatusCode}",
                    context.Request.Method, context.Request.Path, context.Response.StatusCode);
            }
        }
    }
}