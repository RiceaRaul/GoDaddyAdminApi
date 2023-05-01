using BusinessLayer.Implementation;
using BusinessLayer.Interfaces;
using DataAccessLayer;
using ExternalServices;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer
{
    public class BusinessLayerConfiguration
    {
        public static void RegisterDepedencies(IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthentificationService, AuthentificationService>();
            DataAccessLayerConfiguration.RegisterDepedencies(services);
            ExternalServicesConfiguration.RegisterDepedencies(services);
        }
    }
}