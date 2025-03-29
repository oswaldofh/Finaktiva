using Application.Features.EventLogs.Queries;
using Application.Features.ExceptionLogs.Queries;
using Finaktiva.Api.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finaktiva.Api.Controllers
{
    [Route("exceptionLog")]
    [ApiController]
    public class ExceptionLogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExceptionLogController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Listado de todos registros
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => (await _mediator.Send(new GetAllExceptionLogCommand())).ToActionResult();
    }
}
