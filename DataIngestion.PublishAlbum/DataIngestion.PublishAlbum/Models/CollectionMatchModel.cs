using System;

namespace DataIngestion.PublishAlbum.Models
{
    public class CollectionMatchModel
    {
        public long CollectionId { get; set; }
        public long? UPC { get; set; }

        public CollectionMatchModel(long collectionId, long? upc)
        {

            CollectionId = collectionId;
            UPC = upc;
        }
 
    }
}
