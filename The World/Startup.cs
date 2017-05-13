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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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

			services.AddIdentity<WorldUser, IdentityRole>(config =>
			{
				config.User.RequireUniqueEmail = true;
				config.Password.RequiredLength = 8;
				/*Default go to login when the user is not logged in*/
				config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
			})
				.AddEntityFrameworkStores<WorldContext>();

			services.AddDbContext<WorldContext>();

			services.AddScoped<IWorldRepository, WorldRepository>();

			services.AddTransient<GeoCoordsService>();

			services.AddTransient<WorldContextSeedData>();

			services.AddLogging();

            // Add framework services.
            services.AddMvc(config =>
				{
					if (Env.IsProduction()) //On production we want to secure user infromation
					{
						config.Filters.Add(new RequireHttpsAttribute());
					}
				}).AddJsonOptions(config =>
				{
					//When returning json we need to camel case the porperties for consistency
					config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, WorldContextSeedData seeder)
        {
			app.UseStaticFiles();
			//loggerFactory.AddConsole(Configuration.GetSection("Logging"));

			app.UseIdentity();

			Mapper.Initialize(config =>
			{
				config.CreateMap<TripViewModel, Trip>().ReverseMap();
				config.CreateMap<StopViewModel, Trip>().ReverseMap();
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
