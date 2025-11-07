using Asp.Versioning;
using EventosApi.Models;
using EventosApi.Services;
using EventosApi.Settings;
using EventosApi.ML;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks; // Necessário para HealthCheckResult
using Microsoft.AspNetCore.Diagnostics.HealthChecks; // Necessário para HealthCheckOptions
using EventosApi.Middleware; // Nosso Middleware de Tracing

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURAÇÃO DOS SERVIÇOS ---

// 1. Configurar o MongoDB
builder.Services.Configure<EventosDatabaseSettings>(
    builder.Configuration.GetSection("EventosDatabase"));

// 2. Registrar o Cliente MongoDB como Singleton (uma única instância)
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<EventosDatabaseSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// 3. Registrar nosso serviço
builder.Services.AddScoped<IEventoService, EventoService>();

// 4. Registrar os Controladores
builder.Services.AddControllers();

// 5. Configurar o Versionamento da API (CP06 - Parte 2)
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Formata para "v1", "v2", etc.
    options.SubstituteApiVersionInUrl = true;
});

// 6. REGISTRAR HEALTH CHECKS (CP06 - Parte 1)
builder.Services.AddHealthChecks()
    // Adiciona uma verificação de "saúde" da própria API
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "api" })

    // --- CORREÇÃO APLICADA AQUI ---
    // Dizemos ao Health Check para usar o IMongoClient que JÁ ESTÁ 
    // registrado no container de Injeção de Dependência.
    .AddMongoDb(
        sp => sp.GetRequiredService<IMongoClient>(), // Resolve o singleton
        name: "mongodb", // Nome da nossa verificação
        tags: new[] { "database" });

// 7. Configurar o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 8. REGISTRAR E TREINAR O SERVIÇO DE ML (CP06 - Parte 3)
builder.Services.AddSingleton<PrevisaoParticipantesService>(sp =>
{
    var service = new PrevisaoParticipantesService();
    service.TrainModel(); // Treina o modelo imediatamente na inicialização
    return service;
});

// --- FIM DA CONFIGURAÇÃO DOS SERVIÇOS ---


// --- 2. CONSTRUÇÃO E PIPELINE DA APLICAÇÃO ---

var app = builder.Build();

// REGISTRAR MIDDLEWARE DE TRACING (CP06 - Parte 1)
app.UseMiddleware<EventosApi.Middleware.TracingMiddleware>();

// Configure o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

// Mapeia os endpoints dos nossos controladores
app.MapControllers();

// Mapeia o endpoint /health (CP06 - Parte 1)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = WriteCustomHealthResponse
});

app.Run();

// --- FIM DO PIPELINE ---


// --- 3. FUNÇÕES AUXILIARES ---

static Task WriteCustomHealthResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";

    var apiStatus = report.Entries.ContainsKey("self") &&
                    report.Entries["self"].Status == HealthStatus.Healthy
                    ? "Healthy"
                    : "Unhealthy";

    var dbStatus = report.Entries.ContainsKey("mongodb") &&
                   report.Entries["mongodb"].Status == HealthStatus.Healthy
                   ? "Connected"
                   : "Disconnected";

    var result = new
    {
        apiStatus = apiStatus,
        databaseStatus = dbStatus
    };

    return context.Response.WriteAsJsonAsync(result);
}