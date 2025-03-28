using Application.Features.EventTypes.Commands;
using Application.Features.EventTypes.Queries;
using Finaktiva.Api.Helper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finaktiva.Api.Controllers
{
    [Route("eventType")]
    [ApiController]
    public class EventTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listado de todos registros
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => (await _mediator.Send(new GetAllEventTypeCommand())).ToActionResult();


        /// <summary>
        /// Se obtiene un registro pasando el id por parametro
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
            => (await _mediator.Send(new GetEventTypeByIdCommand(id))).ToActionResult();


        /// <summary>
        /// Guarda un registro
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AddEventTypeCommand command)
           => (await _mediator.Send(command)).ToActionResult();

        //=> Ok(await _mediator.Send(command.GetUserData(Request.Headers)));

        /// <summary>
        /// Actualiza un registro pasando el objeto
        /// </summary>
        /// <param name="command"></param>
        [HttpPut]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UpdateEventTypeCommand command)
           => (await _mediator.Send(command)).ToActionResult();

        /// <summary>
        /// Se elimina un registro pasando el id por parametro
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<EventTypeVm>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteById(int id)
            => (await _mediator.Send(new DeleteEventTypeCommand(id))).ToActionResult();
    }
}
