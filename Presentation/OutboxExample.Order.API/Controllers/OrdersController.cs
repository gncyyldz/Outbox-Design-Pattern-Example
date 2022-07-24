using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutboxExample.Application.Features.Commands.CreateOrder;

namespace OutboxExample.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateOrderCommandRequest createOrderCommandRequest)
        {
            return Ok(await _mediator.Send(createOrderCommandRequest));
        }
    }
}
