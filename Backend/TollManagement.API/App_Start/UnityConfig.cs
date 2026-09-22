using Microsoft.Extensions.DependencyInjection;
using TollManagement.Business.Interfaces;
using TollManagement.Business.Services;
using TollManagement.Data;
using TollManagement.Data.Interfaces;
using TollManagement.Data.Repositories;

namespace TollManagement.API.App_Start;

public static class UnityConfig
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<DbConnection>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleService, VehicleService>();
    }
}