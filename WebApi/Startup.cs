using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using WebApi.Data;
using WebApi.Data.Repo;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Middlewares;

namespace WebApi
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
            services.AddControllers();
            services.AddCors();
            services.AddDbContext<MyDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString(
                "HouseStoreDb")));
            services.AddScoped<IUnitOFWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.ConfigureExceptionHander(env);
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseCors(m => m.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
