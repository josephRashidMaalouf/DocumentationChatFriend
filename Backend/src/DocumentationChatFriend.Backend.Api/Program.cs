using DocumentationChatFriend.Backend.Api.BackgroundServices;
using DocumentationChatFriend.Backend.Api.Configs;
using DocumentationChatFriend.Backend.Api.Helpers;
using DocumentationChatFriend.Backend.Api.Setup;
using DocumentationChatFriend.Backend.Application.Queues;
using DocumentationChatFriend.Backend.Application.Services;
using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Interfaces.Configs;
using DocumentationChatFriend.Backend.Infrastructure.Adapters;
using DocumentationChatFriend.Backend.Infrastructure.Persistance.Qdrant;
using DocumentationChatFriend.Backend.Infrastructure.TypedClients;
using OllamaSharp;
using Qdrant.Client;
using Serilog;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();


var qDrantConfigSection = builder.Configuration.GetSection("QDrantConfigs");
var qDrantHost = ConfigHelper.MustBeSet(
    env == "Docker" ? qDrantConfigSection["Docker"] : qDrantConfigSection["Localhost"],
    "QDrantConfigs:Hostname"
);
var qDrantPort = int.Parse(ConfigHelper.MustBeSet(qDrantConfigSection["Port"], "QDrantConfigs:Port"));
builder.Services.AddScoped<QdrantClient>(sp => new QdrantClient(qDrantHost, qDrantPort));
builder.Services.AddScoped<IVectorRepository, QDrantRepository>();

var ollamaSharpConnectionString = ConfigHelper.MustBeSet(
    env == "Docker"
        ? builder.Configuration.GetConnectionString("OllamaSharpDocker")
        : builder.Configuration.GetConnectionString("OllamaSharpLocal"),
    "ConnectionStrings:OllamaSharp"
);
builder.Services.AddTransient<IOllamaApiClient, OllamaApiClient>(sp => new OllamaApiClient(ollamaSharpConnectionString));
builder.Services.AddTransient<IOllamaClientConfigs, OllamaClientConfigs>();
builder.Services.AddTransient<IVectorRepositoryConfigs, VectorRepositoryConfigs>();

builder.Services.AddHttpClient<IChatAdapter, OllamaClient>();

builder.Services.AddScoped<IEmbeddingAdapter, OllamaEmbeddingAdapter>();
builder.Services.AddScoped<IRagService, RagService>();
builder.Services.AddScoped<ITextUploadService, TextUploadService>();
builder.Services.AddScoped<ICollectionManagementService, CollectionManagementService>();

builder.Services.Configure<OllamaModelConfigs>(builder.Configuration.GetSection(OllamaModelConfigs.Name));

builder.Services.AddSerilog(config =>
{
    config
        .MinimumLevel.Information()
        .WriteTo.Console();
});


builder.Services.AddSingleton<TextUploadQueue>();
builder.Services.AddHostedService<TextUploader>();


builder.Services.AddCors(x =>
{
    x.AddPolicy("allow-frontend", pb =>
    {
        var allowedOrigins =
            ConfigHelper.MustBeSet<string[]>(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>(),
                "AllowedOrigins");
        pb.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(allowedOrigins);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("allow-frontend");

app.UseAuthorization();

await app.UseOllama();

app.MapControllers();

app.Run();

