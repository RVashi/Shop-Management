using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebAPI.Infrastructure
{
    public static class InfrastructureModule
    {
        public static string connectionString = String.Empty;
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public static string GetConnectionString()
        {
            var connstr = connectionString;
            return connstr;
        }
    }
}
