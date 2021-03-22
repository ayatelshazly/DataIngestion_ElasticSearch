using System;
using System.Threading.Tasks;
using Broker.Helpers;
using Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Dapr;
using DataIngestion.SubscribeAlbum.UseCases.Album;
using Microsoft.Extensions.Logging;

namespace DataIngestion.SubscribeAlbum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
         private readonly IMediator _mediator;
        private readonly ILogger<AlbumController> _logger;

        public AlbumController(IMediator mediator, ILogger<AlbumController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }


        #region SubscribeAlbumEvents
        [Topic(BrokerHelper.broker, EventNames.PUBLISHALBUM)]
        [Route("/SubscribeToPublishAlbumEvent")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SubscribeToPublishAlbumEvent([FromBody] Event publishedAlbumEvent)
        {
            if (publishedAlbumEvent?.Data != null)
            {
 
                var albumModel = JsonConvert.DeserializeObject<AlbumModelEvent>(publishedAlbumEvent.Data.ToString());
                _logger.LogInformation("start subscribe Collection ID {0} ", albumModel.Collection.Id);

                var injectAlbumCommand = new InjectAlbumCommand(albumModel);
                await _mediator.Send(injectAlbumCommand);
            }
            return Ok();
        }
        #endregion


    }
}
