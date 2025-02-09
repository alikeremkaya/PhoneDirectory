using Microsoft.Extensions.DependencyInjection;
using Report.Application.Interfaces;
using Report.Application.Services;
namespace Report.Application.Extentions;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {



        services.AddScoped<IReportApplicationService, ReportApplicationService>();
       
        return services;
    }


}
