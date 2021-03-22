using DataIngestion.PublishAlbum.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataIngestion.PublishAlbum.Infrastructure.Services
{
    public interface IPublishAlbumService
    {
        Task<List<CollectionModel>> ReadCollectionFileAsync(string filePath);
        Task<List<ArtistModel>> ReadArtistFileAsync(string artistFilePath);
        Task<AlbumModel> FeedAlbumAsync(long artistId, long collectionId);
        Task<List<ArtistCollectionModel>> ReadArtistCollectionFileAsync(string filePath);
        Task<List<ArtistCollectionModel>> ReadAllFilesAsync(string filePath);
        Task<List<CollectionMatchModel>> ReadCollectionMatchFileAsync(string collectionMatch);

    }
}
