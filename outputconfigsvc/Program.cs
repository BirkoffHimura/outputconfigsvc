
using Azure.Identity;
using Microsoft.Extensions.Azure;
using outputconfigsvc;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var settings = builder.Configuration.GetSection("keyVault").Get<MyConfig>();
var keyVaultEndpoint = new Uri(settings.kv_uri);
var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("Program");


if (string.IsNullOrEmpty(settings.mi_id))
{
    // for system assigned identity we don't need to specify the ID
    System.Console.WriteLine("Targetting system managed identity");
    logger.LogInformation("Targetting system managed identity");
    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
} 
else
{
    // for user assigned identity we need to specify since azure resources support multiple identities
    System.Console.WriteLine("Targetting user managed identity: {0}", settings.mi_id);
    logger.LogInformation("Targetting user managed identity: {0}", settings.mi_id);
    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeEnvironmentCredential = true, ManagedIdentityClientId = settings.mi_id }));
}


// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
