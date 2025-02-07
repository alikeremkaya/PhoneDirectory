using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneDirectory.Infrastructure.AppContext;
using PhoneDirectory.Infrastructure.Repositories.CommunicationInfoRepositories;
using PhoneDirectory.Infrastructure.Repositories.PersonRepositories;

namespace PhoneDirectory.Infrastructure.Extentions
{
    public static class DependencyInjection
    {
       
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(configuration.GetConnectionString("AppConnectionDev"));
            });

            services.AddScoped<IPersonRepository, PersonRepository>();  
            services.AddScoped<ICommunicationInfoRepository, CommunicationInfoRepository>();




            return services;
        }
    }
}
