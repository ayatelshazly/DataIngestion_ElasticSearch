using Events;
using MediatR;


namespace DataIngestion.SubscribeAlbum.UseCases.Album
{
    public class InjectAlbumCommand : IRequest<Unit>
    {
        public AlbumModelEvent AlbumModel { get; }

        public InjectAlbumCommand(AlbumModelEvent albumModel)
        {
            AlbumModel = albumModel;
        }
    }
}