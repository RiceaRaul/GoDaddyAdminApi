using ExternalServices.Implementations;
using ExternalServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices
{
    public static class ExternalServicesConfiguration
    {
        public static void RegisterDepedencies(IServiceCollection services)
        {
            services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
            services.AddScoped<IGoDaddyClient, GoDaddyClient>();
        }
    }
}