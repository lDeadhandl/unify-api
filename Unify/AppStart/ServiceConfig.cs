using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unify.Service;

namespace Unify.AppStart
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("AppSettings");

            services.AddSingleton<SpotifyService>();
            services.Configure<SpotifyServiceOptions>(settings);

            return services;
        }
    }
}
