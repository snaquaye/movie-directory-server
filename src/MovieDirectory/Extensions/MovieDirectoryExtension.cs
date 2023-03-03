using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MovieDirectory.Application.Services;
using MovieDirectory.Core.Services;
using MovieDirectory.Infrastructure.Data;

namespace MovieDirectory.Extensions
{
    public static class MovieDirectoryExtension
    {
        public static IServiceCollection AddMovieDirectoryService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();
            services.AddScoped<IOMDbClient, OMDbClient>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(configuration.GetConnectionString("Default")));
            services.AddHttpClient<IOMDbClient, OMDbClient>(builder => builder.BaseAddress = new Uri($"http://www.omdbapi.com"));


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
