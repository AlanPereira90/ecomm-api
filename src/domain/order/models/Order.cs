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
      Delivery = Delivery.FromEntity(entity.Delivery),
      Items = Items.FromEntity(entity.Items)
    };
  }

  public OrderEntity ToEntity()
  {
    return new OrderEntity(
      Guid.Parse(this.Id),
      this.UserId,
      Guid.Parse(this.PaymentTypeId),
      this.Code,
      Enum.Parse<OrderStatus>(this.Status),
      this.Delivery.ToEntity(),
      this.Items.ToEntity()
    );
  }

  [BsonId]
  public string Id { get; set; }
  public string UserId { get; set; }
  public string PaymentTypeId { get; set; }
  public string Code { get; set; }
  public string Status { get; set; }
  [BsonElement("delivery")]
  public Delivery Delivery { get; set; }
  [BsonElement("items")]
  public Items Items { get; set; }
}

public class Delivery
{
  public Delivery(string address, string city, string state, string zipCode, string country)
  {
    this.Address = address;
    this.City = city;
    this.State = state;
    this.ZipCode = zipCode;
    this.Country = country;
  }

  public OrderDeliveryEntity ToEntity() => new OrderDeliveryEntity(
    address: this.Address,
    city: this.City,
    state: this.State,
    zipCode: this.ZipCode,
    country: this.Country
  );

  public static Delivery FromEntity(OrderDeliveryEntity entity) => new Delivery(
    address: entity.Address,
    city: entity.City,
    state: entity.State,
    zipCode: entity.ZipCode,
    country: entity.Country
  );

  public string Address { get; set; }
  public string City { get; set; }
  public string State { get; set; }
  public string ZipCode { get; set; }
  public string Country { get; set; }
}

public class Item
{
  public Item(string product, int quantity, double price)
  {
    this.Product = product;
    this.Quantity = quantity;
    this.Price = price;
  }

  public OrderItemEntity ToEntity() => new OrderItemEntity(
    product: this.Product,
    quantity: this.Quantity,
    price: this.Price
  );

  public static Item FromEntity(OrderItemEntity entity) => new Item(
    product: entity.Product,
    quantity: entity.Quantity,
    price: entity.Price
  );

  public string Product { get; set; }
  public int Quantity { get; set; }
  public double Price { get; set; }
}

public class Items : List<Item>
{
  public List<OrderItemEntity> ToEntity() => this.Select(item => item.ToEntity()).ToList();

  public static Items FromEntity(List<OrderItemEntity> entity)
  {
    var items = new Items();
    foreach (var item in entity)
    {
      items.Add(Item.FromEntity(item));
    }
    return items;
  }
}
