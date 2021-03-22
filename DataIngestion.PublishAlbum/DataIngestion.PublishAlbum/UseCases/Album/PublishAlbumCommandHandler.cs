using Broker;
using Events;
using DataIngestion.PublishAlbum.Infrastructure.Services;
using DataIngestion.PublishAlbum.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using AutoMapper;

namespace DataIngestion.PublishAlbum.UseCases.Album
{
    public class PublishAlbumCommandHandler : IRequestHandler<PublishAlbumCommand, Unit>
    {
        private readonly IPublishAlbumService _publishAlbumService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<PublishAlbumCommandHandler> _logger;
        private readonly IMapper _mapper;

        public PublishAlbumCommandHandler(IPublishAlbumService publishAlbumService,
         IEventBus bus, IMapper mapper, ILogger<PublishAlbumCommandHandler> logger)
        {
            _publishAlbumService = publishAlbumService ?? throw new ArgumentNullException(nameof(publishAlbumService));
            _eventBus = bus ?? throw new ArgumentNullException(nameof(bus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<Unit> Handle(PublishAlbumCommand request, CancellationToken cancellationToken)
        {
            await ReadAndPublishAlbum(request._readingPath);
            _logger.LogInformation("Album is published successfully.");
            return Unit.Value;

        }
        private async Task ReadAndPublishAlbum(string _readingPath)
        {
            AlbumModel albumModel = new AlbumModel();
            List<ArtistCollectionModel> artistCollectionModels = await _publishAlbumService.ReadAllFilesAsync(_readingPath);

            foreach (ArtistCollectionModel artistcollectionModel in artistCollectionModels)
            {

                albumModel = await _publishAlbumService.FeedAlbumAsync(artistcollectionModel.AritistId, artistcollectionModel.CollectionId);
                if (albumModel.Collection == null)
                    continue;

                var albumModelEvent = _mapper.Map<AlbumModelEvent>(albumModel);
                var evt = new Event().CreateEvent(data: albumModelEvent);
                await _eventBus.Publish(evt, EventNames.PUBLISHALBUM);
            }
        }
    }
}
