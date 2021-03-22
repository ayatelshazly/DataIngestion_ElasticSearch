using DataIngestion.PublishAlbum.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DataIngestion.PublishAlbum.UnitTest
{
    public class ReadAlbumTest
    {
        [Theory]
        [InlineData("../TestFiles/artist.txt")]
        public async Task ReadArtistFile_InvalidPath_ThrowDirectoryNotFoundException(string artistPath)
        {
            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();           
            Task readArtistFile() => publishAlbumService.ReadArtistFileAsync(artistPath);

            // Assert
            var directoryNotFoundException = await Assert.ThrowsAsync<DirectoryNotFoundException>(readArtistFile);
          
         }

        [Theory]
        [InlineData("../TestFiles/collection.txt")]
        public async Task ReadCollectionFile_InvalidPath_ThrowDirectoryNotFoundException(string collectionPath)
        {
            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();
            Task readCollectionFile() => publishAlbumService.ReadCollectionFileAsync(collectionPath);

            // Assert
            var directoryNotFoundException = await Assert.ThrowsAsync<DirectoryNotFoundException>(readCollectionFile);

        }

        [Theory]
        [InlineData("../TestFiles/artist_collection.txt")]
        public async Task ReadArtistCollectionFile_InvalidPath_ThrowDirectoryNotFoundException(string artistCollectionPath)
        {
            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();
            Task readArtistCollectionFile() => publishAlbumService.ReadArtistCollectionFileAsync(artistCollectionPath);

            // Assert
            var directoryNotFoundException = await Assert.ThrowsAsync<DirectoryNotFoundException>(readArtistCollectionFile);

        }


        [Fact]
        public async Task ReadArtistCollectionFile_ValidData_ReturnArtistCollectionList()
        {
            // Arrange
            string artistCollectionPath = TestFactories.validDataTestFilesPath;

            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();
             List<ArtistCollectionModel> artistCollectionModels =await publishAlbumService.ReadArtistCollectionFileAsync(artistCollectionPath);

             // Assert
            Assert.NotNull(artistCollectionModels);
            Assert.Equal(artistCollectionModels.FirstOrDefault().AritistId, 10459);
            Assert.Equal(artistCollectionModels.Count(), 19);

        }

        [Fact]
        public async Task ReadCollectionFile_ValidData_ReturnCollectionsList()
        {
            // Arrange
            string collectionPath = TestFactories.validDataTestFilesPath;

            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();
            List<CollectionModel> collectionModels = await publishAlbumService.ReadCollectionFileAsync(collectionPath);

            // Assert
            Assert.NotNull(collectionModels);
            Assert.Equal(collectionModels.FirstOrDefault().Name, "Ранила - Single");
            Assert.Equal(collectionModels.Count(), 10);
        }

        [Fact]
        public async Task ReadArtistFile_ValidData_ReturnArtistsList()
        {
            // Arrange
            string artistPath = TestFactories.validDataTestFilesPath;

            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();
            List<ArtistModel> artistModels = await publishAlbumService.ReadArtistFileAsync(artistPath);

            // Assert
            Assert.NotNull(artistModels);
            Assert.Equal(artistModels.FirstOrDefault().Name, "Daniel Johnston");
            Assert.Equal(artistModels.Count(), 6);

        }

        [Fact]
        public async Task ReadArtistFile_InvalidData_ThrowArgumentException()
        {
            // Arrange
            string artistPath = TestFactories.inValidDataTestFilesPath;

            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();
            Task readArtistFile() =>   publishAlbumService.ReadArtistFileAsync(artistPath);

            // Assert
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(readArtistFile);
        }

        [Fact]
        public async Task ReadCollectionFile_InvalidData_ThrowArgumentException()
        {
            // Arrange
            string collectionPath = TestFactories.inValidDataTestFilesPath;

            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();
            Task readCollectionFile() => publishAlbumService.ReadCollectionFileAsync(collectionPath);

            // Assert
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(readCollectionFile);
        }

        [Fact]
        public async Task ReadArtistCollection_InvalidData_ThrowArgumentException()
        {
            // Arrange
            string artistCollectionPath = TestFactories.inValidDataTestFilesPath;

            // Act
            var publishAlbumService = TestFactories.PublishAlbumServiceFactory();
            Task readArtistCollectionFile() => publishAlbumService.ReadArtistFileAsync(artistCollectionPath);

            // Assert
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(readArtistCollectionFile);
        }
    }
}
