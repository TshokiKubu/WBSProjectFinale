using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.OpenApi.Models;
using Serilog;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WSB.API.Services;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore;
using WSB.API.Repository;
using WSB.API.Data;





namespace WSB.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<ExternalDataService, ExternalDataService>();

            // Add MVC services
            services.AddControllers();

            // Add Swagger documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeConnect", Version = "v1" });
            });         
           

            // Configuration - Redis
            IConfiguration configuration = Configuration.GetSection("RedisConfiguration");
            string redisConnectionString = configuration.GetValue<string>("ConnectionString");
            string redisInstanceName = configuration.GetValue<string>("InstanceName");

            // Redis caching setup
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = redisInstanceName;
            });


            services.AddSingleton<RedisCacheService>();
            services.AddSingleton<ExternalDataService>();

            // Add HttpClient for API calls
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Development-specific middleware
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeConnect v1"));
                app.UseSerilogRequestLogging(); // Log HTTP requests
            }

            // HTTPS redirection
            app.UseHttpsRedirection();

            // Routing
            app.UseRouting();

            // Authorization
            app.UseAuthorization();

            // IP Rate Limiting
           // app.UseIpRateLimiting();

            // Endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}