

namespace DataIngestion.PublishAlbum.Models
{
    public class ArtistModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ArtistModel(long id, string name)
        {

            Name = name;
            Id = id;
        }
    }
}
