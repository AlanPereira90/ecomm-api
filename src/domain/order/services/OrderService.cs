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

  private async Task<OrderEntity> GetOrder(Guid id, string userId)
  {
    var result = await _orderRepository.FindOne(id);

    if (result == null)
    {
      throw new ResponseError(HttpStatusCode.NotFound, "Order not found");
    }

    if (result.UserId != userId)
    {
      throw new ResponseError(HttpStatusCode.Forbidden, "Order does not belong to user");
    }

    return result;
  }

  public async Task<Guid> Create(OrderEntity order)
  {
    await ValidatePaymentType(order.PaymentTypeId);

    var result = await _orderRepository.Create(order);
    return result.Id;
  }

  public async Task<OrderEntity> FindOne(Guid id, string userId)
  {
    return await this.GetOrder(id, userId);
  }
}
