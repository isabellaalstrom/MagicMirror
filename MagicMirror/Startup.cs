using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MagicMirror.Data;
using MagicMirror.Models;
using MagicMirror.Services;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.AspNetCore.Sockets;

namespace MagicMirror
{
    public class Startup
    {
        public static ConnectionManager ConnectionManager;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddSignalR();
            services.AddTransient<MqttService>();
            services.AddTransient<TrafficService>();
            services.AddTransient<GCalendarService>();
            services.AddSingleton<IHassService, HassService>();
            services.AddSingleton<HttpClient>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            ConnectionManager = serviceProvider.GetService<ConnectionManager>();

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseSignalR(routes => // &lt;-- SignalR
            {
                routes.MapHub<SignalRHub>("signalRHub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
