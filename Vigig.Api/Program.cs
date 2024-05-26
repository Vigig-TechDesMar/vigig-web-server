using System.Text.Json.Serialization;
using NLog;
using Vigig.Api.Extensions;
using Vigig.Api.Hubs;
using Vigig.Api.Hubs.Models;
using Vigig.Common.Constants;
using Vigig.Common.Helpers;


LogManager.Setup()
    .LoadConfigurationFromFile($"{Directory.GetCurrentDirectory()}\\Configurations\\nlog.config")
    .GetCurrentClassLogger();   
var builder = WebApplication.CreateBuilder(args);
DataAccessHelper.InitConfiguration(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.RegisterService();
builder.Services.AddSignalR();
builder.Services.AddSingleton<BookingConnectionPool>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(CorsConstant.APP_CORS_POLICY);

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<BookingHub>("/booking-hub");
app.MapHub<ChatHub>("/chat");
DataAccessHelper.EnsureMigrations(AppDomain.CurrentDomain.FriendlyName);

app.Run();
LogManager.Shutdown();
