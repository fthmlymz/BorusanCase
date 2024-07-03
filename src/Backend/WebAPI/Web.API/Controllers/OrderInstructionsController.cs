using Application.Features.OrderInstructions.Commands.CancelledOrderInstructions;
using Application.Features.OrderInstructions.Commands.ChangeOrderInstructionStatus;
using Application.Features.OrderInstructions.Commands.CreateOrderInstruction;
using Application.Features.OrderInstructions.Queries.OrderInstructionInActiveSearch;
using Application.Features.OrderInstructions.Queries.OrderInstructionSearch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderInstructionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderInstructionsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrderInstruction([FromBody] CreateOrderInstructionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeOrderInstructionStatus([FromBody] ChangeOrderInstructionStatusCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<PaginatedResult<GetOrderInstructionPaginationDto>>> GetOrders([FromQuery] GetOrderInstructionPaginationQuery dto)
        {
            return await _mediator.Send(dto);
        }


        [HttpPut("cancel")]
        public async Task<IActionResult> CancelOrderInstruction([FromBody] CancelOrderInstructionCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        [Route("in-active-search")]
        public async Task<ActionResult<PaginatedResult<GetOrderInstructionInactivePaginationDto>>> GetOrders([FromQuery] GetOrderInstructionInActivePaginationQuery dto)
        {
            return await _mediator.Send(dto);
        }
    }
}
