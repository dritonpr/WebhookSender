using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using WebhookSender.Data;
using WebhookSender.Extensions;
using WebhookSender.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebhookContext>(options =>
       options.UseSqlite(builder.Configuration.GetConnectionString("WebhookContext")));

builder.Services.Configure<WebhookPolicyOptions>(builder.Configuration.GetSection("WebhookPolicy"));

builder.Services.AddHttpClient("WebhookClient")
    .AddPolicyHandler((services, request) => Retry.GetRetryPolicy(services))
    .AddPolicyHandler((services, request) => Retry.GetCircuitBreakerPolicy(services));


builder.Services.AddControllers();
builder.Services.AddLogging();

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline..
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

