using MongoDB.Bson.Serialization.Attributes;

using src.domain.order.entities;
using src.domain.order.enums;

namespace src.domain.order.models;
public class Order
{
  public static Order FromEntity(OrderEntity entity)
  {
    return new Order
    {
      Id = entity.Id.ToString(),
      UserId = entity.UserId,
      PaymentTypeId = entity.PaymentTypeId.ToString(),
      Code = entity.Code,
      Status = entity.Status.ToString(),
      Delivery = entity.Delivery,
      Items = entity.Items
    };
  }

  public OrderEntity ToEntity()
  {
    return new OrderEntity(
      Guid.Parse(this.Id),
      this.UserId,
      Guid.Parse(this.PaymentTypeId),
      this.Code, Enum.Parse<OrderStatus>(this.Status),
      this.Delivery,
      this.Items
    );
  }

  [BsonId]
  public string Id { get; private set; }
  public string UserId { get; private set; }
  public string PaymentTypeId { get; private set; }
  public string Code { get; private set; }
  public string Status { get; private set; }
  public Object Delivery { get; private set; }
  public Object Items { get; private set; }
}
