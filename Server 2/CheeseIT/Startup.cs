using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseIT.BusinessLogic;
using CheeseIT.BusinessLogic.Interfaces;
using CheeseIT.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CheeseIT
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            services.AddDbContext<CheeseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IMobileMessagingService, MobileMessagingService>();
            
            services.AddScoped<IRipeningServices, RipeningServices>();
            services.AddScoped<IExperimentServices, ExperimentServices>();
            services.AddScoped<ICloudinaryServices, CloudinaryServices>();
            services.AddHostedService<FinishedCheeseNotifier>();

            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
