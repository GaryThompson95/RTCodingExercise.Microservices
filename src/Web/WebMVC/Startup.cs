﻿using MassTransit;
using RabbitMQ.Client;
using WebMVC.Interfaces;
using WebMVC.Services;

namespace RTCodingExercise.WebMVC
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddMassTransit(x =>
            {
                //ADD CONSUMERS HERE
                x.AddRequestClient<BuyPlateMessage>();
                x.AddRequestClient<ReservePlateMessage>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["EventBusConnection"], "/", h =>
                    {
                        if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                        {
                            h.Username(Configuration["EventBusUserName"]);
                        }

                        if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                        {
                            h.Password(Configuration["EventBusPassword"]);
                        }
                    });

                    cfg.ConfigureEndpoints(context);
                    cfg.ExchangeType = ExchangeType.Fanout;
                });
            });

            services.AddMassTransitHostedService();

            services.AddHttpClient("CatalogAPI", client =>
            {
                client.BaseAddress = new Uri("http://catalog-api:80/");
            });
            services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();

            services.AddScoped<PlateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseStaticFiles();
            app.UseForwardedHeaders();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}
