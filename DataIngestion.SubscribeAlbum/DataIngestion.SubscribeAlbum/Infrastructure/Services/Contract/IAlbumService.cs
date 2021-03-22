using Events;
using System.Threading.Tasks;

namespace DataIngestion.SubscribeAlbum.Services
{
    public interface IAlbumService
    {
        Task InjectAlbumAsync(AlbumModelEvent artistModel);

    }
}
