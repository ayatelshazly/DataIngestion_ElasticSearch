using Events;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataIngestion.SubscribeAlbum.Services
{
    public class AlbumService : IAlbumService
    {
        private IElasticClient _elasticClient;

        public AlbumService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;

        }
        public List<AlbumModelEvent> GetResult()
        { 
                var response = _elasticClient.Search<AlbumModelEvent>();
                return response.Documents.ToList();
             
            
        }
        public async Task  InjectAlbumAsync(AlbumModelEvent albumModel)
        {
            await _elasticClient.IndexDocumentAsync(albumModel);
        }
    }
}
