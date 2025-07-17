using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Infrastructure.Persistance.Qdrant;
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
    MustBeSet(builder.Configuration.GetConnectionString(
        env == "Development" ? "QDrantLocal" : ""),
        nameof(qdrantConnectionString));

builder.Services.AddScoped<QdrantClient>(sp => new QdrantClient(qdrantConnectionString));
builder.Services.AddScoped<IVectorRepository, QDrantRepository>();

//TODO: make sure the correct connectionstring is selected based on env
builder.Services.AddTransient<IOllamaApiClient, OllamaApiClient>(sp => new OllamaApiClient("http://localhost:11434"));

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

string MustBeSet(string? str, string varName)
{
    if(string.IsNullOrWhiteSpace(str))
    {
        throw new InvalidOperationException($"{varName} must be set in appsettings.json");
    }

    return str;
}