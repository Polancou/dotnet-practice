using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ECommerceAPI.Application.Commands;

namespace ECommerceAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            if (command == null || command.Items == null || command.Items.Count == 0)
            {
                return BadRequest("Invalid order request.");
            }

            try
            {
                var orderId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetOrder), new { id = orderId }, orderId);
            }
            catch (Exception ex)
            {
                // Simple error handling
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(Guid id)
        {
            // Placeholder: Not fully implemented yet
            return Ok($"Order {id} details would be here.");
        }
    }
}
