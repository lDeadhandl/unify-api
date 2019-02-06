using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unify.Data;

namespace Unify.AppStart
{
    public static class DataConfig
    {
        public static IServiceCollection AddDatabaseStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UnifyContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IApplicationBuilder UseDatabaseStorage(this IApplicationBuilder app)
        {


            return app;
        }
    }
}
