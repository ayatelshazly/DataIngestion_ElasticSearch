using System.Collections.Generic;

namespace DataIngestion.PublishAlbum.Models
{
    public class AlbumModel
    {
        public CollectionModel Collection { get; set; }
        public List<ArtistModel> Artist { get; set; }
        
    }
}
