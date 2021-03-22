using DataIngestion.SubscribeAlbum.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataIngestion.SubscribeAlbum.UseCases.Album
{
    public class InjectAlbumCommandHandler : IRequestHandler<InjectAlbumCommand, Unit>
    {
        private readonly IAlbumService _albumService;
     

        public InjectAlbumCommandHandler(IAlbumService albumService )
        {
            _albumService = albumService ?? throw new ArgumentNullException(nameof(albumService));
         }
        public async Task<Unit> Handle(InjectAlbumCommand request, CancellationToken cancellationToken)
        {
            await _albumService.InjectAlbumAsync(request.AlbumModel);

            return Unit.Value;

        }


    }
}
