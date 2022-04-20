using Microsoft.AspNetCore.Mvc;

using src.application.dtos;
using src.domain.payment_type.interfaces;

namespace src.application.controllers.payment_type;

[ApiController]
[Route("/payment-type")]
public class PaymentTypeController : ControllerBase
{
  private readonly IPaymentTypeService _paymentTypeService;
  public PaymentTypeController(IPaymentTypeService paymentTypeService)
  {
    _paymentTypeService = paymentTypeService;
  }

  [HttpPost]
  public async Task<IActionResult> CreatePaymentType([FromBody] PaymentTypeDto dto)
  {
    var result = await _paymentTypeService.Create(dto.ToDomain());
    return Created($"/payment-type/{result}", new { id = result });
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<IActionResult> GetPaymentType([FromRoute] string id)
  {
    var result = await _paymentTypeService.FindOne(Guid.Parse(id));
    return Ok(result);
  }

  [HttpPut]
  [Route("{id}")]
  public async Task<IActionResult> UpdatePaymentType([FromRoute] string id, [FromBody] PaymentTypeDto dto)
  {
    //TODO: CREATE A NEW DTO WITHOUT REQUIRED FIELDS, AND THE SERVICE LAYER
    //SHOUD HANDLE WITH THE FIEDS ATTRIBUTION
    var result = await _paymentTypeService.UpdateOne(dto.ToDomain(Guid.Parse(id)));
    return Accepted($"/payment-type/{result}", new { id = result });
  }

}
