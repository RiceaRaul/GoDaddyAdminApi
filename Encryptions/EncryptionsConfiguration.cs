using Encryptions.Implementations;
using Encryptions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Encryptions
{
    public class EncryptionsConfiguration
    {
        public static void RegisterDepedencies(IServiceCollection services)
        {
            services.AddSingleton<Salsa20>();
        }
    }

    public static class Salsa20Extensions{
        public static IServiceCollection UseSalsa20(this IServiceCollection services,string key,string iv,int rounds = 8)
        {
            return services.AddScoped<ISalsa20Service, Salsa20Service>(provider => new Salsa20Service(key,iv,rounds));
        }
    }
}