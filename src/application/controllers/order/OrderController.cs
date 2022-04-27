using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using src.application.dtos;
using src.domain.order.interfaces;

namespace src.application.controllers.payment_type;

[ApiController]
[Route("/order")]
public class OrderController : ControllerBase
{
  private readonly IOrderService _orderService;
  public OrderController(IOrderService OrderService)
  {
    _orderService = OrderService;
  }

  [HttpPost]
  public async Task<IActionResult> CreateOrder(
    [FromBody] OrderDto dto,
    [FromHeader(Name = "user-id")][Required] string userId
  )
  {
    var result = await _orderService.Create(dto.ToDomain(userId));
    return Created($"/order/{result}", new { id = result });
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<IActionResult> GetOrder(
    [FromRoute] string id,
    [FromHeader(Name = "user-id")][Required] string userId
  )
  {
    var result = await _orderService.FindOne(Guid.Parse(id), userId);
    return Ok(OrderDto.FromDomain(result));
  }

  [HttpPatch]
  [Route("{id}/cancel")]
  public async Task<IActionResult> CancelOrder(
    [FromRoute] string id,
    [FromHeader(Name = "user-id")][Required] string userId
  )
  {
    var result = await _orderService.Cancel(Guid.Parse(id), userId);
    return Accepted($"/order/{id}");
  }

  [HttpPatch]
  [Route("{id}/confirm")]
  public async Task<IActionResult> ConfirmOrder(
    [FromRoute] string id,
    [FromHeader(Name = "user-id")][Required] string userId
  )
  {
    var result = await _orderService.Confirm(Guid.Parse(id), userId);
    return Accepted($"/order/{id}");
  }

  [HttpPatch]
  [Route("{id}/finish")]
  public async Task<IActionResult> FinishOrder(
    [FromRoute] string id,
    [FromHeader(Name = "user-id")][Required] string userId
  )
  {
    var result = await _orderService.Finish(Guid.Parse(id), userId);
    return Accepted($"/order/{id}");
  }
}
