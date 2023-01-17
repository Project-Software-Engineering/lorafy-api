using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AspNetCoreRateLimit;
using LorafyAPI.Services;
using LorafyAPI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCors();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnectionString");
var serverVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(connectionString, serverVersion));
builder.Services.AddScoped<EndDeviceService>();
builder.Services.AddScoped<JsonModelsParsingService>();
builder.Services.AddScoped<DataPointService>();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Start the MQTT background service in production mode
if (builder.Environment.EnvironmentName == "Production")
{
    builder.Services.AddHostedService<MQTTBackgroundService>();
}

builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

var app = builder.Build();
app.UseCors(options => options.WithOrigins("*").AllowAnyMethod());
app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
