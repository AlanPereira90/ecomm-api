using src.domain.order.enums;

namespace src.domain.order.entities;

public class OrderEntity
{
  public OrderEntity(Guid id, string userId, Guid paymentTypeId, string code, OrderStatus status, Object delivery, Object items)
  {
    this.Id = id;
    this.UserId = userId;
    this.PaymentTypeId = paymentTypeId;
    this.Code = code;
    this.Status = status;
    this.Delivery = delivery;
    this.Items = items;
  }

  public OrderEntity(string userId, Guid paymentTypeId, string code, Object delivery, Object items)
  {
    this.Id = Guid.NewGuid();
    this.UserId = userId;
    this.PaymentTypeId = paymentTypeId;
    this.Code = code;
    this.Status = OrderStatus.CREATED;
    this.Delivery = delivery;
    this.Items = items;
  }

  public void Cancel()
  {
    this.Status = OrderStatus.CANCELLED;
  }

  public Guid Id { get; private set; }
  public string UserId { get; private set; }
  public Guid PaymentTypeId { get; private set; }
  public string Code { get; private set; }
  public OrderStatus Status { get; private set; }
  public Object Delivery { get; private set; }
  public Object Items { get; private set; }
}
