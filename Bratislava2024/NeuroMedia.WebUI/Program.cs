using NeuroMedia.WebUI;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NeuroMedia.Application.Common.Helpers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http;
using NeuroMedia.WebUI.Services.Interfaces;
using NeuroMedia.WebUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var uri = builder.HostEnvironment.IsDevelopment() ? builder.Configuration["ServerApi:UrlDevelopment"]! : builder.Configuration["ServerApi:Url"]!;
builder.Services.AddHttpClient("ServerAPI", client => client.BaseAddress = new Uri(uri))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://turituri866.onmicrosoft.com/30e5a9c9-b6fd-47e2-bdc2-b755aa3b222c/access_as_user");
});

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("IsAdmin", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var user = context.User;
            return user.IsInRoleCustomImplementation("Admin")
                || user.IsInRoleCustomImplementation("SystemManager");
        });
    });
});

await builder.Build().RunAsync();
