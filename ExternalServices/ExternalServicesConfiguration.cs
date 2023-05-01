using ExternalServices.Implementations;
using ExternalServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices
{
    public class ExternalServicesConfiguration
    {
        public static void RegisterDepedencies(IServiceCollection services)
        {
            services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
        }
    }
}