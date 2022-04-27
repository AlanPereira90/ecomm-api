using src.domain.order.enums;

namespace src.domain.order.entities;

public class OrderEntity
{
  public OrderEntity(
    Guid id,
    string userId,
    Guid paymentTypeId,
    string code,
    OrderStatus status,
    OrderDeliveryEntity delivery,
    List<OrderItemEntity> items)
  {
    this.Id = id;
    this.UserId = userId;
    this.PaymentTypeId = paymentTypeId;
    this.Code = code;
    this.Status = status;
    this.Delivery = delivery;
    this.Items = items;
  }

  public OrderEntity(string userId, Guid paymentTypeId, string code, OrderDeliveryEntity delivery, List<OrderItemEntity> items)
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

  public void Confirm()
  {
    this.Status = OrderStatus.PAID;
  }

  public void Finish()
  {
    this.Status = OrderStatus.FINISHED;
  }

  public Guid Id { get; private set; }
  public string UserId { get; private set; }
  public Guid PaymentTypeId { get; private set; }
  public string Code { get; private set; }
  public OrderStatus Status { get; private set; }
  public OrderDeliveryEntity Delivery { get; private set; }
  public List<OrderItemEntity> Items { get; private set; }
}

public class OrderDeliveryEntity
{
  public OrderDeliveryEntity(string address, string city, string state, string zipCode, string country)
  {
    this.Address = address;
    this.City = city;
    this.State = state;
    this.ZipCode = zipCode;
    this.Country = country;
  }

  public string Address { get; private set; }
  public string City { get; private set; }
  public string State { get; private set; }
  public string ZipCode { get; private set; }
  public string Country { get; private set; }
}

public class OrderItemEntity
{
  public OrderItemEntity(string product, int quantity, double price)
  {
    this.Product = product;
    this.Quantity = quantity;
    this.Price = price;
  }

  public string Product { get; private set; }
  public int Quantity { get; private set; }
  public double Price { get; private set; }
}
