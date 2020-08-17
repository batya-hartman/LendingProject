using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lender.Data;
using Lender.Service;
using Lending.Services;
using Lendings.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lender.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ILenderService, LenderService>();
            services.AddScoped<ILenderRepository, LenderRepository>();
            services.AddScoped<ILendingService, LendingService>();
            services.AddScoped<ILendingRepository, LendingRepository>();
            services.AddDbContext<LenderContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("LenderConnection")));
            services.AddControllers();
           services.AddDbContext<LendingContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("LenderConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
