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
}
