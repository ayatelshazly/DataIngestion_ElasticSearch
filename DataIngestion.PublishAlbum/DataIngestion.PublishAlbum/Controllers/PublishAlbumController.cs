using DataIngestion.PublishAlbum.UseCases.Album;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace DataIngestion.PublishAlbum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishAlbumController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PublishAlbumController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        

        #region Album Publish events
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PublishAlbum(string readingPath)
        {
            var publishAlbumCommand = new PublishAlbumCommand(readingPath);
            await _mediator.Send(publishAlbumCommand);
            return NoContent();

        }
        #endregion

    }
}
