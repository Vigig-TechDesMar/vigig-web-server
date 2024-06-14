using System.Text.Json.Serialization;
using Hangfire;
using NLog;
using Swashbuckle.AspNetCore.SwaggerUI;
using Vigig.Api.Extensions;
using Vigig.Api.Hubs;
using Vigig.Api.Hubs.Models;
using Vigig.Common.Constants;
using Vigig.Common.Helpers;
using RecurringJobScheduler = Vigig.Api.BackgroundUtils.RecurringJobScheduler;


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
builder.Services.AddHangfire(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddSingleton<BookingConnectionPool>();
builder.Services.AddSingleton<ChatConnectionPool>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(opt => opt.DocExpansion(DocExpansion.None));

app.UseCors(CorsConstant.APP_CORS_POLICY);

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");

app.MapHub<BookingHub>("/booking-hub");
app.MapHub<ChatHub>("/chat-hub");
DataAccessHelper.EnsureMigrations(AppDomain.CurrentDomain.FriendlyName);

RecurringJobScheduler.ScheduleJobs();

app.Run();
LogManager.Shutdown();
