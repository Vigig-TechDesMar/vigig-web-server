using NLog;
using Vigig.Api.Extensions;
using Vigig.Common.Helpers;


LogManager.Setup()
    .LoadConfigurationFromFile($"{Directory.GetCurrentDirectory()}\\Configurations\\nlog.config")
    .GetCurrentClassLogger();   
var builder = WebApplication.CreateBuilder(args);
DataAccessHelper.InitConfiguration(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.RegisterService();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
DataAccessHelper.EnsureMigrations(AppDomain.CurrentDomain.FriendlyName);

app.Run();
LogManager.Shutdown();
