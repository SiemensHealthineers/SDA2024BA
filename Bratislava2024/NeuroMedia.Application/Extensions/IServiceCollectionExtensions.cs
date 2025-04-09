using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using NeuroMedia.Application.Interfaces.Services;
using NeuroMedia.Application.Common.Services;
using NeuroMedia.Application.Features.Users.Services;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Microsoft.Graph;

namespace NeuroMedia.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomAutoMapper();
            services.AddMediator();
            services.AddGraphClient(configuration);
            services.AddServices();
        }

        private static void AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        private static void AddGraphClient(this IServiceCollection services, IConfiguration configuration)
        {
            var graphConfig = configuration.GetSection("AzureAd");
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var clientSecretCredential = new ClientSecretCredential(graphConfig["TenantId"], graphConfig["ClientId"], graphConfig["ClientSecret"]);

            services.AddScoped(sp => new GraphServiceClient(clientSecretCredential, scopes));
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IAzureUserService, AzureUserService>();
        }
    }
}
