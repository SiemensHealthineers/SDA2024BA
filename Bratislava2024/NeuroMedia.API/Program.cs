using System.Security.Cryptography;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using NeuroMedia.API.Policies;
using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Extensions;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Infrastructure.Blobstorages;
using NeuroMedia.Infrastructure.Logging;
using NeuroMedia.Persistence.Extensions;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = SerilogConfiguration.LoggerConfiguration(builder.Configuration, builder.Environment);
builder.Host.UseSerilog(logger);

builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddCors(policy => policy.AddPolicy("CorsPolicy", opt => opt
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuers = [builder.Configuration["Token:Issuer"], builder.Configuration["Token:IssuerMobile"]],
            ValidateIssuer = true
        };
    }, options => builder.Configuration.Bind("AzureAd", options));
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomRolePolicyProvider>();
builder.Services.AddTransient<IAuthorizationHandler, CustomRoleAuthorizationHandler>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

try
{
    logger.Information("Building web host");
    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsProduction())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseCors("CorsPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
