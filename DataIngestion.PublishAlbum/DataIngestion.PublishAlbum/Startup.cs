using Broker;
using DataIngestion.PublishAlbum.Infrastructure;
using DataIngestion.PublishAlbum.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Net.Http;
using MediatR;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DataIngestion.PublishAlbum
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
            services.AddControllers().AddDapr();
            services.AddHttpClient("pub").AddHeaderPropagation();
            services.AddAutoMapper(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);

            services.Configure<ConfigurationSettings>(Configuration);
 
            services.AddScoped<IPublishAlbumService, PublishAlbumService>();
            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PublishAlbum Api", Version = "v1" });
            });
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var allowedCors = Configuration.GetSection("AllowedCORS").GetChildren().AsEnumerable().Select(x => x.Value).ToArray();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                builder =>
                {
                    builder.WithOrigins(allowedCors)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
            services.AddScoped<IEventBus, EventBus>(eb =>
            {
                var settings = eb.GetRequiredService<IOptions<ConfigurationSettings>>().Value;
                var httpFactory = eb.GetRequiredService<IHttpClientFactory>().CreateClient("pub"); ;
                var bus = new EventBus(httpFactory)
                {
                    DaprPort = settings.DaprPort,
                    EventsOn = settings.EventsOn,
                    Broker = settings.BrokerName
                };
                return bus;
            });
           
            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddHeaderPropagation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHeaderPropagation();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "PublishAlbum");
            });
            app.UseCors("AllowOrigin");

            app.UseCloudEvents();
            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSubscribeHandler();

            });


        }
    }
}
