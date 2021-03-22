using DataIngestion.SubscribeAlbum.Services;
using Events;
using Events.Models.ReferenceModel;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DataIngestion.SubscribeAlbum.UnitTest
{
    public class InjectAlbumTest
    {


        [Theory]
        [InlineData(1, "elasticSearchTest",2, "Google.com", "Succeed", "imageUrl.com")]
        public async Task InjectAlbumToElastic_CorrectData_Succeed(long id, string name,long upc, string url, string label, string imageUrl)
        {
         

            var elasticClient = new Mock<IElasticClient>();

            //Arrange
            
            var albumModelEventlist = new List<AlbumModelEvent>()
            {
                new AlbumModelEvent() {


                    Collection = new CollectionRefModel()
                    {
                        Id = id,
                        Name = name,
                        UPC = upc,
                        URL = url,
                        ReleaseDate = DateTime.Now,

                        IsCompilation = true,

                        Label = label,
                        ImageUrl = imageUrl

                    },
                    Artist = new List<ArtistRefModel>() {
                             new ArtistRefModel()
                             {
                                Id = 1,
                                Name = "Nancy Agram"
                             }
                    }
                }
                };
            

            var mockSearchResponse = new Mock<ISearchResponse<AlbumModelEvent>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(albumModelEventlist);
 
            var mockElasticClient = new Mock<IElasticClient>();

            mockElasticClient.Setup(x => x
                .Search(It.IsAny<Func<SearchDescriptor<AlbumModelEvent>, ISearchRequest>>()))
            .Returns(mockSearchResponse.Object);
             
            var service =new  AlbumService(mockElasticClient.Object);

            await service.InjectAlbumAsync(albumModelEventlist.First());
            var albumModelEventExpected = service.GetResult();
            Assert.NotNull(albumModelEventExpected);
            Assert.True(albumModelEventExpected.Count()>0);




        }
    }

}



