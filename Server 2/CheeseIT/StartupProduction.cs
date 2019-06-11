using CheeseIT.BusinessLogic;
using CheeseIT.BusinessLogic.Interfaces;
using CheeseIT.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CheeseIT
{
    public class StartupProduction
    {
        public StartupProduction(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CheeseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IMobileMessagingService, MobileMessagingService>();
            services.AddScoped<IRipeningServices, RipeningServices>();
            services.AddScoped<IExperimentServices, ExperimentServices>();
            services.AddScoped<ICloudinaryServices, CloudinaryServices>();
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
