using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DataIngestion.SubscribeAlbum.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using MediatR;
using Broker;
using System.Net.Http;
using DataIngestion.SubscribeAlbum;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DataIngestion.SubscribeAlbum
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
            services.AddHttpClient("sub").AddHeaderPropagation();

            services.Configure<ConfigurationSettings>(Configuration);

            services.AddScoped<IAlbumService, AlbumService>();

            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SubscribeAlbum Api", Version = "v1" });
            });


            services.AddElasticsearch(Configuration);
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
                var httpFactory = eb.GetRequiredService<IHttpClientFactory>().CreateClient("sub"); ;
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

         public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHeaderPropagation();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "SubscribeAlbum");
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
