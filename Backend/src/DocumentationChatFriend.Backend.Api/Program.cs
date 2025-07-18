using DocumentationChatFriend.Backend.Api.Configs;
using DocumentationChatFriend.Backend.Api.Helpers;
using DocumentationChatFriend.Backend.Application.Services;
using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Infrastructure.Adapters;
using DocumentationChatFriend.Backend.Infrastructure.Persistance.Qdrant;
using DocumentationChatFriend.Backend.Infrastructure.TypedClients;
using OllamaSharp;
using Qdrant.Client;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//TODO: Adjust this when docker env is introduced
string qdrantConnectionString =
    ConfigHelper.MustBeSet(builder.Configuration.GetConnectionString(
        env == "Development" ? "QDrantLocal" : ""),
        nameof(qdrantConnectionString));

builder.Services.AddScoped<QdrantClient>(sp => new QdrantClient("localhost", 6334));
builder.Services.AddScoped<IVectorRepository, QDrantRepository>();

//TODO: make sure the correct connectionstring is selected based on env
builder.Services.AddTransient<IOllamaApiClient, OllamaApiClient>(sp => new OllamaApiClient("http://localhost:11434"));

builder.Services.AddHttpClient<IChatAdapter, OllamaClient>();
builder.Services.AddTransient<IOllamaClientConfigs, OllamaClientConfigs>();
builder.Services.AddScoped<IEmbeddingAdapter, NomicEmbeddingAdapter>();
builder.Services.AddScoped<IRagService, RagService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

