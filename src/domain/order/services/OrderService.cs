using System.Net;

using src.domain.order.entities;
using src.domain.order.interfaces;
using src.domain.payment_type.interfaces;

namespace src.domain.order.services;

public class OrderService : IOrderService
{
  private readonly IOrderRepository _orderRepository;
  private readonly IPaymentTypeRepository _paymentTypeRepository;
  public OrderService(IOrderRepository orderRepository, IPaymentTypeRepository paymentTypeRepository)
  {
    _orderRepository = orderRepository;
    _paymentTypeRepository = paymentTypeRepository;
  }

  private async Task<bool> ValidatePaymentType(Guid paymentTypeId)
  {
    var paymentType = await _paymentTypeRepository.FindOne(paymentTypeId);
    if (paymentType == null)
    {
      throw new ResponseError(HttpStatusCode.UnprocessableEntity, "Invalid payment type");
    }

    if (!paymentType.Enabled)
    {
      throw new ResponseError(HttpStatusCode.UnprocessableEntity, "Payment type is disabled");
    }
    return true;
  }

  public async Task<Guid> Create(OrderEntity order)
  {
    //TODO: create order, validate payment type (should exist and should be enabled)
    await ValidatePaymentType(order.PaymentTypeId);

    var result = await _orderRepository.Create(order);
    return result.Id;
  }
}
