using MediatR;
using System;

namespace DataIngestion.PublishAlbum.UseCases.Album
{
    public class PublishAlbumCommand : IRequest<Unit>
    {

        public readonly string _readingPath;
        public PublishAlbumCommand(string readingPath)
        {
            _readingPath = readingPath ?? throw new ArgumentNullException(nameof(readingPath));
        }
    }
}
