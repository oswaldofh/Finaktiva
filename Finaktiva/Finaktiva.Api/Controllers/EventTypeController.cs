using Application.Features.EventTypes.Commands;
using Application.Features.EventTypes.Queries;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [ProducesResponseType(typeof(IEnumerable<EventTypeVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public async Task<IEnumerable<EventTypeVm>> GetAll()
           => await _mediator.Send(new GetAllEventTypeCommand());


        /// <summary>
        /// Se obtiene un registro pasando el id por parametro
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EventTypeVm), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventTypeVm>> GetById(int id)
            => Ok(await _mediator.Send(new GetEventTypeByIdCommand(id)));


        /// <summary>
        /// Guarda un registro
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        [ProducesResponseType(typeof(EventTypeVm), (int)HttpStatusCode.Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventTypeVm>> Create([FromBody] AddEventTypeCommand command)
           => Ok(await _mediator.Send(command));
        //=> Ok(await _mediator.Send(command.GetUserData(Request.Headers)));

        /// <summary>
        /// Actualiza un registro pasando el objeto
        /// </summary>
        /// <param name="command"></param>
        [HttpPut]
        [ProducesResponseType(typeof(EventTypeVm), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventTypeVm>> Update([FromBody] UpdateEventTypeCommand command)
           => Ok(await _mediator.Send(command));

        /// <summary>
        /// Se elimina un registro pasando el id por parametro
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> DeleteById(int id)
            => Ok(await _mediator.Send(new DeleteEventTypeCommand(id)));
    }
}
