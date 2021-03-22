using Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;


namespace DataIngestion.SubscribeAlbum
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(
         this IServiceCollection services, IConfiguration configuration)
        {

            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }
      

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName, c => c
                                                    .Map<AlbumModelEvent>(m => m.AutoMap()));
                
        }
    }
}