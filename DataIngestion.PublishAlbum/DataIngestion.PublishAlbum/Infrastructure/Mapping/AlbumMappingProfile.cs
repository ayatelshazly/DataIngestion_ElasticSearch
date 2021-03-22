using AutoMapper;
using Events;
using Events.Models.ReferenceModel;
 

namespace DataIngestion.PublishAlbum.Infrastructure.Mapping
{
    public class AlbumMappingProfile : Profile
    {
        public AlbumMappingProfile()
        {

            CreateMap<Models.CollectionModel, CollectionRefModel>();
            CreateMap<Models.ArtistModel, ArtistRefModel>();
            CreateMap<Models.AlbumModel, AlbumModelEvent>();
          

        }
    }
}
