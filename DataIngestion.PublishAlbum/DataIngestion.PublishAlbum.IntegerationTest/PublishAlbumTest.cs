using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace DataIngestion.PublishAlbum.IntegerationTest
{
    public class PublishAlbumTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public PublishAlbumTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task PublishAlbum_ValidData_ReturnNoContent()
        {
            // Arrange
            var client = _factory.ConfigureTest().CreateClient();
            string readPath = _factory.validDataTestFilesPath;

            var payload = JsonConvert.SerializeObject(readPath);
            var content = new StringContent(payload, encoding: UTF8Encoding.UTF8, "application/json");
            var url = $"api/PublishAlbum?readingPath={readPath}";

            // Act
            var response = await client.PostAsync(url, content);

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        [Fact]
        public async Task PublishAlbum_InValidData_ReturnThrowArgumentException()
        {
            // Arrange
            var client = _factory.ConfigureTest().CreateClient();
            string readPath = _factory.inValidDataTestFilesPath;

            var payload = JsonConvert.SerializeObject(readPath);
            var content = new StringContent(payload, encoding: UTF8Encoding.UTF8, "application/json");
            var url = $"api/PublishAlbum?readingPath={readPath}";

            // Act
            Task publishFile() => client.PostAsync(url, content);

            // Assert
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(publishFile);
          
        }

        [Fact]
        public async Task PublishAlbum_InValidPath_ReturnDirectoryNotFoundException()
        {
            // Arrange
            var client = _factory.ConfigureTest().CreateClient();
            string readPath = "../TestFiles/";

            var payload = JsonConvert.SerializeObject(readPath);
            var content = new StringContent(payload, encoding: UTF8Encoding.UTF8, "application/json");
            var url = $"api/PublishAlbum?readingPath={readPath}";

            // Act
            Task publishFile() => client.PostAsync(url, content);

            // Assert
            var directoryNotFoundException = await Assert.ThrowsAsync<DirectoryNotFoundException>(publishFile);
        }

    }
}
