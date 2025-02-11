﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Application.Interfaces.Services;
using Report.Infrastructure.AppContext;
using Report.Infrastructure.Repositories;
using Report.Infrastructure.Services;

namespace Report.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        services.AddDbContext<ReportDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(configuration.GetConnectionString("AppConnectionDev2"));
        });

       
        services.AddScoped<IReportRepository, ReportRepository>();
        
        
        services.AddHttpClient<IPhoneDirectoryService, PhoneDirectoryService>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:PhoneDirectory"]);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

      

        return services;
    }
}