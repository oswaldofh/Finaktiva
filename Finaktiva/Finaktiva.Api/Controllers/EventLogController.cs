using Application.Features.EventLogs.Commands;
using Application.Features.EventLogs.Queries;
using Finaktiva.Api.Helper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finaktiva.Api.Controllers
{
    [Route("eventLog")]
    [ApiController]
    public class EventLogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventLogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listado de todos registros
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => (await _mediator.Send(new GetAllEventLogCommand())).ToActionResult();


        /// <summary>
        /// Se obtiene un registro pasando el id por parametro
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
            => (await _mediator.Send(new GetEventLogByIdCommand(id))).ToActionResult();


        /// <summary>
        /// Guarda un registro
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AddEventLogCommand command)
           => (await _mediator.Send(command)).ToActionResult();

        //=> Ok(await _mediator.Send(command.GetUserData(Request.Headers)));

        /// <summary>
        /// Actualiza un registro pasando el objeto
        /// </summary>
        /// <param name="command"></param>
        [HttpPut]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UpdateEventLogCommand command)
           => (await _mediator.Send(command)).ToActionResult();

        /// <summary>
        /// Se elimina un registro pasando el id por parametro
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<EventLogVm>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteById(int id)
            => (await _mediator.Send(new DeleteEventLogCommand(id))).ToActionResult();
    }
}
