using DataIngestion.PublishAlbum.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;

namespace DataIngestion.PublishAlbum.UnitTest
{
    public static class TestFactories
    {
        public static PublishAlbumService PublishAlbumServiceFactory()
        {
            var loggerMock = new Mock<ILogger<PublishAlbumService>>();
            return new PublishAlbumService(loggerMock.Object);

        }
        public static string validDataTestFilesPath = Path.GetFullPath(@"../../../ValidDataTestFiles/");
        public static string inValidDataTestFilesPath = Path.GetFullPath(@"../../../InvalidDataTestFiles/");

        
    }
}