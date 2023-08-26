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
        public static IServiceCollection UseSalsa20(this IServiceCollection services,string key,string iv,string hmac,int rounds = 20)
        {
            return services.AddSingleton<ISalsa20Service, Salsa20Service>(provider => new Salsa20Service(key,iv,hmac,rounds));
        }
    }
}