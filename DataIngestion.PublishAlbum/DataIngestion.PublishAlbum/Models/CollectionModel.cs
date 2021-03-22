using System;


namespace DataIngestion.PublishAlbum.Models
{
    public class CollectionModel
    {

        public long? Id { get; set; }
        public string Name { get; set; }
        public long? UPC { get; set; }
        public string URL { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool? IsCompilation { get; set; }
        public string Label { get; set; }
        public string ImageUrl { get; set; }
        public CollectionModel(long id, string name, long? upc, string url, DateTime? releaseDate, bool? isCompilation,
            string label, string imageUrl)
        {
            Id = id;
            Name = name;
            UPC = upc;
            URL = url;
            ReleaseDate = releaseDate;
            IsCompilation = isCompilation;
            Label = label;
            ImageUrl = imageUrl;
        }
        
    }
}
