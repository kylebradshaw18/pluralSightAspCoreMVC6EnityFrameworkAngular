using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using TheWorld.Models;
using Newtonsoft.Json.Serialization;
using TheWorld.ViewModels;
using AutoMapper;

namespace The_World
{
    public class Startup
    {

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Env;

        public Startup(IHostingEnvironment env)
        {
            Env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Env.ContentRootPath + "/Settings/")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            if (Env.IsEnvironment("Development") || Env.IsEnvironment("Testing"))
            {
                services.AddScoped<IMailService, DebugMailService>();
            }
            else
            {
                // Implement a real Mail Service
            }

			services.AddDbContext<WorldContext>();

			services.AddScoped<IWorldRepository, WorldRepository>();

			services.AddTransient<WorldContextSeedData>();

			services.AddLogging();

            // Add framework services.
            services.AddMvc()
				.AddJsonOptions(config =>
				{
					config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, WorldContextSeedData seeder)
        {
			//loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			Mapper.Initialize(config =>
			{
				config.CreateMap<TripViewModel, Trip>().ReverseMap();
			});

            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
				loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
				//Implement real mail service
				//app.UseExceptionHandler("/Home/Error");
				loggerFactory.AddDebug(LogLevel.Error);
			}

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
				);
            });

			seeder.EnsureSeedData().Wait();
        }
    }
}
