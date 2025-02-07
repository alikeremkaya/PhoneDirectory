using Microsoft.Extensions.DependencyInjection;
using PhoneDirectory.Application.Services.CommunicationInfoServices;
using PhoneDirectory.Application.Services.PersonService;
using PhoneDirectory.Application.Services.PersonServices;

namespace PhoneDirectory.Application.Extentions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
       
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ICommunicationInfoService, CommunicationInfoService>();



        return services;
    }


}
