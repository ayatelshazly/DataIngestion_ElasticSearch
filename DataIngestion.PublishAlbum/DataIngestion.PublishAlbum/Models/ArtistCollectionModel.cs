

namespace DataIngestion.PublishAlbum.Models
{
    public class ArtistCollectionModel
    {
        public long AritistId { get; set; }
        public long CollectionId { get; set; }

        public ArtistCollectionModel(long aritistId, long collectionId)
        {

            AritistId = aritistId;
            CollectionId = collectionId;
        }

    }
}
